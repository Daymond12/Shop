using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shop.Web.Data
{
   public interface  IGenericRepository<T> where T : class
    {
        //T ES CUALQUIE COSA: PRODUCTO, CUSTOMER, PROVIDER
        IQueryable<T> GetAll(); //devuelve una lista de T(cualquier cosa)

        Task<T> GetByIdAsync(int id);//devuelve un T(cualquier cosa)

        Task CreateAsync(T entity);//adiciona un T(cualquier cosa), etc, etc

        Task UpdateAsync(T entity);

        Task DeleteAsync(T entity);

        Task<bool> ExistAsync(int id);

    }
}
