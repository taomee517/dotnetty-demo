// 创建人：taomee
// 创建时间：2020/06/02 14:58

namespace Lora_Common.Constant
{
    public class LoraConst
    {
        public const byte StartSign = 0xA5;
        
        //报文最小长度： 协议标识(1) + 任务序号(1) + 报文顺序号(1) + 生存时间(1) + 通信参数(1) + 时间戳(4) + 控制命令(2) + 源地址(6) + 目的地址(6) + 数据长度(2) + CRC(2)
        public const int MinLength = 27;
    }
}