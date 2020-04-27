namespace gk_common.beans
{
    public class Control
    {
        public short SrcValue { get; set; }
        
        //0-响应 1-命令
        public int ControlType { get; set; }

        public bool IsEncrypt { get; set; }
        
        //是否有签名标记
        public bool HasSignature { get; set; }
        
        public bool HasStarter { get; set; }
        
        public bool HasEndSign { get; set; }
        
        //0-上报 1-下发 2，3-其他
        public int LogicSignal { get; set; }
        
        //0-自报 1-需要确认 2-强制确认 3-心跳
        public int AckType { get; set; }
        
        //0-确认 1-忙 2-异常 3-保留
        public int AckStatus { get; set; }
        
    }
}