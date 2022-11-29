using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace MD4_app.Utility
{
    internal class TrimmedTextBlockVisibilityConverter : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null) return Visibility.Collapsed;

            TextBlock textBlock = (TextBlock)value;

            textBlock.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));

            if (string.IsNullOrWhiteSpace(textBlock.Text))
                return Visibility.Collapsed;
            if (textBlock.ActualWidth < textBlock.DesiredSize.Width)
                return Visibility.Visible;
            else
                return Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
