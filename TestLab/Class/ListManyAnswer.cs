using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace TestLab.Class
{
    public class ListManyAnswer : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null)
            {
                int number = (int)value;
                if (number == 2)
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