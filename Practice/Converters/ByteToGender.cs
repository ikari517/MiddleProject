using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using Practice.Enum;

namespace Practice.Converters
{
    internal class ByteToGender : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!Gender.IsDefined(typeof(Gender), value)) return DependencyProperty.UnsetValue;

            return (Gender)value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!Gender.IsDefined(typeof(Gender), value)) return DependencyProperty.UnsetValue;

            return (Gender)value;
        }
    
    }
}
