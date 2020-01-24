

namespace Shop.UIForms.ViewModels
{
    using GalaSoft.MvvmLight.Command;
    using Shop.Common.Models;
    using Shop.Common.Services;
    using Shop.UIForms.Views;
    using System;
    using System.Windows.Input;
    using Xamarin.Forms;

    public class LoginViewModel: BaseViewModel
    {
        private bool isRunning;
        private bool isEnabled;
        private readonly ApiService apiService;

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



        public string Email { get; set; }

        public string Password { get; set; }

        //Delegado =>
        public ICommand LoginCommand => new RelayCommand(Login);

        public LoginViewModel()
        {
            this.Email = "drogermoises@gmail.com";
            this.Password="123456";
            //Preferible que los servicios los instanciemos en el 
            //constructor de la clase
            this.apiService = new ApiService();
            //inicializamos de arranque el botón
            this.isEnabled = true;
        }

        private async void Login()
        {
            //hacemos las validaciones
            if (string.IsNullOrEmpty(this.Email))
            {
                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    "you must enter an email.",
                    "Accept");
                return;
            }

            if (string.IsNullOrEmpty(this.Password))
            {
                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    "you must enter a Password.",
                    "Accept");
                return;
            }

            this.IsRunning = true;
            this.IsEnabled = false;

            var request = new TokenRequest
            {
                Password = this.Password,
                Username = this.Email
            };

            var url = Application.Current.Resources["UrlAPI"].ToString();
            var response = await this.apiService.GetTokenAsync(
                url,
                "/Account",
                "/CreateToken",
                request);

            this.IsRunning = false;
            this.IsEnabled = true;

            if (!response.IsSuccess)
            {
                await Application.Current.MainPage.DisplayAlert(
                "Error", "Email or password incorrect.", "Accept");
                return;
            }

            var token = (TokenResponse)response.Result;
            var mainViewModel = MainViewModel.GetInstance();
            mainViewModel.Token = token;
            mainViewModel.Products = new ProductsViewModel();
            await Application.Current.MainPage.Navigation.PushAsync(new ProductsPage());


            MainViewModel.GetInstance().Products = new ProductsViewModel();

            //la sigg linea es para apilar la navigation
            //await Application.Current.MainPage.Navigation.PushAsync(new ProductsPage());

            /*En vez de apilar lo que haremos será cambiar la mainPage*/
            /*El Main Page se establece cuando arranca el proyecto y lo que se le dice
             es que ya no arrancará como una navigationPage sino como una masterPage
             vea que esto se hace en tiempo de ejecución y no donde arranca el proyecto,
             la master Page monta una navegación con la ProductPage pero le monta una navegación encima*/
            Application.Current.MainPage = new MasterPage();
        }
    }
}
