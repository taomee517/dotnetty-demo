using System;
using System.Text;
using DotNetty.Buffers;
using DotNetty.Handlers.Timeout;
using DotNetty.Transport.Channels;
using gk_common.utils;

namespace gk_client.handler
{
    public class GkDevice : ChannelHandlerAdapter
    {
        private static int index = 0;
        private const string RealMsg = "fe-00-00-00-01-00-02-3e-76-10-98-80-00-ff-00-1c-7f-fe-00-4a-26-38-a4-bd-00-03-00-0a-00-00-00-0c-00-00-00-00-00-00-00-02-00-00-00-1e-71-0f";
        private const string Regret = "Hello,World!";
        
        public override void ChannelActive(IChannelHandlerContext context)
        {
            var endPoint = context.Channel.RemoteAddress;
            Console.WriteLine("与server建立连接, remote:{0}", endPoint.ToString());
            // SayHello2Server(context,regret);
            SendRealMsg2Server(context, RealMsg);
        }

        public override void ChannelInactive(IChannelHandlerContext context)
        {
            var endPoint = context.Channel.RemoteAddress;
            Console.WriteLine("与server断开连接, remote:{0}", endPoint.ToString());
            context.CloseAsync();
        }

        public override void ChannelRead(IChannelHandlerContext context, object message)
        {
            var buffer = message as IByteBuffer;
            if(buffer==null) return;
            var msg = buffer.ToString(Encoding.UTF8);
            Console.WriteLine("收到server端消息, remote:{0}", msg);
            // index++;
            // if (index>10) return;
            // var resp = regret + index;
            // SayHello2Server(context, resp);
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
                // SayHello2Server(context, Regret);
            }
            else
            {
                Console.WriteLine("全超时事件！");   
            }

        }

        public override void ExceptionCaught(IChannelHandlerContext context, Exception exception)
        {
            Console.WriteLine("GkDevice发生异常,异常原因：{0}", exception.Message);
        }

        private void SayHello2Server(IChannelHandlerContext context,string msg)
        {
            Console.WriteLine("发送消息到Server, msg:{0}", msg);
            var bytes = System.Text.Encoding.UTF8.GetBytes(msg);
            var buffer = Unpooled.WrappedBuffer(bytes);
            context.WriteAndFlushAsync(buffer);
        }

        private void SendRealMsg2Server(IChannelHandlerContext context,string msg)
        {
            Console.WriteLine("发送消息到Server, msg:{0}", msg);
            msg = msg.Replace("-", "");
            var bytes = BytesUtil.Hex2Bytes(msg);
            var buffer = Unpooled.WrappedBuffer(bytes);
            context.WriteAndFlushAsync(buffer);
        }
    }
}