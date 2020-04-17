using System.Collections.Generic;
using DotNetty.Buffers;
using DotNetty.Codecs;
using DotNetty.Transport.Channels;
using dotnetty_server.utils;

namespace dotnetty_server.handler
{
    public class FrameSplitHandler: ByteToMessageDecoder
    {
        protected override void Decode(IChannelHandlerContext context, IByteBuffer input, List<object> output)
        {
            try
            {
                var bytes = ParseUtil.Split(input);
                if(bytes==null) return;
                    output.Add(bytes);
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
            if (!(left > 0 && buffer.ReaderIndex > 0)) return; 
                for (var index = 0; index < left; index++)
                    buffer.SetByte(index, buffer.GetByte(index + start));
            buffer.SetIndex(0, left);
        }
    }
    
   
}