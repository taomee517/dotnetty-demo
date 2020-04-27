using System;
using System.Collections.Generic;
using DotNetty.Codecs;
using DotNetty.Transport.Channels;
using gk_common.beans;
using gk_common.utils;
using NLog;

namespace gk_server.handler
{
    public class GkBodyDecoder : MessageToMessageDecoder<BaseMessage>
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        
        protected override void Decode(IChannelHandlerContext context, BaseMessage message, List<object> output)
        {
            // var content = message.Content;
            var primaryMsg = GkParser.BodyParse(message);
            if (primaryMsg == null) return;
            output.Add(primaryMsg);
        }
        
        public override void ExceptionCaught(IChannelHandlerContext context, Exception exception)
        {
            Logger.Error($"GkBodyDecoder 发生异常,异常原因：{exception.Message}");
        }
    }
}