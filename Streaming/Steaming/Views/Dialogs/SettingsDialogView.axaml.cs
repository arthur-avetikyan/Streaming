using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Streaming.Views.Dialogs
{
    public class SettingsDialogView : UserControl
    {
        public SettingsDialogView()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
