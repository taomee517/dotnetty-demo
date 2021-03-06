﻿using System;
using System.Collections.Generic;
using DotNetty.Buffers;
using DotNetty.Common.Utilities;
using gk_common.beans;
using gk_common.constants;
using Attribute = gk_common.beans.Attribute;

namespace gk_common.utils
{
    public class GkParser
    {
        /// <summary>
        /// 基康设备的拆包方法
        /// </summary>
        /// <param name="input">收到的字节流</param>
        /// <returns>截取的单个完整消息</returns>
        public static IByteBuffer Split(IByteBuffer input)
        {
            var inputLen = input.ReadableBytes;
            //先判断是否能找到帧头,并设置读下标
            var startIndex = FindStarterIndex(input);
            if (startIndex == -1) return null;
            input.SetReaderIndex(startIndex);
            
            //判断帧头后的可读长度是否小于最小消息长度
            var maxMsgLen = inputLen - startIndex;
            if (maxMsgLen < GkDefault.Min_Length) return null;
            
            //获取信息单元长度，起始下标14，占两个字节
            var srcContentLengthBuffer = Unpooled.Buffer(2);
            input.GetBytes(14, srcContentLengthBuffer);
            var contentLength = srcContentLengthBuffer.ReadUnsignedShort();
            
            //判断帧头后的可读长度是否小于本条消息实际长度
            var msgLength = GkDefault.Min_Length + contentLength;
            if (maxMsgLen < msgLength) return null;
            
            //读取本条消息的完整内容
            // var data = new byte[msgLength];
            var buffer = PooledByteBufferAllocator.Default.Buffer(msgLength);
            input.ReadBytes(buffer);
            return buffer;
        }

        
        public static BaseMessage Decode(IByteBuffer msg)
        {
            var head = msg.ReadByte();
            
            var addr = new byte[4];
            msg.ReadBytes(addr);
            var dst = BytesUtil.BytesToHex(addr);

            msg.ReadBytes(addr);
            var src = BytesUtil.BytesToHex(addr);

            var version = (int) msg.ReadByte();
            
            var ctrlValue = msg.ReadShort();
            var ctrlType = ctrlValue >> 15 & 0x1;
            var isEncrypt = (ctrlValue >> 14 & 1) == 1;
            var hasSignature = (ctrlValue >> 13 & 1) == 1;
            var hasStarter = (ctrlValue >> 12 & 1) == 1;
            var hasEndSign = (ctrlValue >> 11  & 1)== 1;
            var logicSignal = ctrlValue >> 9 & 3;
            var ackType = ctrlValue >> 7 & 3;
            var ackStatus = ctrlValue >> 5 & 3;
            var ctrl = new Control();  
            ctrl.SrcValue = ctrlValue;
            ctrl.ControlType = ctrlType;
            ctrl.IsEncrypt = isEncrypt;
            ctrl.HasSignature = hasSignature;
            ctrl.HasStarter = hasStarter;
            ctrl.HasEndSign = hasEndSign;
            ctrl.LogicSignal = logicSignal;
            ctrl.AckType = ackType;
            ctrl.AckStatus = ackStatus;
            
            var send = (int) msg.ReadByte();
            
            var receive = (int) msg.ReadByte();

            var contentLength = msg.ReadUnsignedShort();
            var header = new BaseHeader
            {
                Head = head,
                Dst = dst,
                Src = src,
                Ver = version,
                Control = ctrl,
                Send = send,
                Receive = receive,
                Length = contentLength,
            };
            
            var content = PooledByteBufferAllocator.Default.Buffer(contentLength);
            msg.ReadBytes(content);
            return new BaseMessage
            {
                Header = header,
                Content = content
            };
        }

