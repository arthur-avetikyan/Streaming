using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Streaming.Views
{
    public class RegisterView : UserControl
    {
        public RegisterView()
        {
            this.InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
