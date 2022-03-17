using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace TestLab.Class
{
    public class OneAnswer : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null)
            {
                int type = (int)value;
                if (type == 1)
                {
                    return Visibility.Visible;
                }
            }
            return Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}