

namespace Shop.UIForms.ViewModels
{
    using GalaSoft.MvvmLight.Command;
    using Shop.UIForms.Views;
    using System;
    using System.Windows.Input;
    using Xamarin.Forms;

    public class LoginViewModel
    {

        
        public string Email { get; set; }

        public string Password { get; set; }

        //Delegado =>
        public ICommand LoginCommand => new RelayCommand(Login);

        public LoginViewModel()
        {
            this.Email = "drogermoises@gmail.com";
            this.Password="123456";
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

            if (!this.Email.Equals("drogermoises@gmail.com") || !this.Password.Equals("123456"))
            {
                await Application.Current.MainPage.DisplayAlert(
                   "Error",
                   "User or Password wrong",
                   "Accept");
                return;
            }

            //await Application.Current.MainPage.DisplayAlert(
            //    "Ok",
            //    "Fuck Yeah",
            //    "Accept");

            MainViewModel.GetInstance().Products = new ProductsViewModel();

            await Application.Current.MainPage.Navigation.PushAsync(new ProductsPage());
        }
    }
}
