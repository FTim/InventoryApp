using InventoryApp.Models;
using InventoryApp.ViewModels;

namespace InventoryApp.Views
{
    public partial class HomePage : ContentPage
    {
        public HomePage(HomeViewModel vm)
        {
            InitializeComponent();
            BindingContext = vm;
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            var homeVM = this.BindingContext as HomeViewModel;

            if (homeVM != null)
                homeVM.FirstPage.Execute(this);
        }
        private async void OnProductTapped(object? sender, TappedEventArgs e)
        {
            Border? senderB = sender as Border;
            if (senderB?.BindingContext is not Product)
            {
                return;
            }

            var parameters = new Dictionary<string, object>
            {
                ["Product"] = senderB.BindingContext
            };

            await Shell.Current.GoToAsync(nameof(ProductDetailPage), parameters);
        }
        private async void OnAddClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync(nameof(AddProductPage));
        }
    }
}