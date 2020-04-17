using System;
using DotNetty.Buffers;

namespace dotnetty_feature.utils
{
    public class ParseUtil
    {
        public static byte[] Split(IByteBuffer buffer)
        {
            int readableSize = buffer.ReadableBytes;
            if(readableSize==0){
                return null;
            }
            byte[] starter = new byte[2];
            for(int i=0; i<readableSize-1; i++){
                starter[0] = buffer.GetByte(i);
                starter[1] = buffer.GetByte(i+1);
                if(Equals(starter, START_SIGN)){
                    buffer.SetReaderIndex(i);
                    break;
                }
            }
            //至少得有13个字节 帧头（2) + 长度(2) + 硬件类型(1) + MAC地址(6) + CRC校验(2)
            if(readableSize<13){
                return null;
            }
            byte[] srcLength = new byte[2];
            buffer.GetBytes(2, srcLength);
            int length = BytesUtil.Bytes2Int16(srcLength);

            //真正的包长度 = 帧头（2) + 长度(2) + 【长度值 硬件类型(1) + MAC地址(6) + 业务数据(n)】 + CRC校验(2)
            byte[] frame = null;
            int frameLength = length + 6; 
            if (readableSize >= frameLength)
            {
                frame = new byte[frameLength];
                buffer.ReadBytes(frame);
            }
            return frame;
        }

        private static readonly byte[] START_SIGN = new byte[]{0x3a & 0xff, 0xa3 & 0xff};
    }
}