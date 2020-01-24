
namespace Shop.Web.Helpers
{

    using Microsoft.AspNetCore.Identity;
    using Shop.Web.Data.Entities;
    using Shop.Web.Models;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IUserHelper
    {
        ///ESTA CLASE NOS AYUDARÁ A EVITAR ESTAR INSTANCIADNO
        ///EL UserManager

        Task<User> GetUserByEmailAsync(string email);

        // le mandamos el usuario y la contraseña y retorna un IdentityResult
        //si pudi o no pudi, sino pudo x que no pudo
        Task<IdentityResult> AddUserAsync(User user, string password);

        Task<SignInResult> LoginAsync(LoginViewModel model);

        Task LogoutAsync();

        Task<IdentityResult> UpdateUserAsync(User user);

        Task<IdentityResult> ChangePasswordAsync(User user, string oldPassword, string newPassword);

        Task<SignInResult> ValidatePasswordAsync(User user, string password);
        //
        Task CheckRoleAsync(string roleName);

        //METODO PARA ADICIONAR ROL A UN USUARIO
        Task AddUserToRoleAsync(User user, string roleName);

        Task<bool> IsUserInRoleAsync(User user, string roleName);


        //generar token de confirmación
        //le pasamos el user y nos devuelve un string
        Task<string> GenerateEmailConfirmationTokenAsync(User user);

        //Confirmar token
        //recibe usario y token
        Task<IdentityResult> ConfirmEmailAsync(User user, string token);

        Task<User> GetUserByIdAsync(string userId);


        #region MÉTODOS PARA LA RECUPERACIÓN DEL PASSWORD
        Task<string> GeneratePasswordResetTokenAsync(User user);

        //mandamos el usuario, mandamos el token y el nuevo password
        Task<IdentityResult> ResetPasswordAsync(User user, string token, string password);

        #endregion



        #region MÉTODOS_PARA_EL ADMINISTRADOR DE USUARIOS

        //GetAllUsersAsync DEVUELVE TODOS LOS USUARIOS
        Task<List<User>> GetAllUsersAsync();

        //METODO PARA REMOVER ROL A USUARIO
        Task RemoveUserFromRoleAsync(User user, string roleName);

        //MEODO PARA BORRAR UN USUARIO
        Task DeleteUserAsync(User user);

        #endregion

    }
}

