namespace dotnetty_server.constants
{
    public enum FunctionType
    {
        PublishTlvs = 0xD00,
        
        PublishJson = 0xD01,
        
        PublishString = 0xD02,
        
        SettingEnvs = 0xC05,
        
        ReadEnvs = 0xC06,
        
        Reset = 0xC02,
        
        Recovery = 0xC10
    }
}