using System;
using System.Collections.Generic;
using DotNetty.Buffers;
using DotNetty.Codecs;
using DotNetty.Common.Utilities;
using DotNetty.Transport.Channels;
using gk_common.beans;
using gk_common.utils;

namespace gk_server.handler
{
    public class GkHeadDecoder : MessageToMessageDecoder<IByteBuffer>
    {
        protected override void Decode(IChannelHandlerContext context, IByteBuffer message, List<object> output)
        {
            try
            {
                var baseMessage = GkParser.Decode(message);
                output.Add(baseMessage);
            }
            finally
            {
                message.SafeRelease();
            }
            
        }
        
        public override void ExceptionCaught(IChannelHandlerContext context, Exception exception)
        {
            Console.WriteLine("GkHeadDecoder 发生异常,异常原因：{0}", exception.Message);
        }
    }
}