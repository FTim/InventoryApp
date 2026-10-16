using InventoryApp.Models;
using InventoryApp.Services;
using System.Collections.ObjectModel;

namespace InventoryApp.ViewModels
{
    public class HomeViewModel :ViewModelBase
    {
        private InventoryDatabase dbService;
        private string lastSearched;
        #region Binded properties
        public ObservableCollection<Product> Products
        {
            get => GetValue<ObservableCollection<Product>>();
            set { SetValue<ObservableCollection<Product>>(value); }
        }
        public string SearchString
        {
            get => GetValue<string>();
            set { SetValue<string>(value); }
        }
        public int CurrentPage
        {
            get => GetValue<int>();
            set { SetValue<int>(value); }
        }
        public int TotalPages 
        {
            get => GetValue<int>();
            set { SetValue<int>(value); }
        }
        public int NumberOfProducts
        {
            get => GetValue<int>();
            set { SetValue<int>(value); }
        }
        #endregion

        #region Binded commands
        public Command SearchByName { get; set; }
        public Command SearchByBarcode { get; set; }
        public Command ClearSearch { get; set; }
        public Command FirstPage { get; set; }
        public Command PreviousPage { get; }
        public Command NextPage { get; }
        public Command LastPage { get; set; }
        #endregion
        
        public HomeViewModel(InventoryDatabase db)
        {
            dbService = db;
            lastSearched = string.Empty;
            Products = new ObservableCollection<Product>();

            FirstPage = new Command(async () => await FirstPageExecutionAsync());
            LastPage = new Command(async () => await LastPageExecutionAsync());
            PreviousPage = new Command(async () => await NthPageExecutionAsync(CurrentPage-1));
            NextPage = new Command(async () => await NthPageExecutionAsync(CurrentPage + 1));
            SearchByName = new Command(async() => await SearchProductsByNameExecutionAsync());
            SearchByBarcode = new Command(async() => await SearchProductsByBarcodeExecutionAsync());
            ClearSearch = new Command(async () => await ClearSearchExecutionAsync());
        }
        private async Task FirstPageExecutionAsync()
        {
            await NthPageExecutionAsync(1);
        }
        private async Task LastPageExecutionAsync()
        {
            await NthPageExecutionAsync(TotalPages);
        }
        private async Task NthPageExecutionAsync(int n)
        {
            CurrentPage = n;
            List<Product> list = new List<Product>();
            switch (lastSearched)
            {
                case ("name"):
                    NumberOfProducts = await dbService.GetProductCountForNameAsync(SearchString);
                    list = await dbService.SearchProductsByName(SearchString,CurrentPage);
                    break;

                case ("barcode"):
                    NumberOfProducts = await dbService.GetProductCountForBarcodeAsync(SearchString);
                    list = await dbService.SearchProductsByBarcode(SearchString, CurrentPage);
                    break;

                default:
                    NumberOfProducts = await dbService.GetProductCountAsync();
                    list = await dbService.GetProductsAsync(CurrentPage);
                    break;
            }

            TotalPages = (int)Math.Ceiling(NumberOfProducts / 10.00);
            
            Products.Clear();
            foreach (var item in list)
            {
                Products.Add(item);
            }
        }
        private async Task SearchProductsByBarcodeExecutionAsync()
        {            
            lastSearched = "barcode";
            await NthPageExecutionAsync(1);
        }
        private async Task SearchProductsByNameExecutionAsync()
        {
            lastSearched = "name";
            await NthPageExecutionAsync(1);
        }
        private async Task ClearSearchExecutionAsync()
        {
            lastSearched = String.Empty;
            SearchString = String.Empty;
            await NthPageExecutionAsync(1);
        }
    }
}
