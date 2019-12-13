using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Shop.Web.Data.Entities
{
    public class OrderDetailTemp : IEntity
    {
        public int Id { get; set; }

        //RELACIÓN ESTABLECIDA CON LA CLASE USER
        [Required]
        public User User { get; set; }
        //RELACIÓN ESTABLECIDA CON LA CLASE PRODUCT
        [Required]
        public Product Product { get; set; }


        //PRICE DEPENDE DEPENDE DEL PRODUCTO PERO
        //PRICE DEBE GUARDAR UN HISTÓRICO DE VENTAS //POR ESO 
        //PROCE TAMBIÉN VA ACÁ, PARA VER LA VENTA EN DEERMINADO MOMENTO
        [DisplayFormat(DataFormatString = "{0:C2}")]
        public decimal Price { get; set; }

        [DisplayFormat(DataFormatString = "{0:N2}")]
        public double Quantity { get; set; }

        [DisplayFormat(DataFormatString = "{0:C2}")]
        public decimal Value => this.Price * (decimal)this.Quantity;

        ///    

    }
}
