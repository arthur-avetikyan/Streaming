namespace Streaming.BusinessLogic.Interfaces
{
    public interface IConfigurationSetting
    {
        void AddOrUpdateAppSetting<T>(string key, T value);
    }
}
