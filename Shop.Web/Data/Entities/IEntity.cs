using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shop.Web.Data.Entities
{
    public interface IEntity
    {
        /// <summary>
        /// A las interfaces también se le pueden meter atributos
        /// cualquier clase debe implementar el ID
        /// </summary>
        int Id { get; set; }


        
    }
}
