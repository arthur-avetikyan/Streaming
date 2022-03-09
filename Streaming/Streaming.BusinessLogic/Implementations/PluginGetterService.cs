
using Streaming.BusinessLogic.Interfaces;
using Streaming.BusinessLogic.Models;

using System;
using System.Timers;
using System.IO;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Streaming.BusinessLogic.Implementations
{
    public class PluginGetterService : IRecurringTask, IDisposable
    {
        private readonly ApplicationSettings _setting;
        private readonly IEnvironmentSetting _environmentSetting;

        private System.Timers.Timer _timer;
        private int _executionCount;
        private bool disposedValue;

        public PluginGetterService(ApplicationSettings setting, IEnvironmentSetting environmentSetting)
        {
            _executionCount = 0;
            _setting = setting;
            _environmentSetting = environmentSetting;
            _timer = new System.Timers.Timer();
            SetupTimer(_timer);
        }

        public void SetupTimer(System.Timers.Timer timer)
        {
            timer.Interval = 60000;
            timer.Elapsed += CheckForNewPlugins;
            timer.AutoReset = true;
        }

        public void Start() => _timer.Start();

        public void Stop() => _timer.Stop();

        private async void CheckForNewPlugins(object sender, ElapsedEventArgs e)
        {
            var count = Interlocked.Increment(ref _executionCount);

            try
            {
                if (_setting.KioskIdentifier != Guid.Empty)
                {
                    using HttpClient client = new HttpClient { BaseAddress = new Uri(_setting.ApiBaseUrl) };
                    client.DefaultRequestHeaders.Add(nameof(_setting.KioskIdentifier), _setting.KioskIdentifier.ToString());

                    var response = await client.GetFromJsonAsync<ApiResponseDetails<KioskPluginResponse>>($"api/KioskPlugins");
                    FileLogger.Instance.Log(LogTypes.Event, "Get Plugin API Called");

                    if (response.IsSuccessStatusCode)
                    {
                        await SaveNewPlugin(response.Result);
                    }
                }
            }
            catch (Exception ex)
            {
                FileLogger.Instance.Log(LogTypes.Error, "CheckForNewPlugins Method", ex.Message, ex.StackTrace);
            }
        }

        private async Task SaveNewPlugin(KioskPluginResponse kioskPlugin)
        {
            try
            {
                var folderPath = Path.Combine(_environmentSetting.ContentRootPath, "Plugins");
                Directory.CreateDirectory(folderPath);

                string pluginName = Path.GetFileNameWithoutExtension(kioskPlugin.Path)?.ToLower().Replace(" ", "_");
                string extension = Path.GetExtension(kioskPlugin.Path);
                string fileName = pluginName + extension;
                string filePath = Path.Combine(folderPath, fileName);

                await File.WriteAllBytesAsync(filePath, kioskPlugin.File);

                FileLogger.Instance.Log(LogTypes.Info, "Plugin Downloaded", $"In path: {filePath}");

            }
            catch (Exception ex)
            {
                FileLogger.Instance.Log(LogTypes.Error, "SaveNewPlugin Method", ex.Message, ex.StackTrace);
            }
        }

        #region IDisposable pattern

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects)
                    _timer.Stop();
                    _timer.Close();
                }

                // TODO: free unmanaged resources (unmanaged objects) and override finalizer
                // TODO: set large fields to null
                disposedValue = true;
            }
        }

        // // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
        // ~PluginGetterService()
        // {
        //     // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        //     Dispose(disposing: false);
        // }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}