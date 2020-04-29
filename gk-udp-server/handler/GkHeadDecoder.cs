using System;
using System.Collections.Generic;
using DotNetty.Buffers;
using DotNetty.Codecs;
using DotNetty.Transport.Channels;
using gk_common.utils;
using NLog;

namespace gk_udp_server.handler
{
    public class GkHeadDecoder : MessageToMessageDecoder<IByteBuffer>
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        protected override void Decode(IChannelHandlerContext context, IByteBuffer message, List<object> output)
        {
            try
            {
                var baseMessage = GkParser.Decode(message);
                output.Add(baseMessage);
            }
            finally
            {
                // message.SafeRelease();
            }
            
        }
        
        public override void ExceptionCaught(IChannelHandlerContext context, Exception exception)
        {
            Logger.Error($"GkHeadDecoder 发生异常,异常原因：{exception.Message}");
        }
    }
}