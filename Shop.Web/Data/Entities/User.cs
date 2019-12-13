
namespace Shop.Web.Data.Entities
{
    using Microsoft.AspNetCore.Identity;
    using System.ComponentModel.DataAnnotations;

    //IdentityUser son los usuarios del sistema
    public class User : IdentityUser
    {
        [MaxLength(100, ErrorMessage = "The field {0} only can contain {1} characters length.")]
        public string FirstName { get; set; }

        [MaxLength(100, ErrorMessage = "The field {0} only can contain {1} characters length.")]
        public string LastName { get; set; }

        [Display(Name="Full Name")]
        public string FullName { get { return $"{this.FirstName}{this.LastName}"; } }

        //Propiedades para tabajar con Countries and Cities
            //normalmente solo se pone City pero 
            /*debido a que no se puede acceder directamente al modelo user
             sino es por el userhelper, toca poner CityId, x q después no podemos recuperar esa ciudad*/
        public int CityId { get; set; }

        public City City { get; set; }

        [MaxLength(100, ErrorMessage = "The field {0} only can contain {1} characters length.")]
        public string Address { get; set; }
    }
}
