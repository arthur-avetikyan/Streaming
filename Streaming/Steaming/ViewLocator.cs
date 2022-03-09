using Avalonia.Controls;
using Avalonia.Controls.Templates;

using Streaming;
using Streaming.Composer;

using System;
using System.Linq;

namespace Streaming
{
    public class ViewLocator : IDataTemplate
    {
        public bool SupportsRecycling => false;

        public IControl Build(object data)
        {
            var name = data.GetType().FullName.Replace("ViewModel", "View");
            var type = Type.GetType(name);

            if (type != null)
            {
                return (Control)Activator.CreateInstance(type);
            }
            else
            {
                var pluginKey = name.Substring(name.LastIndexOf('.') + 1).Replace("View", "");
                var plugin = PluginProvider.Instance.Plugins.FirstOrDefault(p => p.Key.Equals(pluginKey));
                if (plugin != null)
                {
                    return (Control)plugin.View.Value;
                }
                return new TextBlock { Text = "Not Found: " + name };
            }
        }

        public bool Match(object data)
        {
            return data is ViewModelBase;
        }
    }
}