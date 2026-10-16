using InventoryApp.Models;
using InventoryApp.Services;

namespace InventoryApp.ViewModels
{
    public class AddProductViewModel : ViewModelBase
    {
        private InventoryDatabase dbService;

        #region Binded properties
        public string Name
        {
            get => GetValue<string>();
            set { SetValue<string>(value); }
        }
        public string Barcode
        {
            get => GetValue<string>();
            set { SetValue<string>(value); }
        }
        public decimal Price
        {
            get => GetValue<decimal>();
            set { SetValue<decimal>(value); }
        }
        public decimal Quantity
        {
            get => GetValue<decimal>();
            set { SetValue<decimal>(value); }
        }
        public bool ViewMode
        {
            get => GetValue<bool>();
            set { SetValue<bool>(value); }
        }
        public bool ProductSaved
        {
            get => GetValue<bool>();
            set { SetValue<bool>(value); }
        }
        public string ProductValidation
        {
            get => GetValue<string>();
            set { SetValue<string>(value); }
        }
        #endregion
        public Command SaveProduct { get; }
        public AddProductViewModel(InventoryDatabase db)
        {
            dbService = db;
            ProductSaved = false;
            ProductValidation=string.Empty;
            SaveProduct = new Command(async () => await SaveProductExecutionAsync());
        }

        private async Task SaveProductExecutionAsync()
        {
            ProductSaved = false;
            ProductValidation=string.Empty;
            var newProduct = new Product()
            {
                Name = Name,
                Barcode = Barcode,
                Price = Price,
                Quantity = Quantity,
                ID = 0
            };
            string validationResult = newProduct.Validate();
            if (string.IsNullOrEmpty(validationResult))
            {
                await dbService.SaveProductAsync(newProduct);

                ProductSaved = true;
            }
            else
            {
                ProductValidation = validationResult;
            }
        }
    }
}
