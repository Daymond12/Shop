using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shop.Web.Data;
using Shop.Web.Data.Repositories;
using Shop.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shop.Web.Controllers
{
    [Authorize]
    public class OrdersController : Controller
    {
        private readonly IOrderRepository orderRepository;
        private readonly IProductRepository productRepository;

        public OrdersController(IOrderRepository orderRepository, IProductRepository productsRepository)
        {
            this.orderRepository = orderRepository;
            this.productRepository = productsRepository;
        }


        public async Task<IActionResult> Index()
        {
            var model = await orderRepository.GetOrdersAsync(this.User.Identity.Name);
            return View(model);
        }

        public async Task<IActionResult> Create()
        {
            var model = await this.orderRepository.GetDetailTempsAsync(this.User.Identity.Name);
            return this.View(model);
        }

        //Agregar productos
        /// <summary>
        ///    Al adicionar un producto le pasamos
        ///    el addItenViewModel
        ///    se asume que la cantidad es uno
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        /// 
        public IActionResult AddProduct()
        {
            var model = new AddItemViewModel
            {
                Quantity = 1,
                //armamos la lista de producto
                //Este es el dromDopList
                Products = this.productRepository.GetComboProducts()
            };

            return View(model);
        }
        
        //Submit
        [HttpPost]
        public async Task<IActionResult> AddProduct(AddItemViewModel model)
        {
            //el modelo es válido?
            if (this.ModelState.IsValid)
            {
                //si es válido indica que es un valor positivo
                //agregamos al repositorio ese modelo, pero al usuario logueado
                await this.orderRepository.AddItemToOrderAsync(model, this.User.Identity.Name);
                return this.RedirectToAction("Create");
            }

            return this.View(model);
        }

        //metodo para el delete del OrderDetailTemp
        public async Task<IActionResult> DeleteItem(int? id)
        {
            if (id==null)
            {
                return NotFound();
            }
            await this.orderRepository.DeleteDetailTempAsync(id.Value);
            return this.RedirectToAction("Create");
            
        }

        //metodo para incrementar la cantidad
        public async Task<IActionResult>Increase(int? id)
        {
            if (id==null)
            {
                return NotFound();
            }

            await this.orderRepository.ModifyOrderDetailTempQuantityAsync(id.Value,1);
            return this.RedirectToAction("Create");
        }
        //metodo para decrementar la cantidad

        public async Task<IActionResult> Decrease(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            await this.orderRepository.ModifyOrderDetailTempQuantityAsync(id.Value, -1);
            return this.RedirectToAction("Create");
        }

        public async Task<IActionResult> ConfirmOrder()
        {
            //confirmar la orden del usuario logueado
            var response = await this.orderRepository.ConfirmOrderAsync(this.User.Identity.Name);
            if (response)
            {
                //si lo hace mandalo al index
                return this.RedirectToAction("Index");
            }
            //mandalo a la vista create
            return this.RedirectToAction("Create");
        }

        //despachar la orden
        public async Task<IActionResult> Deliver(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await this.orderRepository.GetOrdersAsync(id.Value);
            if (order == null)
            {
                return NotFound();
            }

            var model = new DeliverViewModel
            {
                Id = order.Id,
                DeliveryDate = DateTime.Today
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Deliver(DeliverViewModel model)
        {
            if (this.ModelState.IsValid)
            {
                await this.orderRepository.DeliverOrder(model);
                return this.RedirectToAction("Index");
            }

            return this.View();
        }



    }
}
