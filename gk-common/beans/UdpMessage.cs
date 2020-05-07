// 创建人：李鸢
// 创建时间：2020/05/06 11:01

using System.Net;

namespace gk_common.beans
{
    public class UdpMessage : Message
    {
        public EndPoint Sender { get; set; }

        public UdpMessage(Message msg, EndPoint sender)
        {
            base.Header = msg.Header;
            base.Bodies = msg.Bodies;
            Sender = sender;
        }
    }
}