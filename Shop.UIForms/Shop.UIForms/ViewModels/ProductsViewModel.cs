

namespace Shop.UIForms.ViewModels
{
    using Shop.Common.Models;
    using Shop.Common.Services;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using Xamarin.Forms;
    public class ProductsViewModel : BaseViewModel

    {
        //ApiService es la api que nos carga los productos
        private readonly ApiService apiService;
        private bool isRefreshing;
        private ObservableCollection<Product> products;

        //products es la lista de productos que pintaremos en el View
        //esa propiedad se diferencia de la propiedad privada
        public ObservableCollection<Product> Products
        {
            //cuando enviemos lanzamos la privada
            get => this.products;
            set => this.SetValue(ref this.products, value);
            //cuando establecemos, usamos la publica y refrescamos con SetValue
  
        }

        public bool IsRefreshing
        {
            //cuando enviemos lanzamos la privada
            get => this.isRefreshing;
            set => this.SetValue(ref this.isRefreshing, value);
            //Setvalue me establece el valor del atributo y hace que la VM se dé cuenta
        }






        public ProductsViewModel()
        {
            //llamo un metodo desdel constructor porque es asincrono


            this.apiService = new ApiService();
            this.LoadProducts();
        }

        private async void LoadProducts()
        {
            this.IsRefreshing = true;
            var url = Application.Current.Resources["UrlAPI"].ToString();
            var response = await this.apiService.GetListAsync<Product>(
                url,
                "/api",
                "/Products",
                "bearer",
                MainViewModel.GetInstance().Token.Token);

            this.IsRefreshing = false;

            if (!response.IsSuccess)
            {
                //referenciamos Xamarin.Forms
                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    response.Message,
                    "Accept");
                return;
            }
            //casteamos la clase Response
            var products = (List<Product>)response.Result;
            this.Products = new ObservableCollection<Product>(products);
            this.IsRefreshing = false;
        }
    }
}
