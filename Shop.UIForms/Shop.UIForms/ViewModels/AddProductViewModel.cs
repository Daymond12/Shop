

namespace Shop.UIForms.ViewModels
{
    using AutoMapper;
    using Common.Models;
    using Common.Services;
    using GalaSoft.MvvmLight.Command;
    using System.Windows.Input;
    using Xamarin.Essentials;
    using Xamarin.Forms;

    public class AddProductViewModel : BaseViewModel
    {
        private bool isRunning;
        private bool isEnabled;
        //apiService, si creo un nuevo producto lo debo mandar al Api
        private readonly ApiService apiService;

        public string Image { get; set; }


        public bool IsRunning
        {
            get => this.isRunning;
            set => this.SetValue(ref this.isRunning, value);
        }

        public bool IsEnabled
        {
            get => this.isEnabled;
            set => this.SetValue(ref this.isEnabled, value);
        }

        public string Name { get; set; }

        public string Price { get; set; }

        public ICommand SaveCommand => new RelayCommand(this.Save);

        public AddProductViewModel()
        {
            this.apiService = new ApiService();
            this.Image = "noImage";
            this.IsEnabled = true;
        }

        private async void Save()
        {
            if (string.IsNullOrEmpty(this.Name))
            {
                await Application.Current.MainPage.DisplayAlert(
                 "Error",
                 "You must enter a product name.",
                 "Accept");
                return;
            }

            if (string.IsNullOrEmpty(this.Price))
            {
                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    "You must enter a product price.",
                    "Accept");
                return;
            }

            var price = decimal.Parse(this.Price);
            if (price <= 0)
            {
                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    "The price must be a number greather than zero.",
                    "Accept");
                return;
            }

            this.IsRunning = true;
            this.IsEnabled = false;

            //TODO: Add image
            //a continuación el objeto que mandaremos al post
            var product = new Product
            {
                IsAvailabe = true,
                Name = this.Name,
                Price = price,        
                User = new User { Email = MainViewModel.GetInstance().UserEmail }
            };

            //obtenemos la url
            var url = Application.Current.Resources["UrlAPI"].ToString();
            //hacemos el post y mandamos el objeto product
            //como es un metodo seguro mandamos el bearer y el token    
            //recordemos que el token lo sacamos de la mainViewModel
            var response = await this.apiService.PostAsync(
                url,
                "/api",
                "/Products",
                product,
                "bearer",
                MainViewModel.GetInstance().Token.Token);

            if (!response.IsSuccess)
            {
                await Application.Current.MainPage.DisplayAlert
                    ("Error", response.Message, "Accept");
                return;
            }
            //castear la respuesta como un producto
            var newProduct = (Product)response.Result;
            //GetInstance es el SingleTown
            MainViewModel.GetInstance().Products.AddProductToList(newProduct);
       
            
         

            this.IsRunning = false;
            this.IsEnabled = true;
            //devolver al nivel anterior
            await App.Navigator.PopAsync();
        }
    }

}
