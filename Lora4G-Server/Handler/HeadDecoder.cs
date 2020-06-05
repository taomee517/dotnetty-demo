// 创建人：taomee
// 创建时间：2020/06/03 10:18

using System;
using System.Collections.Generic;
using DotNetty.Buffers;
using DotNetty.Codecs;
using DotNetty.Transport.Channels;
using Lora_Common.SDK;
using Lora_Common.Util;

namespace Lora4G_Server.Handler
{
    public class HeadDecoder : ByteToMessageDecoder
    {
        protected override void Decode(IChannelHandlerContext context, IByteBuffer input, List<object> output)
        {
            var header = LoraParser.HeaderParse(input);
            Console.WriteLine($"消息头解析结果：fun = {header.FunType}, mac = {header.SrcMac}, content = {BytesUtil.BytesToHexWithSeparator(header.Content,"-")}");
            output.Add(header);
        }
    }
}