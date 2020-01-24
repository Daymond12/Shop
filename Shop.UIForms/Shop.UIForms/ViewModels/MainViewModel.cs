
using Shop.Common.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

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

        //La MainViewModel liga las demás páginas
        //si hay una ProductPage es x que hay una ProductViewModel
        public LoginViewModel Login { get; set; }
        public ProductsViewModel Products { get; set; }


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
