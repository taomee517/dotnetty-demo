﻿using System;
using System.Collections.Generic;
using DotNetty.Codecs;
using DotNetty.Transport.Channels;
using gk_common.beans;
using gk_common.utils;
using NLog;

namespace gk_udp_server.handler
{
    public class GkBodyDecoder : MessageToMessageDecoder<BaseUdpMessage>
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        
        protected override void Decode(IChannelHandlerContext context, BaseUdpMessage message, List<object> output)
        {
            // var content = message.Content;
            var primaryMsg = GkParser.BodyParse(message);
            if (primaryMsg == null) return;
            var primaryUdpMsg = new UdpMessage(primaryMsg,message.Sender);
            output.Add(primaryUdpMsg);
        }
        
        public override void ExceptionCaught(IChannelHandlerContext context, Exception exception)
        {
            Logger.Error($"GkBodyDecoder 发生异常,异常原因：{exception.Message}");
        }
    }
}