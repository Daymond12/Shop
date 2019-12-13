

namespace Shop.Web.Controllers.API
{
    using Microsoft.AspNetCore.Authentication.JwtBearer;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Data;

    //Vamos a rutear el API
    /// <summary>
    /// e.d cuando publiqe mi sitio Web xxx/api/nameController
    /// accederá a los datos de ese controlador
    ///
    /// </summary>
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/[Controller]")]
    public class ProductsController : Controller
    {
        private readonly IProductRepository productRepository;


        //inyectamos el product repository para acceder a los datos del producto
        public ProductsController(IProductRepository productRepository)
        {
            this.productRepository = productRepository;

        }


        [HttpGet]
        public IActionResult GetProducts()
        {
            //return ok envuelve el resultado en un Json
            return Ok(this.productRepository.GetAllWithUsers());
        }


    }
}
