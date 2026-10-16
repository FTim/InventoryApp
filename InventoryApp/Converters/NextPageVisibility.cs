using System.Globalization;

namespace InventoryApp.Converters
{
    internal class NextPageVisibility : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object? parameter, CultureInfo culture)
        {
            if (values.Length < 2)
                return false;

            if (int.TryParse(values[0]?.ToString(), out int currentPage) &&
                int.TryParse(values[1]?.ToString(), out int totalPages))
                return currentPage < totalPages;

            return false;
        }

        public object[] ConvertBack(object? value, Type[] targetTypes, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
