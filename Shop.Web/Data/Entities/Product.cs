
namespace Shop.Web.Data.Entities
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class Product : IEntity
    {
        public int Id { get; set; }

        [MaxLength(50, ErrorMessage = "This Field only can contain {1} characters length.")]//Esta notación cambia la estructura de la base de datos
        [Required]
        public string Name { get; set; }

        [DisplayFormat(DataFormatString = "{0:C2}", ApplyFormatInEditMode = false)]
        //DisplayFormat no cambia la BD, es solo visual
        public decimal Price { get; set; }


        [Display(Name = "Image")]
        public string ImageUrl { get; set; }

        [Display(Name = "Last Purchase")]
        public DateTime? LastPurchase { get; set; }
        //Los DateTime por defecto son NotNull
        //El ? de C# me permite que sean campos que acepten valores nulos

        [Display(Name = "Last Sale")]
        public DateTime? LastSale { get; set; }

        [Display(Name = "Is Availabe?")]
        public bool IsAvailabe { get; set; }

        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = false)]
        public double Stock { get; set; }
        //relación uno a varios, product es el lado varios
        //solo acá se hace cambio para hacer la relación
        public User User { get; set; }

        //Ls propiedades de lectura no modifican la base de datos
        //por lo que puedo hacer tantas yo quiera
        //y no migrar la base de datos
        public string ImageFullPath {
            get
            {

                if(string.IsNullOrEmpty(this.ImageUrl))
                {
                    return null;
                }
                return $"https://shopnevin.azurewebsites.net{this.ImageUrl.Substring(1)}";
            }



        }
    }

}
