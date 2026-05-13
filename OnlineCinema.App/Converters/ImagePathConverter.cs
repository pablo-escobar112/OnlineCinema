// OnlineCinema.App/Converters/ImagePathConverter.cs
using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace OnlineCinema.App.Converters
{
    public class ImagePathConverter : IValueConverter
    {
        public object? Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string path && !string.IsNullOrEmpty(path))
            {
                try
                {
                    var uri = new Uri("pack://application:,,,/Images/" + path, UriKind.Absolute);
                    return new BitmapImage(uri);
                }
                catch
                {
                    return null;
                }
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}