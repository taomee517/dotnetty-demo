// 创建人：taomee
// 创建时间：2020/06/02 14:50

using System;
using DotNetty.Buffers;
using Lora_Common.Beans;
using Lora_Common.Constant;
using Lora_Common.Util;

namespace Lora_Common.SDK
{
    public class LoraParser
    {
        public static byte[] Split(IByteBuffer buffer)
        {
            
            var rdx = 0;
            var crcBytes = new byte[2];
            var crc = 0;
            var length = 0;
            var valid = false;
            while (true)
            {
                // 可读字节数小于最小报文长度
                if (buffer.ReadableBytes<LoraConst.MinLength)
                {
                    return null;
                }
                // 确定帧头的位置
                if (LoraConst.StartSign != buffer.GetByte(rdx))
                {
                    rdx = GetStartIndex(buffer, LoraConst.StartSign);
                }

                // 没有找到帧头
                if (rdx == -1) return null;
                buffer.SetReaderIndex(rdx);
                var lengthBytes = new byte[2];
                buffer.GetBytes(rdx + 23, lengthBytes);
                length = BytesUtil.Bytes2Int16(lengthBytes);
                if (buffer.ReadableBytes<LoraConst.MinLength + length)
                {
                    rdx++;
                    buffer.SetReaderIndex(rdx);
                    continue;
                }
                var content = new byte[25 + length];
                buffer.GetBytes(rdx, content);
                buffer.GetBytes(rdx + 25 + length, crcBytes);
                crc = CRC16.calcCrc16(content);
                valid = BytesUtil.ByteArrayEquals(crcBytes, BytesUtil.Int16ToBytes((Int16) crc));
                if (!valid)
                {
                    rdx++;
                    buffer.SetReaderIndex(rdx);
                    continue;
                }
                var msg = new byte[LoraConst.MinLength + length];
                buffer.ReadBytes(msg);
                return msg;
            }
        }


        public static Header HeaderParse(byte[] msg)
        {
            var buffer = Unpooled.WrappedBuffer(msg);
            return HeaderParse(buffer);
        }
        
        public static Header HeaderParse(IByteBuffer buffer)
        {
            // start sign
            var startSign = buffer.ReadByte();
            // task sn
            var taskId = buffer.ReadByte();
            // msg sn
            var msgSn = buffer.ReadByte();
            // ttl
            var ttl = buffer.ReadByte();
            // 通信方式
            // 0x81 - 以太网,  0x82 - GPRS , 0x83 - NBIOT
            var transportCode = buffer.ReadByte();
            var transportType = (TransportType)transportCode;
            // 时间戳
            var timestamp = buffer.ReadInt();
            var date = TimestampUtil.ConvertSeconds2DateTime(timestamp).AddHours(8);
            // 控制命令 心跳 - 0x4064
            var funCode = buffer.ReadShort();
            var funType = (FunType) funCode;
            // src mac - 6B
            var srcMacBytes = new byte[6];
            buffer.ReadBytes(srcMacBytes);
            // dst mac
            var dstMacBytes = new byte[6];
            buffer.ReadBytes(dstMacBytes);
            var srcMac = BytesUtil.BytesToHex(srcMacBytes);
            var dstMac = BytesUtil.BytesToHex(dstMacBytes);
            // content length 
            var length = buffer.ReadShort();
            byte[] content = null;
            if (length>0)
            {
                content = new byte[length];
                buffer.ReadBytes(content);
            }
            
            var crc = new byte[2];
            buffer.ReadBytes(crc);
            var header = new Header()
            {
                TaskId = taskId,
                MsgSn = msgSn,
                Ttl = ttl,
                TransportType = transportType,
                Time = date,
                FunType = funType,
                SrcMac = srcMac,
                DstMac = dstMac,
                Length = length,
                Content = content,
                crc = crc
            };
            return header;
        }

        private static int GetStartIndex(IByteBuffer buffer, byte startSign)
        {
            for (var i = buffer.ReaderIndex;i<buffer.WriterIndex;i++)
            {
                var temp = buffer.GetByte(i);
                if (temp == startSign)
                {
                    return i;
                }
            }

            return -1;
        }
    }
}