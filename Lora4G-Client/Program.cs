using System;
using Lora_Common.Constant;
using Lora_Common.SDK;
using Lora_Common.Util;
using Lora4G_Client.Client;

namespace Lora4G_Client
{
    class Program
    {
        static void Main(string[] args)
        {
            var host = "127.0.0.1";
            var port = 18568;
            Console.WriteLine("项目启动...");
            var client = new Lora4GClient(host,port);
            client.Start().Wait();

//            var mac = "000106000020";
//            var hbMsg = MessageBuilder.BuildHeartBeat(mac);
//            var hex = BytesUtil.BytesToHex(hbMsg);
//            Console.WriteLine(BytesUtil.HexInsertSpace(hex));

//            var unmarkHeight = 3100;
//            var height = 10050.32f;
//            var temp = -3;
//            var mac = "000106000147";
//            var core = MessageBuilder.BuildSettleData(unmarkHeight, temp, height);
//            var sensorMsg = MessageBuilder.BuildSensorMsg(mac, 1, SensorType.THSTC, core);
//            var msg = MessageBuilder.BuildMessage(0, TransportType.GRPS, FunType.GatewayCacheDataBPublish, mac,
//                sensorMsg);
//            var hex = BytesUtil.HexInsertSpace(BytesUtil.BytesToHex(msg));
//            Console.WriteLine($"Time: {DateTime.Now} Mac: {mac} => 发送消息：{hex}");
        }
    }
}