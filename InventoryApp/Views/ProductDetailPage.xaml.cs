using InventoryApp.Models;
using InventoryApp.ViewModels;
using System.ComponentModel;


namespace InventoryApp.Views
{
    public partial class ProductDetailPage : ContentPage, IQueryAttributable
    {
        public ProductDetailPage(ProductDetailViewModel vm)
        {
            InitializeComponent();
            BindingContext = vm;
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (BindingContext is ProductDetailViewModel vm)
                vm.PropertyChanged += OnDetailedProductChangeSaved;
        }
        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            if (query.TryGetValue("Product", out var value) &&
                value is Product product)
            {
                var productDetailVM = this.BindingContext as ProductDetailViewModel;

                if (productDetailVM != null)
                    productDetailVM.AfterNavigationProduct(product);
            }
        }

        private void OnDetailedProductChangeSaved(object? sender, PropertyChangedEventArgs e)
        {
            if ((e.PropertyName == nameof(ProductDetailViewModel.Saved)|| e.PropertyName == nameof(ProductDetailViewModel.Deleted))
                && sender is ProductDetailViewModel vm
                && (vm.Saved || vm.Deleted))
            {
                if (vm.Saved)
                    DisplayAlert("Success", "Product updated!", "Ok");

                if(vm.Deleted)
                    DisplayAlert("Success", "Product deleted!", "Ok");

                Shell.Current.SendBackButtonPressed();
            }
        }
    }
}