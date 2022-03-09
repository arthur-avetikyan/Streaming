using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Streaming.Views
{
    public class PluginListView : UserControl
    {
        public PluginListView()
        {
            this.InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
