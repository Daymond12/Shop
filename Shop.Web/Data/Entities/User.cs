
namespace Shop.Web.Data.Entities
{
    using Microsoft.AspNetCore.Identity;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    //IdentityUser son los usuarios del sistema
    public class User : IdentityUser
    {
        [Display(Name = "First Name")]
        [MaxLength(100, ErrorMessage = "The field {0} only can contain {1} characters length.")]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")] 
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

        [Display(Name = "Phone Number")]
        public override string PhoneNumber { get => base.PhoneNumber; set => base.PhoneNumber = value; }


        [MaxLength(100, ErrorMessage = "The field {0} only can contain {1} characters length.")]
        public string Address { get; set; }

        [Display(Name = "Email Confirmed")]
        public override bool EmailConfirmed { get => base.EmailConfirmed; set => base.EmailConfirmed = value; }

        [NotMapped]
        [Display(Name = "Is Admin?")]
        public bool IsAdmin { get; set; }

    }
}
