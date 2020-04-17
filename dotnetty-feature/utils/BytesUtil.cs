using System;
using System.Text;

namespace dotnetty_feature.utils
{
    public class BytesUtil
    {
        public static byte[] String2Bytes(string msg)
        {
            return System.Text.Encoding.Default.GetBytes(msg);
        }

        public static string Bytes2String(byte[] bytes)
        {
            return System.Text.Encoding.Default.GetString(bytes);
        }

        /*16进制字符串转为字节数组*/
        public static byte[] Hex2Bytes(string hex)
        {
            var result = new byte[hex.Length/2];
            var sb = new StringBuilder("0x");
            for (int i = 0; i < hex.Length; i++)
            {
                var c = hex[i];
                sb.Append(c);
                if (i%2==1)
                {
                    var singleHex = sb.ToString();
                    result[i / 2] = Convert.ToByte(singleHex,16);
                    sb = new StringBuilder("0x");
                }
            }
            return result;
        }


        public static string HexInsertSpace(string hex)
        {
            var sb = new StringBuilder();
            for (int i = 0; i < hex.Length; i++)
            {
                var c = hex[i];
                sb.Append(c);
                if (i%2==1)
                {
                    sb.Append(' ');
                }
            }

            return sb.ToString();
        }

        public static string ToShowString(byte[] bytes)
        {
            int lastIndex = bytes.Length-1;
            StringBuilder sb = new StringBuilder();
            sb.Append("[");
            for (int i = 0; i < lastIndex; i++) {
                sb.Append(bytes[i]);
                sb.Append(",");
            }
            sb.Append(bytes[lastIndex]);
            sb.Append("]");
            return sb.ToString();
        }

        public static byte[] Int16ToBytes(Int16 value)
        {
            return new byte[2] {(byte) (value >> 8 & 0xff), (byte) (value & 0xff)};
        }

        public static Int16 Bytes2Int16(byte[] bytes)
        {
            int checkSum = 0;
            for (int i = 0; i < bytes.Length; i++)
            {
                byte b = bytes[i];
                checkSum = b << (bytes.Length - 1 - i) | checkSum;
            }

            return (Int16)(checkSum & 0xffff);
        }
    }
}