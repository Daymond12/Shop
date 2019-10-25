using Shop.UIForms.Views;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Shop.UIForms
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            /*navigation Page es un contenedor, e.d estamos arrancando
             la LoginPage desde el navigation page y no directamente
             cuyos resultados son:*/
            MainPage = new NavigationPage(new loginPage());

        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
