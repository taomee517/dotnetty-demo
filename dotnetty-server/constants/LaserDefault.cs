namespace dotnetty_server.constants
{
    public class LaserDefault
    {
        public static readonly byte[] Starter = new byte[] {0x3a, 0xa3 & 0xff};
        
        public const int HardwareTypeLength = 1;

        public const int MacLength = 6;
    }
}