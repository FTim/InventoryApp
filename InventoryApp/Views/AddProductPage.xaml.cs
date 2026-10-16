using InventoryApp.ViewModels;
using System.ComponentModel;


namespace InventoryApp.Views
{
    public partial class AddProductPage : ContentPage
    {
        public AddProductPage(AddProductViewModel vm)
        {
            InitializeComponent();
            BindingContext = vm;
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (BindingContext is AddProductViewModel vm)
                vm.PropertyChanged += OnNewProductSaved;
        }

        private void OnNewProductSaved(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(AddProductViewModel.ProductSaved)
                && sender is AddProductViewModel vm
                && vm.ProductSaved)
            {
                DisplayAlert("Success", "Product added!", "Ok");
                Shell.Current.SendBackButtonPressed();
            }
        }
    }
}