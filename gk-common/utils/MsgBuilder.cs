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
            var serial = message.Body.SerialNumber;
            var content = BuildContent(IdType.HeartBeat, MsgType.State, OpsType.ReportCommand,  0, serial, coreRespContent);
        }


        public static byte[] BuildMessage(string dst, string src, int version, int control, int send, int receive, byte[] content)
        {
            var buffer = Unpooled.Buffer(content.Length + GkDefault.Min_Length);
            buffer.WriteByte(GkDefault.Starter);

            buffer.WriteBytes(BytesUtil.Hex2Bytes(dst));
            buffer.WriteBytes(BytesUtil.Hex2Bytes(src));
            
            buffer.WriteByte(version);
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
            var buffer = Unpooled.Buffer(core.Length + 20);
            
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
            buffer.WriteBytes(opsTypeBytes);

            var length = core.Length + 4;
            var lenBytes = BytesUtil.Int16ToBytes((Int16)length);
            buffer.WriteBytes(lenBytes);
            
            var serialBytes = BytesUtil.Int32ToBytes(serial);
            buffer.WriteBytes(serialBytes);

            buffer.WriteBytes(core);
            return buffer.Array;

        }
    }
}