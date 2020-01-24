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

        //adiciona un T(cualquier cosa), etc, etc
        // Task<T> e.d euq devuelve el moedlo tal cual fué mapeado en la BD

        Task<T> CreateAsync(T entity);

        Task<T> UpdateAsync(T entity);

        Task DeleteAsync(T entity);

        Task<bool> ExistAsync(int id);

    }
}
