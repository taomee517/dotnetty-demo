using System.Collections.Generic;
using DotNetty.Buffers;
using DotNetty.Codecs;
using DotNetty.Transport.Channels;
using gk_common.utils;

namespace gk_udp_server.handler
{
    public class FrameSplitDecoder : ByteToMessageDecoder
    {
        protected override void Decode(IChannelHandlerContext context, IByteBuffer input, List<object> output)
        {
            try{
                var buffer = GkParser.Split(input);
                if(buffer==null) return;
                output.Add(buffer);
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