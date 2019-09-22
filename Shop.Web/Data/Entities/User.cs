
namespace Shop.Web.Data.Entities
{
    using Microsoft.AspNetCore.Identity;

    //IdentityUser son los usuarios del sistema
    public class User : IdentityUser
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

    }
}
