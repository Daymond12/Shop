
namespace Shop.UIForms.ViewModels
{
    public class MainViewModel
    {

        //esto es puntero a la misma clase
        private static MainViewModel instance;

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
        }
        //devuelve una instancia de la mainviewModel

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
