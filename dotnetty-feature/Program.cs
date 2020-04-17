using System;
using System.Net;
using System.Text;
using DotNetty.Buffers;
using dotnetty_feature.utils;

namespace dotnetty_feature
{
    class Program
    {
        static void Main(string[] args)
        {
            string msg =
                "3aa3002a0920200226000201000b2d00d003001a5e959187000100003a980e823600000e1014003c000100000000ca8d";
            string hexMsg = BytesUtil.HexInsertSpace(msg);
            Console.WriteLine("hex with blank : {0}", hexMsg);

            byte[] bytes = BytesUtil.Hex2Bytes(msg);
            Console.WriteLine("hex bytes :{0}", BytesUtil.ToShowString(bytes));

            var buffer = UnpooledByteBufferAllocator.Default.Buffer();
            buffer.WriteBytes(bytes);
            
            // var bs = buffer.Slice(buffer.ArrayOffset, buffer.ReadableBytes).Array;
            var bs = ParseUtil.Split(buffer);
            
            Console.WriteLine(BytesUtil.ToShowString(bs));
            
            var aliyunBroadcast = IPAddress.Parse("100.64.0.0").GetBroadcast(10);
            Console.WriteLine(aliyunBroadcast);
            
        }

    }
}