        public static Message BodyParse(BaseMessage message)
        {
            var buffer = message.Content;
            try
            {
                var bodies = new List<BaseBody>();
                do
                {
                    var msgId = buffer.ReadInt();

                    var srcTime = buffer.ReadInt();
                    var targetTime = TimeUtil.BuildTime(srcTime);

                    var msgType = buffer.ReadUnsignedShort();
                    var opsType = buffer.ReadUnsignedShort();

                    var msgAttr = buffer.ReadUnsignedShort();
                    var ackSuccess = (msgAttr >> 15 & 1) == 0;
                    var infoValueTypeValue = msgAttr >> 13 & 3;
                    var infoValueType = EnumUtil.ToEnum<InfoValueType>(infoValueTypeValue);
                    var dataSize = (msgAttr >> 10 & 7) + 1;
                    var reportTypeValue = msgAttr >> 8 & 3;
                    var reportType = EnumUtil.ToEnum<ReportType>(reportTypeValue);
                    var isTest = (msgAttr >> 7 & 1) == 1;
                    var isHistory = (msgAttr >> 6 & 1) == 1;
                    var isRepeat = (msgAttr >> 5 & 1) == 1;
                    var force = (msgAttr >> 4 & 1) == 1;
                    var digitType = msgAttr & 7;

                    var attr = new Attribute();
                    attr.AckSuccess = ackSuccess;
                    attr.ValueType = infoValueType;
                    attr.DataSize = dataSize;
                    attr.ReportType = reportType;
                    attr.IsTest = isTest;
                    attr.IsHistory = isHistory;
                    attr.IsRepeat = isRepeat;
                    attr.Force = force;
                    attr.DigitType = EnumUtil.ToEnum<DigitType>(digitType);

                    var coreContentLen = buffer.ReadUnsignedShort();
                    var serial = buffer.ReadInt();
                    var coreContent = new byte[coreContentLen - 4];
                    buffer.ReadBytes(coreContent);

                    var body = new BaseBody();
                    body.IdType = EnumUtil.ToEnum<IdType>(msgId);
                    body.Time = targetTime;
                    body.MsgType = EnumUtil.ToEnum<MsgType>(msgType);
                    body.OpsType = EnumUtil.ToEnum<OpsType>(opsType);
                    body.Attribute = attr;
                    body.Length = coreContentLen;
                    body.SerialNumber = serial;
                    body.CoreMsg = coreContent;
                    bodies.Add(body);
                } while (buffer.ReadableBytes>0);
                return new Message
                {
                    Header = message.Header,
                    Bodies = bodies
                };
            }
            finally
            {
                buffer.SafeRelease();
            }
        }
        
        public static void ParseCore(ref Message msg)
        {
            var bodies = msg.Bodies;
            foreach (var body in bodies)
            {
                var core = body.CoreMsg;
                if(core == null) return;
                var buffer = Unpooled.WrappedBuffer(core);
                switch (body.IdType)
                {
                    case IdType.HeartBeat:
                        var heartBeatType = EnumUtil.ToEnum<HeartBeatType>(buffer.ReadInt());
                        var keepAliveDuration = buffer.ReadInt();
                        var hb = new HeartBeatInfo();
                        hb.HeartBeatType = heartBeatType;
                        hb.KeepAliveDuration = keepAliveDuration;
                        body.HeartBeatInfo = hb;
                        break;
                    case IdType.BatteryInfo:
                        var voltage = buffer.ReadFloat();
                        var temp = buffer.ReadFloat();
                        var capa = buffer.ReadFloat();
                        var battery = new BatteryData();
                        battery.voltage = voltage;
                        battery.batteryTemp = temp;
                        battery.capacity = capa;
                        body.BatteryData = battery;
                        break;
                    case IdType.SensorData:
                        var sensorValues = new List<double>();
                        do
                        {
                            var value = buffer.ReadFloat();
                            sensorValues.Add(value);
                        } while (buffer.ReadableBytes > 0);
                        var sensor = new SensorData();
                        sensor.UnparsedData = sensorValues;
                        body.SensorData = sensor;
                        break;
                    default:
                        break;
                }
            }
        }
        
        
        private static int FindStarterIndex(IByteBuffer input)
        {
            //等同于for循环去查找input中starter的下标，没有就返回-1
            return input.ForEachByte(new IndexOfProcessor(GkDefault.Starter));
        }
    }
}