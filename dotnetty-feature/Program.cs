using System;
using System.Net;
using System.Text;
using System.Threading;
using DotNetty.Buffers;
using dotnetty_feature.utils;
using NewLife.Caching;

namespace dotnetty_feature
{
    class Program
    {
        static void Main(string[] args)
        {
            // string msg =
            //     "3aa3002a0920200226000201000b2d00d003001a5e959187000100003a980e823600000e1014003c000100000000ca8d";
            // string hexMsg = BytesUtil.HexInsertSpace(msg);
            // Console.WriteLine("hex with blank : {0}", hexMsg);
            //
            // byte[] bytes = BytesUtil.Hex2Bytes(msg);
            // Console.WriteLine("hex bytes :{0}", BytesUtil.ToShowString(bytes));
            //
            // var buffer = UnpooledByteBufferAllocator.Default.Buffer();
            // buffer.WriteBytes(bytes);
            //
            // // var bs = buffer.Slice(buffer.ArrayOffset, buffer.ReadableBytes).Array;
            // var bs = ParseUtil.Split(buffer);
            //
            // Console.WriteLine(BytesUtil.ToShowString(bs));
            //
            // var aliyunBroadcast = IPAddress.Parse("100.64.0.0").GetBroadcast(10);
            // Console.WriteLine(aliyunBroadcast);

            var ipAddress = IPAddress.Parse("218.204.252.241");
            var addr = ipAddress.ToString();
            Console.WriteLine(addr);
            var cache = MemoryCache.Default;
            var now = DateTime.Now;
            Console.WriteLine("当前时间{0}", now.ToString());
            var expire = new TimeSpan(0, 0, 10);
            cache.Set(addr, 1, expire);
            
            var ttl0 = cache.GetExpire(addr);
            Console.WriteLine("马上查过期时间{0},值:{1}", ttl0.TotalSeconds, cache.Get<int>(addr));
            
            Thread.Sleep(1000);
            var expire1 = cache.ContainsKey(addr);
            cache.Set(addr, cache.Get<int>(addr) + 1, expire);
            Console.WriteLine("1秒后查，还有数据吗？ {0},值:{1}", expire1, cache.Get<int>(addr));
            
            Thread.Sleep(3000);
            var expire2 = cache.ContainsKey(addr);
            Console.WriteLine("4秒后查，还有数据吗？ {0},值:{1}", expire2, cache.Get<int>(addr));
            
            Thread.Sleep(5000);
            var expire3 = cache.ContainsKey(addr);
            Console.WriteLine("10秒后查，还有数据吗？ {0},值:{1}", expire3, cache.Get<int>(addr));

        }

    }
}