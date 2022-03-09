
using Newtonsoft.Json;

using Streaming.BusinessLogic.Interfaces;

using System;
using System.IO;

namespace Streaming.BusinessLogic.Implementations
{
    public class ConfigurationSetting : IConfigurationSetting
    {
        //private readonly ILogger<ConfigurationSetting> _logger;
        //private readonly IEnvironmentSetting _environmentSetting;
        //public ConfigurationSetting(ILogger<ConfigurationSetting> logger, IEnvironmentSetting environmentSetting)
        //{
        //    _logger = logger;
        //    _environmentSetting = environmentSetting;
        //}

        private readonly IEnvironmentSetting _environmentSetting;

        public ConfigurationSetting(IEnvironmentSetting environmentSetting)
        {
            _environmentSetting = environmentSetting;
        }

        public void AddOrUpdateAppSetting<T>(string key, T value)
        {
            try
            {
                string lFilePath = Path.Combine(_environmentSetting.ContentRootPath, "Settings", "appsettings.json");

                //string lFilePath = _environmentSetting.IsDevelopment ?
                //    Path.Combine(_environmentSetting.ContentRootPath, DefineConstant.AppsettingsDevelopmentFileName) :
                //    Path.Combine(AppContext.BaseDirectory, DefineConstant.AppsettingsFileName);

                string lAppSettingJson = File.ReadAllText(lFilePath);
                dynamic lDeserializedObject = JsonConvert.DeserializeObject(lAppSettingJson);

                string lSectionPath = key.Split(":")[0];
                if (!string.IsNullOrEmpty(lSectionPath) && !lSectionPath.Equals(key))
                {
                    string lKeyPath = key.Split(":")[1];
                    lDeserializedObject[lSectionPath][lKeyPath] = value;
                }
                else
                    lDeserializedObject[lSectionPath] = value; // if no section path just set the value

                string lOutput = JsonConvert.SerializeObject(lDeserializedObject, Formatting.Indented);
                File.WriteAllText(lFilePath, lOutput);
                FileLogger.Instance.Log(LogTypes.Info, "AppSetting Add Or Update Method", "Configuration Setting Changed.");


            }
            catch (Exception ex)
            {
                FileLogger.Instance.Log(LogTypes.Error, "AppSetting Add Or Update Method", ex.Message, ex.StackTrace);
            }
        }
    }
}
