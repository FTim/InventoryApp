using System.Globalization;

namespace InventoryApp.Converters
{
    public class QuantityColor : IValueConverter
    {
        public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (int.TryParse(value?.ToString(), out int quantity))
                if (quantity == 0)
                {
                    return (Color)Application.Current!.Resources["MyWarningColor"];                    
                }

            return (Color)Application.Current!.Resources["MyCardBackgroundColor"];
        }

        public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
