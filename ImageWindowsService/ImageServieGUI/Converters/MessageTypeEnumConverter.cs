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
                    return Brushes.LightGreen;
                case MessageTypeEnum.WARNING:
                    return Brushes.Red;
                case MessageTypeEnum.FAIL:
                    return Brushes.Yellow;
            }
            return Brushes.Transparent;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
