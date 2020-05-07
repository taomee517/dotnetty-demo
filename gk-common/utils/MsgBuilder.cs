using System;
using DotNetty.Buffers;
using DotNetty.Transport.Channels;
using gk_common.beans;
using gk_common.constants;
using Newtonsoft.Json;

namespace gk_common.utils
{
    public class MsgBuilder
    {

        public static void BuildResp(IChannelHandlerContext context, Message message)
        {
            var respHbType = Convert.ToInt32(HeartBeatType.KeepAlive);
            var coreRespContent = BytesUtil.Int32ToBytes(respHbType);
            var serial = message.Bodies[0].SerialNumber;
            var content = BuildContent(IdType.HeartBeat, MsgType.State, OpsType.ReportCommand,  0, serial, coreRespContent);
        }


        /**
         * 如果是响应-isAck=true, control = 0x1880
         * 如果是命令 isAck=false, control = 0x9880
         */
        public static byte[] BuildMessage(string dst, string src, int version, bool isAck, int send, int receive, byte[] content)
        {
            var buffer = Unpooled.Buffer(content.Length + GkDefault.Min_Length);
            buffer.WriteByte(GkDefault.Starter);

            buffer.WriteBytes(BytesUtil.Hex2Bytes(dst));
            buffer.WriteBytes(BytesUtil.Hex2Bytes(src));
            
            buffer.WriteByte(version);
            var control = isAck ? 0x1880 : 0x9880;
            buffer.WriteBytes(BytesUtil.Int16ToBytes((Int16)control));

            buffer.WriteByte(send);
            buffer.WriteByte(receive);
            buffer.WriteBytes(BytesUtil.Int16ToBytes((Int16) content.Length));
            buffer.WriteBytes(content);

            var crc = new byte[2];
            buffer.WriteBytes(crc);

            return buffer.Array;
        }


        public static byte[] BuildContent(IdType idType, MsgType msgType, OpsType opsType, int attr, int serial, byte[] core)
        {
            var coreLen = core == null ? 0 : core.Length;
            var buffer = Unpooled.Buffer( coreLen + 20);
            
            var msgId = Convert.ToInt32(idType);
            var msgIdBytes = BytesUtil.Int32ToBytes(msgId);
            buffer.WriteBytes(msgIdBytes);

            var timestamp = TimeUtil.GetTimeStamp();
            var timeBytes = BytesUtil.Int32ToBytes(timestamp);
            buffer.WriteBytes(timeBytes);

            var msgTypeCode = Convert.ToInt16(msgType);
            var msgTypeBytes = BytesUtil.Int16ToBytes(msgTypeCode);
            buffer.WriteBytes(msgTypeBytes);
            
            var opsTypeCode = Convert.ToInt16(opsType);
            var opsTypeBytes = BytesUtil.Int16ToBytes(opsTypeCode);
            buffer.WriteBytes(opsTypeBytes);
            
            var attrBytes = BytesUtil.Int16ToBytes((Int16)attr);
            buffer.WriteBytes(attrBytes);

            var length = coreLen + 4;
            var lenBytes = BytesUtil.Int16ToBytes((Int16)length);
            buffer.WriteBytes(lenBytes);
            
            var serialBytes = BytesUtil.Int32ToBytes(serial);
            buffer.WriteBytes(serialBytes);

            if (core!=null)
            {
                buffer.WriteBytes(core);
            }
            return buffer.Array;

        }
    }
}