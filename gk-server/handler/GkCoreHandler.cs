using System;
using DotNetty.Handlers.Timeout;
using DotNetty.Transport.Channels;
using gk_common.beans;
using NewLife.Serialization;

namespace gk_server.handler
{
    public class GkCoreHandler : ChannelHandlerAdapter
    {
        public override void ChannelActive(IChannelHandlerContext context)
        {
            var endPoint = context.Channel.RemoteAddress;
            Console.WriteLine("设备与平台建立连接, remote:{0}", endPoint.ToString());
            // SayHello2Server(context,regret);
        }

        public override void ChannelInactive(IChannelHandlerContext context)
        {
            var endPoint = context.Channel.RemoteAddress;
            Console.WriteLine("设备与平台断开连接, remote:{0}", endPoint.ToString());
            context.CloseAsync();
        }

        public override void ChannelRead(IChannelHandlerContext context, object message)
        {
            if (message is BaseMessage msg)
            {
                Console.WriteLine("收到设备消息, remote:{0}", msg.ToJson());
            }
        }

        public override void UserEventTriggered(IChannelHandlerContext context, object evt)
        {
            var idleEvt = evt as IdleStateEvent;
            if(idleEvt == null) return;
            if (Equals(IdleState.ReaderIdle, idleEvt.State))
            {
                Console.WriteLine("读超时事件！");  
            }
            else if(Equals(IdleState.WriterIdle, idleEvt.State))
            {
                Console.WriteLine("写超时事件！");
            }
            else
            {
                Console.WriteLine("全超时事件！");   
            }

        }

        public override void ExceptionCaught(IChannelHandlerContext context, Exception exception)
        {
            Console.WriteLine("GkCoreHandler 发生异常,异常原因：{0}", exception.Message);
        }
    }
}