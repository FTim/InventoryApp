using InventoryApp.Models;
using InventoryApp.Services;

namespace InventoryApp.ViewModels
{
    public class ProductDetailViewModel : ViewModelBase
    {
        private InventoryDatabase dbService;
        private Product originalProduct;
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
        public bool EditMode
        {
            get => GetValue<bool>();
            set
            {
                SetValue<bool>(value);
                ViewMode = !value;
            }
        }
        public bool ViewMode
        {
            get => GetValue<bool>();
            set { SetValue<bool>(value); }
        }
        public bool Saved
        {
            get => GetValue<bool>();
            set { SetValue<bool>(value); }
        }
        public bool Deleted
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

        #region Binden commands
        public Command EditProduct { get; }
        public Command SaveProduct { get; }
        public Command DiscardChanges { get; }
        public Command DeleteProduct { get; }
        #endregion
        public ProductDetailViewModel(InventoryDatabase db)
        {
            dbService = db;
            Saved = false;
            Deleted = false;
            ProductValidation=string.Empty;
            originalProduct = new Product(); //itt will be correctly assigned after navigation
            DiscardChanges = new Command(() =>  DiscardChangesExecution());
            EditProduct = new Command(() => EditProductExecution());
            DeleteProduct = new Command(async () => await DeleteProductExecutionAsync());
            SaveProduct = new Command(async () => await SaveProductExecutionAsync());
        }

        public void AfterNavigationProduct(Product product)
        {
            EditMode = false;
            Saved = false;
            Deleted = false;
            ProductValidation = string.Empty;

            Name = product.Name;
            Barcode=product.Barcode;
            Price = product.Price;
            Quantity= product.Quantity;

            originalProduct = new Product()
            {
                Name = product.Name,
                Barcode = product.Barcode,
                Price = product.Price,
                Quantity = product.Quantity,
                ID = product.ID
            };            
        }
        private void DiscardChangesExecution()
        {
            EditMode=false;
            ProductValidation=string.Empty;

            Name = originalProduct.Name;
            Barcode = originalProduct.Barcode;
            Price = originalProduct.Price;
            Quantity = originalProduct.Quantity;
        }
        private void EditProductExecution()
        {
            EditMode = true;
        }

        private async Task DeleteProductExecutionAsync()
        {
            Deleted = false;
            await dbService.DeleteProductAsync(originalProduct);
            Deleted = true;
        }
        private async Task SaveProductExecutionAsync()
        {
            Saved = false;
            ProductValidation = string.Empty;
            var updatedProduct = new Product()
            {
                Name = Name,
                Barcode = Barcode,
                Price = Price,
                Quantity = Quantity,
                ID = originalProduct.ID
            };

            string validationResult = updatedProduct.Validate();
            if (string.IsNullOrEmpty(validationResult))
            {
                await dbService.SaveProductAsync(updatedProduct);
                EditMode = false;
                Saved = true;
            }
            else
            {
                ProductValidation = validationResult;
            }            
        }
    }
}
