using ImageService.Logging.Model;
using System;
using System.Windows.Data;
using System.Windows.Media;

namespace ImageServieGUI.Converters
{
    class MessageTypeEnumConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            switch ((MessageTypeEnum)value)
            {
                case MessageTypeEnum.INFO:
                    return Brushes.YellowGreen;
                case MessageTypeEnum.WARNING:
                    return Brushes.Yellow;
                case MessageTypeEnum.FAIL:
                    return Brushes.Tomato;
            }
            return Brushes.Transparent;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
