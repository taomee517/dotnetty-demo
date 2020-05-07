using System;
using System.Net;
using System.Text;
using DotNetty.Buffers;
using DotNetty.Handlers.Timeout;
using DotNetty.Transport.Channels;
using DotNetty.Transport.Channels.Sockets;
using NLog;
using gk_common.utils;

namespace gk_udp_client
{
    public class GkDevice : SimpleChannelInboundHandler<DatagramPacket>
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        private const string RemoteHost = "127.0.0.1";
        private const int RemotePort = 19557;
        private static int Index = 0;
        
        // private const string HeartBeatMsg = "fe-00-00-00-01-00-02-3e-76-10-98-80-00-ff-00-1c-7f-fe-00-4a-26-38-a4-bd-00-03-00-0a-00-00-00-0c-00-00-00-00-00-00-00-02-00-00-00-1e-71-0f";
        // private const string RealMsg = "FE00000001000240D910988000FF03C07FFE1D002509DC400001000A0005000C00000000453964EE45774A577FFE00422509DC400001000A00050010000000004075AC9541CB907A461600007FFE1D00250A14800001000A0005000C000000004539634A457747347FFE0042250A14800001000A00050010000000004075ADCE41CD14CE461600007FFE1D00250A4CC00001000A0005000C00000000453960B9457748967FFE0042250A4CC00001000A00050010000000004075D0114210EB17461600007FFE1D00250A85000001000A0005000C000000004539651C4577459E7FFE0042250A85000001000A00050010000000004075D6D0421E438E461600007FFE1D00250ABD400001000A0005000C0000000045396231457749A47FFE0042250ABD400001000A00050010000000004075D36C41F7907D461600007FFE1D00250AF5800001000A0005000C0000000045395EAA457748287FFE0042250AF5800001000A00050010000000004075D12241DD08AE461600007FFE1D00250B2DC00001000A0005000C00000000453960A9457749787FFE0042250B2DC00001000A0005001000000000407585F741D43E74461600007FFE1D00250B66000001000A0005000C0000000045395E73457749337FFE0042250B66000001000A000500100000000040758F0A41D86ED4461600007FFE1D00250B9E400001000A0005000C0000000045395CCC457743AF7FFE0042250B9E400001000A00050010000000004075A35742031728461600007FFE1D00250BD6800001000A0005000C000000004539613245773D7E7FFE0042250BD6800001000A00050010000000004075AD4F421C0DCC461600007FFE1D00250C0EC00001000A0005000C0000000045396133457739A77FFE0042250C0EC00001000A00050010000000004075A5A941E7F6A6461600007FFE1D00250C47000001000A0005000C0000000045395F2D45773C8E7FFE0042250C47000001000A00050010000000004075AA6441D88882461600007FFE1D00250C7F400001000A0005000C000000004539619045774F697FFE0042250C7F400001000A000500100000000040755C1641D5826D461600007FFE1D00250CB7800001000A0005000C000000004539609E4577463F7FFE0042250CB7800001000A0005001000000000407567CA41D90684461600007FFE1D00250CEFC00001000A0005000C00000000453960D045773E317FFE0042250CEFC00001000A000500100000000040758D7142376211461600007FFE1D00250D28000001000A0005000C000000004539664A457732677FFE0042250D28000001000A0005001000000000407584324232FA614616000063AA";
        private const string RealMsg =
            "fe-00-00-00-01-00-02-3e-6e-10-98-80-00-ff-00-4c-7f-fe-1d-00-26-44-ba-80-00-01-00-0a-00-05-00-1c-00-00-00-00-44-0e-4b-62-45-51-5f-6c-44-22-ec-9c-45-51-ce-d4-fe-96-76-99-45-4e-2d-ff-7f-fe-00-42-26-44-ba-80-00-01-00-0a-00-05-00-10-00-00-00-00-40-85-51-e2-41-bf-c2-a2-46-16-00-00-3a-76";

//        private const string  RealMsg = "FE0000000100023E6E10024822DE00001000A0005001C00000000FE967699FE967699FE967699FE967699FE967699FE9676997FFE004224822DE00001000A0005001000000000407B399C41B54E25461600007FFE1D00248282400001000A0005001C00000000FE967699FE967699FE967699FE967699FE967699FE9676997FFE0042248282400001000A0005001000000000407B18A641E62300461600007FFE1D002482D6A00001000A0005001C00000000FE967699FE967699FE967699FE967699FE967699FE9676997FFE00422482D6A00001000A0005001000000000407AEBF44223DBA2461600007FFE1D0024832B000001000A0005001C00000000FE967699FE967699FE967699FE967699FE967699FE9676997FFE004224832B000001000A0005001000000000407B1FF341A35E4E461600007FFE1D0024837F600001000A0005001C00000000FE967699FE967699FE967699FE967699FE967699FE9676997FFE004224837F600001000A0005001000000000407B1A17418EDCF6461600007FFE1D002483D3C00001000A0005001C00000000FE967699FE967699FE967699FE967699FE967699FE9676997FFE00422483D3C00001000A0005001000000000407B0EA141A35384461600007FFE1D00248428200001000A0005001C00000000FE967699FE967699FE967699FE967699FE967699FE9676997FFE0042248428200001000A0005001000000000407B029241AEF7E8461600007FFE1D0024847C800001000A0005001C00000000FE967699FE967699FE967699FE967699FE967699FE9676997FFE004224847C800001000A0005001000000000407AFCC5419A40AF461600007FFE1D002484D0E00001000A0005001C00000000FE967699FE967699FE967699FE967699FE967699FE9676997FFE00422484D0E00001000A0005001000000000407AF188419402B9461600007FFE1D00248525400001000A0005001C00000000FE967699FE967699FE967699FE967699FE967699FE9676997FFE0042248525400001000A0005001000000000407ADE0541A8BCA5461600007FFE1D00248579A00001000A0005001C00000000FE967699FE967699FE967699FE967699FE967699FE9676997FFE0042248579A00001000A0005001000000000407AD19F41AF9F04461600007FFE1D002485CE000001000A0005001C00000000FE967699FE967699FE967699FE967699FE967699FE9676997FFE00422485CE000001000A0005001000000000407ACFE5418E691B461600007FFE1D00248622600001000A0005001C00000000FE967699FE967699FE967699FE967699FE967699FE9676997FFE0042248622600001000A0005001000000000407AC65A418758F246160000B187";

//        private const string LogoutMsg =
//            "fe-00-00-00-01-00-02-3e-7b-10-98-80-00-ff-00-1c-7f-fe-00-4a-26-45-3c-dd-00-07-00-0a-00-00-00-0c-00-00-00-00-00-00-00-01-00-00-00-1e-87-f1";
//        private const string LoginMsg =
//            "fe-00-00-00-01-00-02-3e-7b-10-98-80-00-ff-00-1c-7f-fe-00-4a-26-45-3c-dd-00-07-00-0a-00-00-00-0c-00-00-00-00-00-00-00-00-00-00-00-1e-87-f0";

