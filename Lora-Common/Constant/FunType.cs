// 创建人：taomee
// 创建时间：2020/06/02 18:14

namespace Lora_Common.Constant
{
    public enum FunType
    {
        ErrorAck = 0x0001,
        HeartBeat = 0x4064,
        HeartBeatAck = 0x0065,
        HeartBeatIntervalSetting = 0x4066,
        HeartBeatIntervalSettingAck = 0x0067,
        
        GatewayCacheDataSetting = 0x406E,
        GatewayCacheDataSettingAck = 0x006F,
        GatewayCacheDataPublish = 0x4070,
        GatewayCacheDataPublishAck = 0x0071,
        
        WirelessDataSetting = 0x4072,
        WirelessDataSettingAck = 0x0073,
        WirelessDataPublish = 0x4074,
        WirelessDataPublishAck = 0x0075,
        
        
        GatewayCacheDataBPublish = 0x4076,
        GatewayCacheDataBPublishAck = 0x0077,
        WirelessCacheDataBPublish = 0x4078,
        WirelessCacheDataBPublishAck = 0x0079,
        
        
        Setting = 0x407A,
        SettingAck = 0x007B,
        SettingError = 0x007C,
        
        Query = 0x407D,
        QueryError = 0x007D,
        QueryAck = 0x007F

    }
}