using System;
using System.Collections.Generic;
using DotNetty.Buffers;
using DotNetty.Codecs;
using DotNetty.Transport.Channels;
using DotNetty.Transport.Channels.Sockets;
using gk_common.beans;
using gk_common.utils;
using NLog;

namespace gk_udp_server.handler
{
    public class GkHeadDecoder : MessageToMessageDecoder<DatagramPacket>
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        protected override void Decode(IChannelHandlerContext context, DatagramPacket message, List<object> output)
        {
            try
            {
                if (message is DatagramPacket packet)
                {
                    var buffer = packet.Content;
                    var baseMessage = GkParser.Decode(buffer);
//                    var baseUdpMsg = (BaseUdpMessage)baseMessage;
                    var baseUdpMsg = new BaseUdpMessage(baseMessage,packet.Recipient);
                    output.Add(baseUdpMsg);
                }
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