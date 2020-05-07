using System;
using System.Net;
using DotNetty.Buffers;
using DotNetty.Handlers.Timeout;
using DotNetty.Transport.Channels;
using DotNetty.Transport.Channels.Sockets;
using gk_common.beans;
using gk_common.constants;
using gk_common.utils;
using NLog;

namespace gk_udp_server.handler
{
    public class GkCoreHandler : ChannelHandlerAdapter
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        
        public override void ChannelActive(IChannelHandlerContext context)
        {
            Logger.Info($"设备与平台建立连接");
        }

        public override void ChannelInactive(IChannelHandlerContext context)
        {
            Logger.Warn($"设备与平台断开连接");
            context.CloseAsync();
        }

        public override void ChannelRead(IChannelHandlerContext context, object message)
        {
            var sender = ((UdpMessage) message).Sender;
            if (message is Message msg)
            {
                // var json = JsonConvert.SerializeObject(msg,Formatting.None);
                GkParser.ParseCore(ref msg);
                Logger.Info($"收到设备消息, msg:{msg.Bodies[0].IdType.ToString()}");
//                MsgBuilder.BuildResp(context, msg);
                var header = msg.Header;
                var body = msg.Bodies[0];
                if (IdType.HeartBeat.Equals(body.IdType) && HeartBeatType.Online.Equals(body.HeartBeatInfo.HeartBeatType))
                {
                    //下发查询设备类型指令
                    var content = MsgBuilder.BuildContent(IdType.ReadDeviceInfo, MsgType.Configure, OpsType.ReadCommand,
                        0, 1, null);
                    var cmd = MsgBuilder.BuildMessage(header.Src,header.Dst,header.Ver,false,1,0,content);
                    var buffer = Unpooled.WrappedBuffer(cmd);
                    var packet = new DatagramPacket(buffer,sender);
                    context.WriteAndFlushAsync(packet);
                }
            }
        }

        public override void UserEventTriggered(IChannelHandlerContext context, object evt)
        {
            var idleEvt = evt as IdleStateEvent;
            if(idleEvt == null) return;
            switch (idleEvt.State)
            {
                case IdleState.ReaderIdle:
                    Logger.Info("读超时事件!");
                    break;
                case IdleState.WriterIdle:
                    Logger.Info("写超时事件!");
                    break;
                case IdleState.AllIdle:
                    Logger.Info("全超时事件!");
                    break;
                default:
                    break;
            }

        }

        public override void ExceptionCaught(IChannelHandlerContext context, Exception exception)
        {
           Logger.Error($"GkCoreHandler 发生异常,异常原因：{exception.Message}");
        }
    }
}