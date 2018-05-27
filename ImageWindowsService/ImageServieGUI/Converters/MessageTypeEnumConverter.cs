using ImageService.Logging.Model;
using System;
using System.Windows.Data;
using System.Windows.Media;

namespace ImageServieGUI.Converters
{
    /// <summary>
    /// Class MessageTypeEnumConverter.
    /// </summary>
    /// <seealso cref="System.Windows.Data.IValueConverter" />
    class MessageTypeEnumConverter : IValueConverter
    {
        /// <summary>
        /// Converts a value.
        /// </summary>
        /// <param name="value">The value produced by the binding source.</param>
        /// <param name="targetType">The type of the binding target property.</param>
        /// <param name="parameter">The converter parameter to use.</param>
        /// <param name="culture">The culture to use in the converter.</param>
        /// <returns>A converted value. If the method returns null, the valid null value is used.</returns>
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

        /// <summary>
        /// Converts a value.
        /// </summary>
        /// <param name="value">The value that is produced by the binding target.</param>
        /// <param name="targetType">The type to convert to.</param>
        /// <param name="parameter">The converter parameter to use.</param>
        /// <param name="culture">The culture to use in the converter.</param>
        /// <returns>A converted value. If the method returns null, the valid null value is used.</returns>
        /// <exception cref="System.NotSupportedException"></exception>
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
