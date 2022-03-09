using Avalonia.Markup.Xaml;

using AvaloniaStyles = Avalonia.Styling.Styles;


namespace Streaming.Styles.Themes
{
    public class DarkTheme : AvaloniaStyles
    {
        public DarkTheme()
        {
            this.InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
