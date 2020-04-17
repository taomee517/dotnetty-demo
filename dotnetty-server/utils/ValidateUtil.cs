using DotNetty.Buffers;
using DotNetty.Transport.Bootstrapping;

namespace dotnetty_server.utils
{
    public class ValidateUtil
    {
        public static bool validate(byte[] content)
        {
            var buffer = Unpooled.WrappedBuffer(content);
            var waitValidateData = new byte[buffer.ReadableBytes -2];
            buffer.ReadBytes(waitValidateData);
            
            var crc = new byte[2];
            buffer.ReadBytes(crc);
            
            var calCrc = CRCUtil.CalCrc(waitValidateData);
            return BytesUtil.ByteArrayEquals(calCrc, crc);
        }

       
    }
}