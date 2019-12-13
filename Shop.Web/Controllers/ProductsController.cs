
namespace Shop.Web.Controllers
{
    using Data;
    using Data.Entities;
    using Helpers;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using Shop.Web.Models;
    using System;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;

    
    public class ProductsController : Controller
    {
        private readonly IProductRepository productRepository;

        private readonly IUserHelper UserHelper;

        public ProductsController(IProductRepository productRepository, IUserHelper userHelper)
        {
            this.productRepository = productRepository;
            this.UserHelper = userHelper;
        }

        // GET: Products
        public IActionResult Index()
        {
            return View(this.productRepository.GetAll().OrderBy(p => p.Name));
        }

        // GET: Products/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new NotFoundViewResult("ProductNotFound");
            }

            var product = await this.productRepository.GetByIdAsync(id.Value);
            if (product == null)
            {
                return new NotFoundViewResult("ProductNotFound");
            }

            return View(product);
        }
        [Authorize(Roles = "Admin")]
        // GET: Products/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Products/Create
        [HttpPost]
        public async Task<IActionResult> Create(ProductViewModel view)
        {
           
            if (ModelState.IsValid)
            {
                


                // #region RUTA_PARA_GUARDAR_IMG_EN EL SERVIDOR
                var path = string.Empty;
                //si el imageFile es diferente de null
                //y si .lengt(tamaño físico) es mayor a cero
                if (view.ImageFile != null && view.ImageFile.Length > 0)
                {
                    //guid hace que n repita es un viajao de numeros
                    var guid = Guid.NewGuid().ToString();
                    var file = $"{guid}.jpg";
                    path = Path.Combine(
                        Directory.GetCurrentDirectory(),
                        "wwwroot\\images\\Products",
                        file);


                    // SUBIR LA IMG AL SERVIDOR
                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        await view.ImageFile.CopyToAsync(stream);
                    }
                   // #endregion


                    path = $"~/images/Products/{file}";
                }

               
               
                view.User = await this.UserHelper.GetUserByEmailAsync(this.User.Identity.Name);
                //cambiar de productViewModel a Product
                var product = this.ToProduct(view, path);
                await this.productRepository.CreateAsync(product);
                return RedirectToAction(nameof(Index));
            }

            //si falla lo devolvemos a la vista1
            return View(view);
        }
        // En el post convertimos el productViewModel a  Product
        private Product ToProduct(ProductViewModel view, string path)
        {
            return new Product
            {
                Id = view.Id,
                ImageUrl = path,
                IsAvailabe = view.IsAvailabe,
                LastPurchase = view.LastPurchase,
                LastSale = view.LastSale,
                Name = view.Name,
                Price = view.Price,
                Stock = view.Stock,
                User = view.User

            };
        }

        [Authorize(Roles = "Admin")]
        // GET: Products/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await this.productRepository.GetByIdAsync(id.Value);
            if (product == null)
            {
                return NotFound();
            }

            var view = this.ToProductViewModel(product);

            return View(view);
        }
        //le pasamos el producto que vamos a editar
        //por lo tanto convertimos el Product a productViewModel
        private ProductViewModel ToProductViewModel(Product product)
        {
            return new ProductViewModel
            {
                Id = product.Id,
                IsAvailabe = product.IsAvailabe,
                LastPurchase = product.LastPurchase,
                LastSale = product.LastSale,
                ImageUrl = product.ImageUrl,
                Name = product.Name,
                Price = product.Price,
                Stock = product.Stock,
                User = product.User
            };
        }



        // POST: Products/Edit/5
        //Aquí recibe un productViewModel
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ProductViewModel view)
        {
            if (ModelState.IsValid)
            {
                try
                {

               
                // #region RUTA_PARA_GUARDAR_IMG_EN EL SERVIDOR
                var path = view.ImageUrl;
                //si el imageFile es diferente de null
                //y si .lengt(tamaño físico) es mayor a cero
                if (view.ImageFile != null && view.ImageFile.Length > 0)
                {
                    //guid hace que n repita es un viajao de numeros
                    var guid = Guid.NewGuid().ToString();
                    var file = $"{guid}.jpg";
                    path = Path.Combine(
                        Directory.GetCurrentDirectory(),
                        "wwwroot\\images\\Products",
                        file);


                    // SUBIR LA IMG AL SERVIDOR
                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        await view.ImageFile.CopyToAsync(stream);
                    }
                    // #endregion


                    path = $"~/images/Products/{file}";
                }


      
               view.User = await this.UserHelper.GetUserByEmailAsync(this.User.Identity.Name);
                //cambiar de productViewModel a Product
                var product = this.ToProduct(view, path);
                await this.productRepository.UpdateAsync(product);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if(!await this.productRepository.ExistAsync(view.Id))
                    {
                        return NotFound();
                    }
                    else
                    { throw;
                    }
                   
                }
                return RedirectToAction(nameof(Index));
            }

            return View(view);
        }

        [Authorize(Roles = "Admin")]
        // GET: Products/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await this.productRepository.GetByIdAsync(id.Value);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await this.productRepository.GetByIdAsync(id);
            await this.productRepository.DeleteAsync(product);
            return RedirectToAction(nameof(Index));
        }

        public IActionResult ProductNotFound()
        {
            return this.View();
        }

        



    }
}

