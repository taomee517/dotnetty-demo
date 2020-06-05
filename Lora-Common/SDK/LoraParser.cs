// 创建人：taomee
// 创建时间：2020/06/02 14:50

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
            return null;
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
    }
}