// 创建人：taomee
// 创建时间：2020/06/02 14:17

using System;
using System.Collections.Generic;
using DotNetty.Buffers;
using DotNetty.Codecs;
using DotNetty.Transport.Channels;
using Lora_Common.Util;

namespace Lora4G_Server.Handler
{
    public class FrameSplitDecoder : ByteToMessageDecoder
    {

        protected override void Decode(IChannelHandlerContext context, IByteBuffer input, List<object> output)
        {
            var msg = new byte[input.ReadableBytes];
            input.ReadBytes(msg);
            var hex = BytesUtil.HexInsertSpace(BytesUtil.BytesToHex(msg));
            var srcMac = hex.Substring(50, 18).Replace(" ", "");
            Console.WriteLine($"Time: {DateTime.Now} Mac: {srcMac} => 收到服务器消息：{hex}");
            output.Add(Unpooled.WrappedBuffer(msg));
        }
    }
}