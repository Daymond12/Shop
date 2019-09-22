
namespace Shop.Web.Helpers
{

    using Microsoft.AspNetCore.Identity;
    using Shop.Web.Data.Entities;
    using System.Threading.Tasks;

    public interface IUserHelper
    {
        ///ESTA CLASE NOS AYUDARÁ A EVITAR ESTAR INSTANCIADNO
        ///EL UserManager
   
        Task<User> GetUserByEmailAsync(string email);

        // le mandamos el usuario y la contraseña y retorna un IdentityResult
        //si pudi o no pudi, sino pudo x que no pudo
        Task<IdentityResult> AddUserAsync(User user, string password);

    }
}

