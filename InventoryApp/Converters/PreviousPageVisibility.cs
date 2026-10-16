using System.Globalization;

namespace InventoryApp.Converters
{
    public class PreviousPageVisibility : IValueConverter
    {
        public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if(int.TryParse(value?.ToString(), out int currentPage))
                return currentPage > 1;

            return false;
        }

        public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
