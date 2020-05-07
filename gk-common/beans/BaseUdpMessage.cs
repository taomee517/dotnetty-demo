// 创建人：李鸢
// 创建时间：2020/05/06 10:58

using System.Net;

namespace gk_common.beans
{
    public class BaseUdpMessage : BaseMessage
    {
        public EndPoint Sender { get; set; }

        public BaseUdpMessage(BaseMessage baseMessage, EndPoint sender)
        {
            base.Header = baseMessage.Header;
            base.Content = baseMessage.Content;
            Sender = sender;
        }
    }
}