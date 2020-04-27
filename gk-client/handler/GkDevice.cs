using System;
using System.Text;
using DotNetty.Buffers;
using DotNetty.Handlers.Timeout;
using DotNetty.Transport.Channels;
using gk_common.utils;
using NLog;

namespace gk_client.handler
{
    public class GkDevice : ChannelHandlerAdapter
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        
        private static int Index = 0;
        private const string RealMsg = "fe-00-00-00-01-00-02-3e-76-10-98-80-00-ff-00-1c-7f-fe-00-4a-26-38-a4-bd-00-03-00-0a-00-00-00-0c-00-00-00-00-00-00-00-02-00-00-00-1e-71-0f";
        private const string Regret = "Hello,World!";
        
        public override void ChannelActive(IChannelHandlerContext context)
        {
            var endPoint = context.Channel.RemoteAddress;
            Logger.Info($"与server建立连接, remote:{endPoint.ToString()}");
            // SayHello2Server(context,regret);
            SendRealMsg2Server(context, RealMsg);
        }

        public override void ChannelInactive(IChannelHandlerContext context)
        {
            var endPoint = context.Channel.RemoteAddress;
            Logger.Warn($"与server断开连接, remote:{endPoint.ToString()}");
            context.CloseAsync();
        }

        public override void ChannelRead(IChannelHandlerContext context, object message)
        {
            var buffer = message as IByteBuffer;
            if(buffer==null) return;
            var msg = buffer.ToString(Encoding.UTF8);
            Logger.Info($"收到server端消息, remote:{msg}");
            // Index++;
            // if (index>10) return;
            // var resp = regret + Index;
            // SayHello2Server(context, resp);
        }

        public override void UserEventTriggered(IChannelHandlerContext context, object evt)
        {
            var idleEvt = evt as IdleStateEvent;
            if(idleEvt == null) return;
            switch (idleEvt.State)
            {
                case IdleState.ReaderIdle:
                    Logger.Info($"读超时事件!");
                    break;
                case IdleState.WriterIdle:
                    Logger.Info($"写超时事件!");
                    break;
                case IdleState.AllIdle:
                    Logger.Info($"全超时事件!");
                    break;
                default:
                    break;
            }
        }

        public override void ExceptionCaught(IChannelHandlerContext context, Exception exception)
        {
            Logger.Info($"GkDevice发生异常,异常原因：{exception.Message}");
        }

        private void SayHello2Server(IChannelHandlerContext context,string msg)
        {
            Logger.Info($"发送消息到Server, msg:{msg}");
            var bytes = System.Text.Encoding.UTF8.GetBytes(msg);
            var buffer = Unpooled.WrappedBuffer(bytes);
            context.WriteAndFlushAsync(buffer);
        }

        private void SendRealMsg2Server(IChannelHandlerContext context,string msg)
        {
            Logger.Info($"发送消息到Server, msg:{msg}");
            msg = msg.Replace("-", "");
            var bytes = BytesUtil.Hex2Bytes(msg);
            var buffer = Unpooled.WrappedBuffer(bytes);
            context.WriteAndFlushAsync(buffer);
        }
    }
}