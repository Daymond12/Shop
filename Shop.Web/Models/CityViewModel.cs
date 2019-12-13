

namespace Shop.Web.Models
{
    using System.ComponentModel.DataAnnotations;
    public class CityViewModel
    {
        /*Cuándo creamos una ciudd decimos, de que país es?*/
        public int CountryId { get; set; }

        //cual es código de la ciudad?
        public int CityId { get; set; }

        //El nombre la ciudad
        [Required]
        [Display(Name = "City")]
        [MaxLength(50, ErrorMessage = "The field {0} only can contain {1} characters length.")]
        public string Name { get; set; }

    }
}
