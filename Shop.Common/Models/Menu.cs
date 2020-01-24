using System;
using System.Collections.Generic;
using System.Text;

namespace Shop.Common.Models
{
    public class Menu
    {

        //el icono que usará
        public string Icon { get; set; }

        //el titulo de la opcion del menu
        public string Title { get; set; }

        //pagina a direccionar cuado toque el menu
        public string PageName { get; set; }
    }

}
