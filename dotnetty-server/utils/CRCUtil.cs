namespace dotnetty_server.utils
{
    public class CRCUtil
    {
        public static byte[] CalCrc(byte[] data)
        {
            var crc = new byte[2];
            var checkSum = 0x0000ffff;
            const int polynomial = 0x0000a001;

            int i, j;
            for (i = 0; i < data.Length; i++) {
                checkSum ^= ((int) data[i] & 0x000000ff);
                for (j = 0; j < 8; j++) {
                    if ((checkSum & 0x00000001) != 0) {
                        checkSum >>= 1;
                        checkSum ^= polynomial;
                    } else {
                        checkSum >>= 1;
                    }
                }
            }
            crc[1] = (byte) (checkSum >> 8 & 0xff);
            crc[0] = (byte) (checkSum & 0xff);
            return crc;
        }
    }
}