

namespace Shop.Web.Models
{

    using Microsoft.AspNetCore.Http;
    using Shop.Web.Data.Entities;
    

    public class ProductViewModel : Product
    {

        //mediante la clase IFormFile capturamos la img en memoria
        //y se puede subir al servidor
        public IFormFile ImageFile { get; set; }


    }
}
