namespace gk_common.constants
{
    public class GkDefault
    {
        //最小长度 = 帧头(1) + 目的地址(4) + 源地址(4) + 版本(1) + 控制(2) + 发送序号(1) + 接收序号(1) + 信息单元长度(2) + 帧校验(2)
        public const int Min_Length = 18;
        
        //帧头
        public const byte Starter = 0xfe;
    }
}