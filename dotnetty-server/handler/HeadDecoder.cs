using System;
using System.Collections.Generic;
using DotNetty.Codecs;
using DotNetty.Transport.Channels;
using dotnetty_server.beans;
using dotnetty_server.utils;
using Newtonsoft.Json;
using NLog;

namespace dotnetty_server.handler
{
    public class HeadDecoder: MessageToMessageDecoder<byte[]>
    {
        private static Logger _logger = LogManager.GetCurrentClassLogger();
        
        protected override void Decode(IChannelHandlerContext context, byte[] message, List<object> output)
        {
            BaseMessage baseMessage = ParseUtil.parse(message);
            var msgJson = JsonConvert.SerializeObject(baseMessage, Formatting.Indented);
            // _logger.Info($"解析后的基础类，baseMessage={msgJson}");
            output.Add(baseMessage);
        }
    }
}