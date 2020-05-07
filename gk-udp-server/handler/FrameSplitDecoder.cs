using System.Collections.Generic;
using DotNetty.Buffers;
using DotNetty.Codecs;
using DotNetty.Transport.Channels;
using DotNetty.Transport.Channels.Sockets;
using gk_common.utils;
using NLog;

namespace gk_udp_server.handler
{
    public class FrameSplitDecoder : MessageToMessageDecoder<DatagramPacket>
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();


        protected override void Decode(IChannelHandlerContext context, DatagramPacket message, List<object> output)
        {
            Logger.Info("FrameSplitDecoder 收到server端消息");
            var input = message.Content;
            try
            {
                var buffer = GkParser.Split(input);
                if(buffer==null) return;
                output.Add(new DatagramPacket(buffer,message.Sender));
            }
            finally
            {
                ResetBuffer(input);
            }
        }

        private void ResetBuffer(IByteBuffer buffer)
        {
            var left = buffer.ReadableBytes;
            var start = buffer.ReaderIndex;
            if (left == 0 && buffer.ReaderIndex > 0)
            {
                buffer.SetIndex(0, 0);
                return;
            }
            if (!(left > 0 && buffer.ReaderIndex > 0)) return; 
            for (var index = 0; index < left; index++) 
                buffer.SetByte(index, buffer.GetByte(index + start));
            buffer.SetIndex(0, left);
        }
    }
}