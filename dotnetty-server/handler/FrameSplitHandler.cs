using System.Collections.Generic;
using DotNetty.Buffers;
using DotNetty.Codecs;
using DotNetty.Transport.Channels;
using dotnetty_server.logger;
using dotnetty_server.utils;
using NLog;

namespace dotnetty_server.handler
{
    public class FrameSplitHandler: ByteToMessageDecoder
    {
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();
        
        protected override void Decode(IChannelHandlerContext context, IByteBuffer input, List<object> output)
        {
            try
            {
                _logger.Info($"进入拆包处理类，input: rdx={input.ReaderIndex}, wdx-{input.WriterIndex}, size-{input.ReadableBytes}");
                var bytes = ParseUtil.Split(input);
                if(bytes==null) return;
                    output.Add(bytes);
            }
            finally
            {
                _logger.Info($"调用reset buffer前，input: rdx={input.ReaderIndex}, wdx-{input.WriterIndex}, size-{input.ReadableBytes}");
                
                // input.DiscardSomeReadBytes();
                // _logger.Info($"调用DiscardSomeReadBytes后，input: rdx={input.ReaderIndex}, wdx-{input.WriterIndex}, size-{input.ReadableBytes}");
                
                ResetBuffer(input);
                _logger.Info($"调用reset buffer后，input: rdx={input.ReaderIndex}, wdx-{input.WriterIndex}, size-{input.ReadableBytes}");
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