using Avalonia.Controls;
using Avalonia.Markup.Xaml;

using Streaming.Composer;
using Streaming.Composer.Base;

namespace Streaming.WebCamImage.Views
{
    [ExportView("WebCamImageView")]
    public class WebCamImageView : UserControl, IView
    {
        public WebCamImageView()
        {
            this.InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
