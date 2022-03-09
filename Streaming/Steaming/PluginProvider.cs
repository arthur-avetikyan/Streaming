using Streaming.BusinessLogic;
using Streaming.Composer;
using Streaming.Composer.UI;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;

namespace Streaming
{
    public class PluginProvider : IPartImportsSatisfiedNotification
    {
        private static object _lock = new object();
        private static PluginProvider _instance = null;

        static PluginProvider()
        {

        }

        public PluginProvider()
        {
            try
            {
                PluginManager.Initialize(this, isRecomposable: true);

            }
            catch (System.Exception ex)
            {
                FileLogger.Instance.Log(LogTypes.Error, "PluginManager Initialization", ex.Message, ex.StackTrace);
            }

        }

        [ImportMany(typeof(IUIViewProviderBase), AllowRecomposition = true)]
        public ObservableCollection<IUIViewProviderBase> Plugins { get; set; }

        public static PluginProvider Instance
        {
            get
            {
                lock (_lock)
                {
                    if (_instance == null)
                    {
                        _instance = new PluginProvider();
                    }
                }
                return _instance;
            }
        }

        public void OnImportsSatisfied()
        {
            // uncomment if you need to do something when plugin(s) is/are loaded
            //Debugger.Break();
        }
    }
}