#region ASI ESTABA EL CÓDIGO ANTES DEL GENERICREPOSITORY EN EL CONTROLADOR
/* using System.Threading.Tasks;
using Data;
using Data.Entities;
using Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


public class ProductsController : Controller
{

private readonly IProductRepository productRepository;
private readonly IUserHelper userHelper;

#region MODIFICACIÓN DEL DATA CONTEXT

/// <summary>
/// Se ha dejado en pelotas el constructor, ya que hacía una inyección que 
/// lo vinculaba directaente a la BD
/// </summary>
/// <returns></returns>
// private readonly DataContext _context;

//Esta inyeccion viene desde el D:\Projects\Shop\Shop.Web\Startup.cs
//public ProductsController(DataContext context)
//{
//    _context = context;
//}
#endregion

public ProductsController(IProductRepository productRepository, IUserHelper userHelper)
{
   this.productRepository = productRepository;
   this.userHelper = userHelper;
}

// GET: Products
public IActionResult Index()
{
   return View(this.repository.GetProducts());
}

// GET: Products/Details/5
public IActionResult Details(int? id)
{
   if (id == null)
   {
       return NotFound();
   }

   var product = this.repository.GetProduct(id.Value);

   if (product == null)
   {
       return NotFound();
   }

   return View(product);
}

// GET: Products/Create
public IActionResult Create()
{
   return View();
}

// POST: Products/Create
[HttpPost]
[ValidateAntiForgeryToken]
public async Task<IActionResult> Create(Product product)
{
   if (ModelState.IsValid)
   {
       //TODO:CHANGE FOR THE LOGGED USER
       product.User = await this.userHelper.GetUserByEmailAsync("drogermoises@gmail.com");
       this.repository.AddProduct(product);
       await this.repository.SaveAllAsync();
       return RedirectToAction(nameof(Index));
   }
   return View(product);
}

// GET: Products/Edit/5
public IActionResult Edit(int? id)
{

   if (id == null)
   {
       return NotFound();
   }

   var product = this.repository.GetProduct(id.Value);
   if (product == null)
   {
       return NotFound();
   }
   return View(product);
}

// POST: Products/Edit/5
[HttpPost]
[ValidateAntiForgeryToken]
public async Task<IActionResult> Edit(int id, Product product)
{
   //TODO:CHANGE FOR THE LOGGED USER
   product.User = await this.userHelper.GetUserByEmailAsync("drogermoises@gmail.com");
   ///la siguiente validación solo sirve para el api
   ///por la comentamos
   //if (id != product.Id)
   //{
   //    return NotFound();
   //}

   if (ModelState.IsValid)
   {
       try
       {
           ///si el modelo es valido
           ///hacentamos los cambios en la bD
           this.repository.UpdateProduct(product);
           await this.repository.SaveAllAsync();
       }
       catch (DbUpdateConcurrencyException)
       {
           if (!this.repository.ProductExists(product.Id))
           {
               return NotFound();
           }
           else
           {
               throw;
           }
       }
       return RedirectToAction(nameof(Index));
   }
   return View(product);
}

// GET: Products/Delete/5
public IActionResult Delete(int? id)
{
   if (id == null)
   {
       return NotFound();
   }

   var product = this.repository.GetProduct(id.Value);

   if (product == null)
   {
       return NotFound();
   }

   return View(product);
}

// POST: Products/Delete/5
[HttpPost, ActionName("Delete")]
[ValidateAntiForgeryToken]
public async Task<IActionResult> DeleteConfirmed(int id)
{
   var product = this.repository.GetProduct(id);
   this.repository.RemoveProduct(product);
   await this.repository.SaveAllAsync();
   return RedirectToAction(nameof(Index));
}

#region MÉTODO_SIRVE POR QUE ES DE CONEXIÓN DIRECTA
#region SE HACE MEJOR POR REPOSITORIO
//private bool ProductExists(int id)
//{
//    return _context.Products.Any(e => e.Id == id);
//}
#endregion
#endregion

}*/
#endregion

