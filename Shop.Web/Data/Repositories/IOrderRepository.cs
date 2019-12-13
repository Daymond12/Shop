

namespace Shop.Web.Data.Repositories
{
    using Shop.Web.Data.Entities;
    using Shop.Web.Models;
    using System.Linq;
    using System.Threading.Tasks;

    public interface IOrderRepository : IGenericRepository<Order>
    {
        Task<IQueryable<Order>> GetOrdersAsync(string userName);

        Task<IQueryable<OrderDetailTemp>> GetDetailTempsAsync(string userName);

        Task AddItemToOrderAsync(AddItemViewModel model, string userName);

        Task ModifyOrderDetailTempQuantityAsync(int id, double quantity);

        //Borrar la Orden temporal
        Task DeleteDetailTempAsync(int id);

        //confirmar ordenes
        Task<bool> ConfirmOrderAsync(string userName);

        //Entrega de Orden
        Task DeliverOrder(DeliverViewModel model);

        //Obtener la orden
        Task<Order> GetOrdersAsync(int id);





    }
}
