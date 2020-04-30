using System;
using DotNetty.Handlers.Timeout;
using DotNetty.Transport.Channels;
using gk_common.beans;
using gk_common.utils;
using NewLife.Serialization;
using Newtonsoft.Json;
using NLog;

namespace gk_server.handler
{
    public class GkCoreHandler : ChannelHandlerAdapter
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        
        public override void ChannelActive(IChannelHandlerContext context)
        {
            var endPoint = context.Channel.RemoteAddress;
            Logger.Info($"设备与平台建立连接, remote:{endPoint.ToString()}");
            // SayHello2Server(context,regret);
        }

        public override void ChannelInactive(IChannelHandlerContext context)
        {
            var endPoint = context.Channel.RemoteAddress;
            Logger.Warn($"设备与平台断开连接, remote:{endPoint.ToString()}");
            context.CloseAsync();
        }

        public override void ChannelRead(IChannelHandlerContext context, object message)
        {
            if (message is Message msg)
            {
                // var json = JsonConvert.SerializeObject(msg,Formatting.None);
                GkParser.ParseCore(ref msg);
                Logger.Info($"收到设备消息, msg:{msg.Bodies[0].IdType.ToString()}");
                MsgBuilder.BuildResp(context, msg);
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