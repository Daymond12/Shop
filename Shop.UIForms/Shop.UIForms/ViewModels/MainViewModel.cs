
using GalaSoft.MvvmLight.Command;
using Shop.Common.Models;
using Shop.UIForms.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace Shop.UIForms.ViewModels
{
    public class MainViewModel
    {

        //esto es puntero a la misma clase
        private static MainViewModel instance;

        //propiedad para alimentar el menu
        //El nombre Menus debe coincidir con lo que se liga en la Binding MenuPage
        public ObservableCollection<MenuItemViewModel> Menus { get; set; }

        //Guardar en memoria el Token
        public TokenResponse Token { get; set; }

        //guardamos el Email x q necesitamos saber que usuario
        //crea el producto
        public string UserEmail { get; set; }

        //guardaremos el UserPassword x que en caso que el usuario haga reset a su password
        //se pedirá el usuario actual, e.d el correcto
        public string UserPassword { get; set; }


        //La MainViewModel liga las demás páginas
        //si hay una ProductPage es x que hay una ProductViewModel
        public LoginViewModel Login { get; set; }
        public ProductsViewModel Products { get; set; }

        public AddProductViewModel AddProduct { get; set; }

        public EditProductViewModel EditProduct { get; set; }

        public ICommand AddProductCommand { get { return new RelayCommand(this.GoAddProduct); } }

       

        public MainViewModel()
        {
            //con el singleton ya no es necesario instanciar en el ctor
            //la mainViewModel
            // this.Login = new LoginViewModel();

            //cuando instancie la MVM x 1ra vez, instance quedará apuntando
            //donde este la dirección señalada por el locator
            //pero requiere de un metodo privado y estatico
            instance = this;
            this.LoadMenus();
        }

        //GoAddProduct llama la página del producto
        private async void GoAddProduct()
        {
            //estamos en la MainViewModel apuntamos con this
            this.AddProduct = new AddProductViewModel();
            //apilamos y lo ponemos a navegar
            await App.Navigator.PushAsync(new AddProductPage());
        }
        //devuelve una instancia de la mainviewModel
        private void LoadMenus()
        {
            //menus es una nueva lista de Menu
            var menus = new List<Menu>
     {
        new Menu
        {
            Icon = "ic_info",
            PageName = "AboutPage",
            Title = "About"
        },

        new Menu
        {
            Icon = "ic_phonelink_setup",
            PageName = "SetupPage",
            Title = "Setup"
        },

        new Menu
        {
            Icon = "ic_exit_to_app",
            PageName = "loginPage",
            Title = "Close session"
        }
    };

            this.Menus = new ObservableCollection<MenuItemViewModel>(
                menus.Select(m => new MenuItemViewModel
            {
                Icon = m.Icon,
                PageName = m.PageName,
                Title = m.Title
            }).ToList());
        }


        public static MainViewModel GetInstance()
        {

            if(instance==null)
            {
                return new MainViewModel();
            }
            return instance;
        }

    }
}
