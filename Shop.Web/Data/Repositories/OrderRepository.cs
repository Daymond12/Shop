

using Microsoft.EntityFrameworkCore;
using Shop.Web.Data.Entities;
using Shop.Web.Helpers;
using Shop.Web.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Shop.Web.Data.Repositories
{
    public class OrderRepository : GenericRepository<Order>, IOrderRepository
    {
        private readonly DataContext context;
        private readonly IUserHelper userHelper;

        public OrderRepository(DataContext context, IUserHelper userHelper) : base(context)
        {
            this.context = context;
            this.userHelper = userHelper;
        }


        //te psao el codigo del usuario
        public async Task<IQueryable<Order>> GetOrdersAsync(string userName)
        {
            //lo busca en la BD
            var user = await this.userHelper.GetUserByEmailAsync(userName);
            if (user == null)
            {
                return null;
            }
            //si tiene el rol de administrador
            if (await this.userHelper.IsUserInRoleAsync(user, "Admin"))
            {
                //Include es equivalent a InnerJoin
                //dame todas las ordenes como admin
                return this.context.Orders
                    .Include(o => o.User)
                    .Include(o => o.Items)
                    .ThenInclude(i => i.Product)
                    .OrderByDescending(o => o.OrderDate);
            }

            // dame solo mis ordenes
            return this.context.Orders
                .Include(o => o.Items)
                .ThenInclude(i => i.Product)
                .Where(o => o.User == user)
                .OrderByDescending(o => o.OrderDate);
        }

        //
        public async Task<IQueryable<OrderDetailTemp>> GetDetailTempsAsync(string userName)
        {
            var user = await this.userHelper.GetUserByEmailAsync(userName);
            if (user == null)
            {
                return null;
            }


            //devolveme los detalles y sus productos
            return this.context.OrderDetailTemps
                .Include(o => o.Product)
                .Where(o => o.User == user)
                .OrderBy(o => o.Product.Name);
        }


        #region METODOS A NIVEL REPOSITORIO DE ORDENES
        //Add elemento a la orden
        //le pasamos el modelo y y el usuario que lo está adicionando
        public async Task AddItemToOrderAsync(AddItemViewModel model, string userName)
        {
            //vamos al helper de usuarios
            var user = await this.userHelper.GetUserByEmailAsync(userName);
            //ese usuario existe
            if (user == null)
            {
                // no existe !pa fuera
                return;
            }

            //buscamos el producto
            var product = await this.context.Products.FindAsync(model.ProductId);
            if (product == null)
            {
                // no existe !pa fuera
                return;
            }

            //si ese usuario y ese producto existen, buscamos si ya lo agregaron
            /*Es decir si hago pedido de un iphone y quiero agregar otro
             iphone no me agrega otra linea sino que pongo cantidad 2*/
            //si ese usuario ya pidió ese producto
            //si es de diferente usuario no hay problema
            //si es null es x que nunca lo ha pedido y lo adiciona
            var orderDetailTemp = await this.context.OrderDetailTemps
                .Where(odt => odt.User == user && odt.Product == product)
                .FirstOrDefaultAsync();
            if (orderDetailTemp == null)
            {
                orderDetailTemp = new OrderDetailTemp
                {
                    Price = product.Price,
                    Product = product,
                    Quantity = model.Quantity,
                    User = user,
                };

                this.context.OrderDetailTemps.Add(orderDetailTemp);
            }
            else
            //las siguintes lineas permiten no crear lineas dobles, si un mismo usuario--
            //ya ha agregado un mismo producto
            {
                orderDetailTemp.Quantity += model.Quantity;
                this.context.OrderDetailTemps.Update(orderDetailTemp);
            }

            await this.context.SaveChangesAsync();
        }


        //modificar la cantidad
        /*pasamos el id y la cantidad
         */
        public async Task ModifyOrderDetailTempQuantityAsync(int id, double quantity)
        {
            //lo busca
            var orderDetailTemp = await this.context.OrderDetailTemps.FindAsync(id);
            if (orderDetailTemp == null)
            {
                return;
            }
            //si lo encuentra incrementa la cantidad
            //siempre y cuando la cantidad sea mayor a cero, se inscribe en BD
            orderDetailTemp.Quantity += quantity;
            if (orderDetailTemp.Quantity > 0)
            {
                this.context.OrderDetailTemps.Update(orderDetailTemp);
                await this.context.SaveChangesAsync();
            }
        }
        #endregion

        public async Task DeleteDetailTempAsync(int id)
        {
            var orderDetailTemp = await this.context.OrderDetailTemps.FindAsync(id);
            if(orderDetailTemp == null)
            {
                return;
            }
            this.context.OrderDetailTemps.Remove(orderDetailTemp);
            await this.context.SaveChangesAsync();
           
        }

        public async Task<bool> ConfirmOrderAsync(string userName)
        {
            //buscamos el usuario
            var user = await this.userHelper.GetUserByEmailAsync(userName);
            if (user == null)
            {
                //es casi imposible que llegue nulo 
                //x q agregue productos, estoy logueado
                //debe sera una situacion extremadamente rara,pero es parte de la calidad de código
                return false;
            }
            //extraer mis registros temporales como usuarios
            //x q yo armé mi carrito de compras
            /*buscar los productos que haya en OrderDetailTemps
             pero hace un inner con la tabla product, y llamamos al método
             ToListAsync para hacer una lista*/

            var orderTmps = await this.context.OrderDetailTemps
                .Include(o => o.Product)
                .Where(o => o.User == user)
                .ToListAsync();

            //si eso es == 0, e.d no hay carrito de compras
            if (orderTmps == null || orderTmps.Count == 0)
            {
                return false;
            }
            //sino convertimos esa colección a OrderDetails


            //select coge una colección y la convierte en otra
            //x cada reg en detail crea un nuevo orderDetail
            var details = orderTmps.Select(o => new OrderDetail
            {
                Price = o.Price,
                Product = o.Product,
                Quantity = o.Quantity
            }).ToList();
            //utc now
            var order = new Order
            {
                OrderDate = DateTime.UtcNow,
                User = user,
                Items = details,
            };

            //grabá el carrito de compras
            this.context.Orders.Add(order);
            //borra el carrito de compras
            this.context.OrderDetailTemps.RemoveRange(orderTmps);
            await this.context.SaveChangesAsync();
            return true;

        }

        //le pasamos el modelo
        public async Task DeliverOrder(DeliverViewModel model)
        {
            //busca la orden en la base de datos
            var order = await this.context.Orders.FindAsync(model.Id);
            if (order == null)
            {
                // no la encontró !PaFuera
                return;
            }
            //si la encuentra, actualiza la fecha de despacho
            order.DeliveryDate = model.DeliveryDate;
            this.context.Orders.Update(order);
            await this.context.SaveChangesAsync();
        }

        //devolver la orden con ese id
        public async Task<Order> GetOrdersAsync(int id)
        {
            return await this.context.Orders.FindAsync(id);
        }

    }
}
    