using System;
using System.Net;
using System.Text;

namespace dotnetty_feature.utils
{
    public static class IpAddressExtensions
    {
        public static IPAddress GetBroadcast(this IPAddress ipAddress, int mask)
        {
            if (ipAddress.IsIPv4MappedToIPv6)
            {
                ipAddress = ipAddress.MapToIPv4();
            }
            var index = 0;
            var maskBytes = new byte[4];
            for (int i = 0; i < 4; i++)
            {
                var maskString = new StringBuilder();
                for (int j = 0; j < 8; j++)
                {
                    maskString.Append(index < mask ? "1" : "0");
                    index += 1;
                }

                maskBytes[i] = (byte)Convert.ToInt32(maskString.ToString(), 2);
            }

            var maskAddress = new IPAddress(maskBytes);
            var sub = maskAddress.GetAddressBytes();
            var ip = ipAddress.GetAddressBytes();
            for (int i = 0; i < ip.Length; i++)
            {
                ip[i] = (byte)(~sub[i] | ip[i]);
            }

            var broadcastIpAddress = new IPAddress(ip);
            return broadcastIpAddress;
        }
    }
}