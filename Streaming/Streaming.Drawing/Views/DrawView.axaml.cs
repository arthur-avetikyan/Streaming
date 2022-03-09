using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

using Streaming.Composer;
using Streaming.Composer.Base;

namespace Streaming.Drawing.Views
{
    [ExportView("DrawView")]
    public class DrawView : UserControl, IView
    {
        public DrawView()
        {
            this.InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
