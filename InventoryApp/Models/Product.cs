using SQLite;

namespace InventoryApp.Models
{
    public class Product
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }
        public string Name { get; set; }
        public string Barcode { get; set; }
        public decimal Price { get; set; }
        public decimal Quantity { get; set; }

        public string Validate()
        {
            string res = string.Empty;

            if (string.IsNullOrWhiteSpace(Name))
                res += $"Name cannot be empty{Environment.NewLine}";

            if (string.IsNullOrWhiteSpace(Barcode))
                res += $"Barcode cannot be empty{Environment.NewLine}";

            if (Price <= 0.00m)
                res += $"Price cannot be negative or 0{Environment.NewLine}";

            if (Quantity < 0.00m)
                res += $"Quantity cannot be negative{Environment.NewLine}";

            if (Quantity % 1m != 0m)
                res += $"Quantity must be a whole number{Environment.NewLine}";

            return res;
        }
    }
}
