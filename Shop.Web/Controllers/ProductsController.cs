
namespace Shop.Web.Controllers
{
    using Data;
    using Data.Entities;
    using Helpers;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using System.Threading.Tasks;

    public class ProductsController : Controller
    {
        private readonly IProductRepository productRepository;

        private readonly IUserHelper userHelper;

        public ProductsController(IProductRepository productRepository, IUserHelper userHelper)
        {
            this.productRepository = productRepository;
            this.userHelper = userHelper;
        }

        // GET: Products
        public IActionResult Index()
        {
            return View(this.productRepository.GetAll());
        }

        // GET: Products/Details/5
        public async Task<IActionResult> Details(int? id)
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
                // TODO: Pending to change to: this.User.Identity.Name
                product.User = await this.userHelper.GetUserByEmailAsync("jzuluaga55@gmail.com");
                await this.productRepository.CreateAsync(product);
                return RedirectToAction(nameof(Index));
            }

            return View(product);
        }

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

            return View(product);
        }

        // POST: Products/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Product product)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // TODO: Pending to change to: this.User.Identity.Name
                    product.User = await this.userHelper.GetUserByEmailAsync("jzuluaga55@gmail.com");
                    await this.productRepository.UpdateAsync(product);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await this.productRepository.ExistAsync(product.Id))
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

