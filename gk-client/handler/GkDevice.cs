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
        //private const string RealMsg = "fe-00-00-00-01-00-02-3e-76-10-98-80-00-ff-00-1c-7f-fe-00-4a-26-38-a4-bd-00-03-00-0a-00-00-00-0c-00-00-00-00-00-00-00-02-00-00-00-1e-71-0f";
        private const string RealMsg =
            "FE0000000100023E7E10988000FF03DC7FFE1D0024822DE00001000A0005001C00000000FE967699FE967699FE967699FE967699FE967699FE9676997FFE004224822DE00001000A0005001000000000407B399C41B54E25461600007FFE1D00248282400001000A0005001C00000000FE967699FE967699FE967699FE967699FE967699FE9676997FFE0042248282400001000A0005001000000000407B18A641E62300461600007FFE1D002482D6A00001000A0005001C00000000FE967699FE967699FE967699FE967699FE967699FE9676997FFE00422482D6A00001000A0005001000000000407AEBF44223DBA2461600007FFE1D0024832B000001000A0005001C00000000FE967699FE967699FE967699FE967699FE967699FE9676997FFE004224832B000001000A0005001000000000407B1FF341A35E4E461600007FFE1D0024837F600001000A0005001C00000000FE967699FE967699FE967699FE967699FE967699FE9676997FFE004224837F600001000A0005001000000000407B1A17418EDCF6461600007FFE1D002483D3C00001000A0005001C00000000FE967699FE967699FE967699FE967699FE967699FE9676997FFE00422483D3C00001000A0005001000000000407B0EA141A35384461600007FFE1D00248428200001000A0005001C00000000FE967699FE967699FE967699FE967699FE967699FE9676997FFE0042248428200001000A0005001000000000407B029241AEF7E8461600007FFE1D0024847C800001000A0005001C00000000FE967699FE967699FE967699FE967699FE967699FE9676997FFE004224847C800001000A0005001000000000407AFCC5419A40AF461600007FFE1D002484D0E00001000A0005001C00000000FE967699FE967699FE967699FE967699FE967699FE9676997FFE00422484D0E00001000A0005001000000000407AF188419402B9461600007FFE1D00248525400001000A0005001C00000000FE967699FE967699FE967699FE967699FE967699FE9676997FFE0042248525400001000A0005001000000000407ADE0541A8BCA5461600007FFE1D00248579A00001000A0005001C00000000FE967699FE967699FE967699FE967699FE967699FE9676997FFE0042248579A00001000A0005001000000000407AD19F41AF9F04461600007FFE1D002485CE000001000A0005001C00000000FE967699FE967699FE967699FE967699FE967699FE9676997FFE00422485CE000001000A0005001000000000407ACFE5418E691B461600007FFE1D00248622600001000A0005001C00000000FE967699FE967699FE967699FE967699FE967699FE9676997FFE0042248622600001000A0005001000000000407AC65A418758F246160000B187";

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