        private const string DeviceInfoMsg = "";
        private const string Regret = "Hello,World!";

        protected override void ChannelRead0(IChannelHandlerContext ctx, DatagramPacket msg)
        {
            Logger.Info("GkDevice收到server端消息");
            var buffer = msg.Content;
            if(buffer==null) return;
            var serverMsg = Unpooled.Buffer(buffer.ReadableBytes);
            buffer.ReadBytes(serverMsg);
            var hexMsg = BytesUtil.BytesToHex(serverMsg.Array);
            Logger.Info($"收到server端消息, data:{hexMsg}");
            if (hexMsg.Contains("7ffe0048"))
            {
                var deviceInfo = DeviceInfoMsg.Replace("-", "");
                var bytes = BytesUtil.Hex2Bytes(deviceInfo);
                var deviceBuffer = Unpooled.WrappedBuffer(bytes);
                ctx.WriteAndFlushAsync(new DatagramPacket(deviceBuffer, msg.Sender));
            }
        }

        public override void ChannelActive(IChannelHandlerContext context)
        {
            Logger.Info("绑定端口成功！");

//            var endPoint = context.Channel.RemoteAddress;
//            Logger.Info($"与server建立连接, remote:{endPoint.ToString()}, local:{context.Channel.LocalAddress.ToString()}");

//            SayHello2Server(context,Regret);
            SendRealMsg2Server(context, RealMsg);
        }

        public override void ChannelInactive(IChannelHandlerContext context)
        {
            var endPoint = context.Channel.RemoteAddress;
            Logger.Warn($"与server断开连接, remote:{endPoint.ToString()}");
            context.CloseAsync();
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
            var bytes = System.Text.Encoding.UTF8.GetBytes(msg);
            var buffer = Unpooled.WrappedBuffer(bytes);
//            DatagramPacket packet = null;
//            if (Index==0)
//            {
//                packet = new DatagramPacket(buffer, new IPEndPoint(IPAddress.Parse((string) RemoteHost), RemotePort));
//            }
//            else
//            {
//                packet = new DatagramPacket(buffer,context.Channel.RemoteAddress);
//            }
//            Logger.Info($"发送消息到Server, msg:{msg}");
//            context.WriteAndFlushAsync(packet);
//            Index++;

            var packet = new DatagramPacket(buffer,context.Channel.RemoteAddress);
            context.WriteAndFlushAsync(packet);
        }

        private void SendRealMsg2Server(IChannelHandlerContext context,string msg)
        {
            Logger.Info($"发送消息到Server, msg:{msg}");
            msg = msg.Replace("-", "");
            var bytes = BytesUtil.Hex2Bytes(msg);
            var buffer = Unpooled.WrappedBuffer(bytes);
            
            DatagramPacket packet = null;
            if (Index==0)
            {
                packet = new DatagramPacket(buffer, new IPEndPoint(IPAddress.Parse((string) RemoteHost), RemotePort));
            }
            else
            {
                packet = new DatagramPacket(buffer,context.Channel.RemoteAddress);
            }
            context.WriteAndFlushAsync(packet);
            Index++;

//            var packet = new DatagramPacket(buffer,context.Channel.RemoteAddress);
//            context.WriteAndFlushAsync(packet);
        }
        
        
        
    }
}