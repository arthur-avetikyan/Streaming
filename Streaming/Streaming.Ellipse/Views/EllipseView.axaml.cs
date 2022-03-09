using Avalonia.Controls;
using Avalonia.Markup.Xaml;

using Streaming.Composer;
using Streaming.Composer.Base;

namespace Streaming.Ellipse.Views
{
    [ExportView("EllipseView")]
    public class EllipseView : UserControl, IView
    {
        public EllipseView()
        {
            this.InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
