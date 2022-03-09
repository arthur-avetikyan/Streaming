using Avalonia.Data.Converters;

using Streaming.Composer.Base;

using System;
using System.Globalization;

namespace Streaming.Converters
{
    internal class UIExportViewConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return ((Lazy<IViewModel>)value).Value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
