

namespace Shop.UIForms.ViewModels
{
    using Shop.Common.Models;
    using Shop.Common.Services;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using Xamarin.Forms;
    public class ProductsViewModel : BaseViewModel

    {
        //ApiService es la api que nos carga los productos
        private readonly ApiService apiService;
        //myProducts  nos ayudará a mantener en memoria los productos que devuelva el API
        //myProducts es una lista del objeto Product
        private List<Product> myProducts;
        private bool isRefreshing;
        //products es un aributo de tipo ObservableCollection del ProductItemViewModel
        private ObservableCollection<ProductItemViewModel> products;

        //esa propiedad se diferencia de la propiedad privada
        //products es la lista de productos que pintaremos en el View
        public ObservableCollection<ProductItemViewModel> Products
        {
            //cuando enviemos lanzamos la privada
            get { return this.products; }
            set { this.SetValue(ref this.products, value); }
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
            //cargue la lista original y recargue la lista
            this.myProducts = (List<Product>)response.Result;
            this.RefresProductsList();
        }

        public void AddProductToList(Product product)
        {
            this.myProducts.Add(product);
            this.RefresProductsList();
        }

        public void UpdateProductInList(Product product)
        {
            var previousProduct = this.myProducts.Where(p => p.Id == product.Id).FirstOrDefault();
            if (previousProduct != null)
            {
                this.myProducts.Remove(previousProduct);
            }

            this.myProducts.Add(product);
            this.RefresProductsList();
        }

        public void DeleteProductInList(int productId)
        {
            var previousProduct = this.myProducts.Where(p => p.Id == productId).FirstOrDefault();
            if (previousProduct != null)
            {
                this.myProducts.Remove(previousProduct);
            }

            this.RefresProductsList();
        }

        private void RefresProductsList()
        {
            this.Products = new ObservableCollection<ProductItemViewModel>(
               
                //porcad producto en myProducts crear una nueva instancia de ProductItemViewModel
                myProducts.Select(p => new ProductItemViewModel
                {
                    Id = p.Id,
                    ImageUrl = p.ImageUrl,
                    ImageFullPath = p.ImageFullPath,
                    IsAvailabe = p.IsAvailabe,
                    LastPurchase = p.LastPurchase,
                    LastSale = p.LastSale,
                    Name = p.Name,
                    Price = p.Price,
                    Stock = p.Stock,
                    User = p.User
                   
                    
                })    
               
            .OrderBy(p => p.Name)
            .ToList()); 
            
            
        }


    }
}

