using Shop.Web.Data.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

public class Order : IEntity
{
    public int Id { get; set; }

    //FECHA QUE SE CREA
    [Required]
    [Display(Name = "Order date")]
    [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd hh:mm tt}", ApplyFormatInEditMode = false)]
    public DateTime OrderDate { get; set; }

    //FECHA EN QUE SE DESPACHA, ES OPCIONAL XQ NO SE SABE CUANDO SE DESPACHARÁ
    [Display(Name = "Delivery date")]
    [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd hh:mm tt}", ApplyFormatInEditMode = false)]
    public DateTime? DeliveryDate { get; set; }

    //RELACIÓN CON USUARIO
    [Required]
    public User User { get; set; }

    //RELACIÓN CON ORDER DETAILS
    //UNA ORDEN PUEDE TERNER MUCHOS PRODUCTOS
    public IEnumerable<OrderDetail> Items { get; set; }

    //lineas es la cantidad de productos no repetidos en el pedido
    [DisplayFormat(DataFormatString = "{0:N0}")]
    public int Lines { get { return this.Items == null ? 0 : this.Items.Count(); } }


    [DisplayFormat(DataFormatString = "{0:N2}")]
    public double Quantity { get { return this.Items == null ? 0 : this.Items.Sum(i => i.Quantity); } }

    [DisplayFormat(DataFormatString = "{0:C2}")]
    public decimal Value { get { return this.Items == null ? 0 : this.Items.Sum(i => i.Value); } }

    [Display(Name = "Order date")]
    [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd hh:mm tt}", ApplyFormatInEditMode = false)]
    public DateTime? OrderDateLocal 
    {
        get
        { 
        if(this.OrderDate==null)
            {
                return null;
            }
            return this.OrderDate.ToLocalTime();
        }
        
    }
}

