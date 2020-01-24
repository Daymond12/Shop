

namespace Shop.Web.Controllers.API
{
    using Microsoft.AspNetCore.Authentication.JwtBearer;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Data;
    using System.Threading.Tasks;
    using Shop.Web.Helpers;
    using Shop.Web.Data.Entities;


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
        private readonly IUserHelper userHelper;




        //inyectamos el product repository para acceder a los datos del producto
        public ProductsController(
            IProductRepository productRepository,
            IUserHelper userHelper)
        {
            this.productRepository = productRepository;
            this.userHelper = userHelper;
        }


        [HttpGet]
        public IActionResult GetProducts()
        {
            //return ok envuelve el resultado en un Json
            return Ok(this.productRepository.GetAllWithUsers());
        }

        //FromBody e.d lo tomará del cuerpo del mensaje
        //buscará del cuerpo del mensaje un ommon.Models.Product
        //e.d el producto que me manda el telefono
        //Common.Models.Product product

        [HttpPost]
        public async Task<IActionResult> PostProduct([FromBody] Common.Models.Product product)
        {
            if (!ModelState.IsValid)
            {
                return this.BadRequest(ModelState);
            }

            var user = await this.userHelper.GetUserByEmailAsync(product.User.Email);
            if (user == null)
            {
                return this.BadRequest("Invalid user");
            }
                        //TODO: Upload images
             //El sig producto es de Entitie
             //armamos el objeto que mandaremos a base de datos con el producto del frombody
            var entityProduct = new Product
            {
                IsAvailabe = product.IsAvailabe,
                LastPurchase = product.LastPurchase,
                LastSale = product.LastSale,
                Name = product.Name,
                Price = product.Price,
                Stock = product.Stock,
                User = user
            };

            var newProduct = await this.productRepository.CreateAsync(entityProduct);
            //acá devuelve el objeto como se mando a la BD
            return Ok(newProduct);
        }

        //
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProduct([FromRoute] int id, [FromBody] Common.Models.Product product)
        {
            if (!ModelState.IsValid)
            {
                return this.BadRequest(ModelState);
            }

            if (id != product.Id)
            {
                return BadRequest();
            }

            var oldProduct = await this.productRepository.GetByIdAsync(id);
            if (oldProduct == null)
            {
                return this.BadRequest("Product Id don't exists.");
            }

            //TODO: Upload images
            oldProduct.IsAvailabe = product.IsAvailabe;
            oldProduct.LastPurchase = product.LastPurchase;
            oldProduct.LastSale = product.LastSale;
            oldProduct.Name = product.Name;
            oldProduct.Price = product.Price;
            oldProduct.Stock = product.Stock;

            var updatedProduct = await this.productRepository.UpdateAsync(oldProduct);
            return Ok(updatedProduct);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return this.BadRequest(ModelState);
            }

            var product = await this.productRepository.GetByIdAsync(id);
            if (product == null)
            {
                return this.NotFound();
            }

            await this.productRepository.DeleteAsync(product);
            return Ok(product);
        }




    }
}
