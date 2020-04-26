using DotNetty.Buffers;
using DotNetty.Common.Utilities;
using gk_common.beans;
using gk_common.constants;

namespace gk_common.utils
{
    public class GkParser
    {
        /// <summary>
        /// 基康设备的拆包方法
        /// </summary>
        /// <param name="input">收到的字节流</param>
        /// <returns>截取的单个完整消息</returns>
        public static IByteBuffer Split(IByteBuffer input)
        {
            var inputLen = input.ReadableBytes;
            //先判断是否能找到帧头,并设置读下标
            var startIndex = FindStarterIndex(input);
            if (startIndex == -1) return null;
            input.SetReaderIndex(startIndex);
            
            //判断帧头后的可读长度是否小于最小消息长度
            var maxMsgLen = inputLen - startIndex + 1;
            if (maxMsgLen < GkDefault.Min_Length) return null;
            
            //获取信息单元长度，起始下标13，占两个字节
            var srcContentLengthBuffer = Unpooled.Buffer(2);
            input.GetBytes(13, srcContentLengthBuffer);
            var contentLength = srcContentLengthBuffer.ReadInt();
            
            //判断帧头后的可读长度是否小于本条消息实际长度
            var msgLength = GkDefault.Min_Length + contentLength;
            if (maxMsgLen < msgLength) return null;
            
            //读取本条消息的完整内容
            // var data = new byte[msgLength];
            var buffer = PooledByteBufferAllocator.Default.Buffer(msgLength);
            input.ReadBytes(buffer);
            return buffer;
        }

        
        public static BaseMessage Decode(IByteBuffer msg)
        {
            var head = msg.ReadByte();
            
            var addr = new byte[4];
            msg.ReadBytes(addr);
            var dst = BytesUtil.BytesToHex(addr);

            msg.ReadBytes(addr);
            var src = BytesUtil.BytesToHex(addr);

            var version = (int) msg.ReadByte();

            var srcControl = new byte[2];
            msg.ReadBytes(srcControl);
            var ctrl = BytesUtil.BytesToHex(srcControl);
            
            var send = (int) msg.ReadByte();
            
            var receive = (int) msg.ReadByte();

            var contentLength = msg.ReadUnsignedShort();

            var content = PooledByteBufferAllocator.Default.Buffer(contentLength);
            msg.ReadBytes(content);
            
            return new BaseMessage
            {
                Head = head,
                Dst = dst,
                Src = src,
                Control = ctrl,
                Send = send,
                Receive = receive,
                Length = contentLength,
                Content = content
            };
        }
        
        
        private static int FindStarterIndex(IByteBuffer input)
        {
            //等同于for循环去查找input中starter的下标，没有就返回-1
            return input.ForEachByte(new IndexOfProcessor(GkDefault.Starter));
        }
    }
}