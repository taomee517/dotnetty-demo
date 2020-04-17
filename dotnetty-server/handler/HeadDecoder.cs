using System;
using System.Collections.Generic;
using DotNetty.Codecs;
using DotNetty.Transport.Channels;
using dotnetty_server.beans;
using dotnetty_server.utils;
using Newtonsoft.Json;

namespace dotnetty_server.handler
{
    public class HeadDecoder: MessageToMessageDecoder<byte[]>
    {
        protected override void Decode(IChannelHandlerContext context, byte[] message, List<object> output)
        {
            BaseMessage baseMessage = ParseUtil.parse(message);
            var msgJson = JsonConvert.SerializeObject(baseMessage, Formatting.Indented);
            Console.WriteLine(msgJson);
        }
    }
}