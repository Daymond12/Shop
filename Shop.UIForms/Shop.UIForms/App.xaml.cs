using Shop.UIForms.ViewModels;
using Shop.UIForms.Views;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Shop.UIForms
{
    public partial class App : Application
    {
        public static NavigationPage Navigator { get; internal set; }

        public static MasterPage Master { get; internal set; }

        public App()
        {
            InitializeComponent();
            /*navigation Page es un contenedor, e.d estamos arrancando
             la LoginPage desde el navigation page y no directamente
             cuyos resultados son:*/

            //antes de instanciar la Page instanciamos la LoginViewModel asociada a la page
            //que la page binda
            MainViewModel.GetInstance().Login = new LoginViewModel();

            //la main page se establce cuando arranca el poyecto
            //acá le decimos que la mainPage arranca por una navigationPage de loginPage
            this.MainPage = new NavigationPage(new loginPage());

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
