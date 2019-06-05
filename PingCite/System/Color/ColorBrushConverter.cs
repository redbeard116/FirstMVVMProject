using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace PingSite
{
    public class ColorBrushConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string input = value as string;
            switch (input)
            {
                case "Доступен":
                    return Brushes.LimeGreen;
                case "Не доступен":
                    return Brushes.Red;
                default:
                    return Brushes.Orange;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
