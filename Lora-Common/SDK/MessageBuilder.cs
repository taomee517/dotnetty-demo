using System;
using DotNetty.Buffers;
using Lora_Common.Constant;
using Lora_Common.Util;

namespace Lora_Common.SDK
{
    public class MessageBuilder
    {
        public static byte[] BuildHeartBeat(string mac)
        {
            // 0x81 - 以太网,  0x82 - GPRS , 0x83 - NBIOT
            return BuildMessage(0, TransportType.GRPS, FunType.HeartBeat, mac, null);
        }
        
        
        public static byte[] BuildMessage(int taskId, TransportType transportType, FunType funType, string mac, byte[] content)
        {
            var buffer = Unpooled.Buffer(100);
            try
            {
                // start sign
                buffer.WriteByte(LoraConst.StartSign);
                // task sn
                buffer.WriteByte(taskId);
                // msg sn
                buffer.WriteByte(SearialUtil.GetSerial(mac));
                // ttl
                buffer.WriteByte(0);
                // 通信方式
                // 0x81 - 以太网,  0x82 - GPRS , 0x83 - NBIOT
                buffer.WriteByte((int)transportType);
                // 时间戳
                buffer.WriteInt(TimestampUtil.GetUtcSecondsStamp());
                // 控制命令 心跳 - 0x4064
                buffer.WriteShort((int)funType);
                // src mac - 6B
                var macBytes = BytesUtil.Hex2Bytes(mac);
                buffer.WriteBytes(macBytes);
                // dst mac
                buffer.WriteBytes(macBytes);
                // content length 
                if (content!=null)
                {
                    buffer.WriteShort(content.Length);
                    buffer.WriteBytes(content);
                }
                else
                {
                    buffer.WriteShort(0);
                }
            
                var validatingData = new byte[buffer.ReadableBytes];
                buffer.GetBytes(buffer.ReaderIndex,validatingData);
                // crc
                var crc = CRC16.calcCrc16(validatingData);
                buffer.WriteShort(crc);

                var msg = new byte[buffer.ReadableBytes];
                buffer.ReadBytes(msg);
                return msg;
            }
            finally
            {
                buffer.Release();
            }
        }


        public static byte[] BuildSensorMsg(string mac,int port, SensorType sensorType, byte[] coreData)
        {
            var buffer = Unpooled.Buffer(1024);
            try
            {
                var settingCoreSize = 1;
                var detectInterval = 5;
                var coreSize = coreData.Length / 21;
                buffer.WriteByte(settingCoreSize);
                buffer.WriteShort(detectInterval);
                buffer.WriteByte(coreSize);
                
                var macBytes = BytesUtil.Hex2Bytes(mac);
                buffer.WriteBytes(macBytes);
                buffer.WriteByte(port);
                buffer.WriteShort((int)sensorType);
                buffer.WriteInt(TimestampUtil.ConvertDateTime2Seconds(DateTime.Now));
                buffer.WriteBytes(coreData);
                var msg = new byte[buffer.ReadableBytes];
                buffer.ReadBytes(msg);
                return msg;
            }
            finally
            {
                buffer.Release();
            }
        }

        public static byte[] BuildCrackData(int distance, float temp)
        {
            var buffer = Unpooled.Buffer(8);
            try
            {
                buffer.WriteInt(distance);
                buffer.WriteFloat(temp);
                var msg = new byte[buffer.ReadableBytes];
                buffer.ReadBytes(msg);
                return msg;
            }
            finally
            {
                buffer.Release();
            }
        }
        
        public static byte[] BuildSettleData(int unmarkHeight, int temp, float height)
        {
            var buffer = Unpooled.Buffer(8);
            try
            {
                var unmarkData = unmarkHeight / 2000 * 0x4AFFFF;
                var unmarkBytes = new byte[] { (byte)(unmarkData>>16&0xff), (byte)(unmarkData>>8&0xff),(byte)(unmarkData&0xff)};
                buffer.WriteBytes(unmarkBytes);
                buffer.WriteByte(temp);
                buffer.WriteFloat(height);
                var msg = new byte[buffer.ReadableBytes];
                buffer.ReadBytes(msg);
                return msg;
            }
            finally
            {
                buffer.Release();
            }
        }
    }
}