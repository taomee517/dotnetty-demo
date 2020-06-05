// 创建人：taomee
// 创建时间：2020/06/02 14:40

using System;
using DotNetty.Buffers;
using DotNetty.Handlers.Timeout;
using DotNetty.Transport.Channels;
using Lora_Common.Constant;
using Lora_Common.SDK;
using Lora_Common.Util;

namespace Lora4G_Client.Handler
{
    public class Lora4GDevice : ChannelHandlerAdapter
    {
        private readonly string mac = "000106000147";
        private int index = 0;
        
        public override void ChannelActive(IChannelHandlerContext context)
        {
//            var msg = GetAndLogHeartBeat();
//            var msg = GetRealMsg();
//            var msg = GetCrackDeviceMsg();
            var msg = GetSettleDeviceMsg();
            context.WriteAndFlushAsync(Unpooled.WrappedBuffer(msg));
        }

        public override void ChannelInactive(IChannelHandlerContext context)
        {
            base.ChannelInactive(context);
        }

        public override void ChannelRead(IChannelHandlerContext context, object message)
        {
            base.ChannelRead(context, message);
        }

        public override void UserEventTriggered(IChannelHandlerContext context, object evt)
        {
            if (evt is IdleStateEvent state)
            {
                switch (state.State)
                {
                    case IdleState.WriterIdle:
//                        var hbMsg = GetAndLogHeartBeat();
//                        context.WriteAndFlushAsync(Unpooled.WrappedBuffer(hbMsg));

                        var msg = GetSettleDeviceMsg();
                        context.WriteAndFlushAsync(Unpooled.WrappedBuffer(msg));
                        break;
                    case IdleState.ReaderIdle:
                        Console.WriteLine("reader idle time out!");
                        break;
                    case IdleState.AllIdle:
                        Console.WriteLine("all idle time out!");
                        break;
                    default:
                        break;
                }
            } 
            
        }

        public override void Flush(IChannelHandlerContext context)
        {
            base.Flush(context);
        }


        private byte[] GetAndLogHeartBeat()
        {
            var hbMsg = MessageBuilder.BuildHeartBeat(mac);
            var hbHex = BytesUtil.BytesToHexWithSeparator(hbMsg, "-");
            Console.WriteLine($"Time: {DateTime.Now} Mac: {mac} => 发送心跳消息：{hbHex}");
            return hbMsg;
        }

        private byte[] GetRealMsg()
        {
            //线上数据
            var hex =
                "a5 2e 5d 00 82 5e d7 03 13 40 76 00 01 06 00 00 14 00 16 3e 12 5c 98 00 19 01 02 58 01 00 01 06 00 00 14 01 0b bf 5e d7 03 13 43 72 54 90 ee 96 76 99 7a 05";
            var mac = hex.Substring(32, 18).Replace(" ", "");
            Console.WriteLine($"Time: {DateTime.Now} Mac: {mac} => 发送消息：{hex}");
            hex = hex.Replace(" ", "");
            var msg = BytesUtil.Hex2Bytes(hex);
            return msg;
        }

        private byte[] GetCrackDeviceMsg()
        {
            var core = MessageBuilder.BuildCrackData(1005, 30f);
            var sensorMsg = MessageBuilder.BuildSensorMsg(mac, 1, SensorType.THLSD, core);
            var msg = MessageBuilder.BuildMessage(0, TransportType.GRPS, FunType.GatewayCacheDataBPublish, mac,
                sensorMsg);
            var hex = BytesUtil.BytesToHex(msg);
            Console.WriteLine($"Time: {DateTime.Now} Mac: {mac} => 发送消息：{hex}");
            return msg;
        }
        
        
        private byte[] GetSettleDeviceMsg()
        {
            var srcUnmarkHeight = 3000;
            var srcHeight = 10050.32f;
            var unmarkHeight = srcUnmarkHeight + index * 12;
            var height = srcHeight + index * 1.2f * (new Random().Next(10));
            var core = MessageBuilder.BuildSettleData(unmarkHeight, -3, height);
            var sensorMsg = MessageBuilder.BuildSensorMsg(mac, 1, SensorType.THSTC, core);
            var msg = MessageBuilder.BuildMessage(0, TransportType.GRPS, FunType.GatewayCacheDataBPublish, mac,
                sensorMsg);
            var hex = BytesUtil.HexInsertSpace(BytesUtil.BytesToHex(msg));
            Console.WriteLine($"Time: {DateTime.Now} Index : {index} Mac: {mac} => 发送消息：{hex}");
            index++;
            return msg;
        }
    }
}