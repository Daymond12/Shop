

namespace Shop.Web.Helpers
{
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;
    using Shop.Web.Data.Entities;
    using Shop.Web.Models;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    public class UserHelper : IUserHelper
    {
        private readonly UserManager<User> userManager;
        private readonly SignInManager<User> singInManager;
        private readonly RoleManager<IdentityRole> roleManager;

        //Inyectamos el userManagaer en el contructor

        public UserHelper(UserManager<User> userManager, 
            SignInManager<User> singInManager,
            RoleManager<IdentityRole> roleManager)
        {
            this.userManager = userManager;
            //El singInManager es para login y logout
            this.singInManager = singInManager;
            //El roleManager es para los roles
            this.roleManager = roleManager;
        }

        public async Task<IdentityResult> AddUserAsync(User user, string password)
        {
            return await this.userManager.CreateAsync(user, password);
        }

       

        //User es mi clase extendida del UserIdentity
        public async Task<User> GetUserByEmailAsync(string email)
        {
            return await this.userManager.FindByEmailAsync(email);

        }

        //singInManager para login
        public async Task<SignInResult> LoginAsync(LoginViewModel model)
        {
            return await this.singInManager.PasswordSignInAsync(
        model.Username,
        model.Password,
        model.RememberMe,   
        false);

        }

        public async Task LogoutAsync()
        {
            await this.singInManager.SignOutAsync();
        }

        public async Task<IdentityResult> UpdateUserAsync(User user)
        {
            return await this.userManager.UpdateAsync(user);
        }

        public async Task<IdentityResult> ChangePasswordAsync(User user, string oldPassword, string newPassword)
        {
            return await this.userManager.ChangePasswordAsync(user, oldPassword, newPassword);
        }

        public async Task<SignInResult> ValidatePasswordAsync(User user, string password)
        {
            return await this.singInManager.CheckPasswordSignInAsync(
                user,
                password,
                //false es para el contador de intentos fallidos para bloquear el usuario
                false);
        }

        public async Task CheckRoleAsync(string roleName)
        {
            var roleExists = await this.roleManager.RoleExistsAsync(roleName);
            if (!roleExists)
            {
                await this.roleManager.CreateAsync(new IdentityRole
                {
                    Name = roleName
                });
            }

        }

        public async Task AddUserToRoleAsync(User user, string roleName)
        {
            await this.userManager.AddToRoleAsync(user, roleName);
        }

        public async Task<bool> IsUserInRoleAsync(User user, string roleName)
        {
            //buscar si el usuaio pertenece a ese rol
         return await  this.userManager.IsInRoleAsync(user, roleName);
        }

        #region MÉTODOS PARA USO DEL CORREO
        public async Task<string> GenerateEmailConfirmationTokenAsync(User user)
        {
            return await this.userManager.GenerateEmailConfirmationTokenAsync(user);
        }

        public async Task<IdentityResult> ConfirmEmailAsync(User user, string token)
        {
            return await this.userManager.ConfirmEmailAsync(user, token);
        }

        public async Task<User> GetUserByIdAsync(string userId)
        {
            return await this.userManager.FindByIdAsync(userId);
        }
        #endregion


        public async Task<string> GeneratePasswordResetTokenAsync(User user)
        {
            return await this.userManager.GeneratePasswordResetTokenAsync(user);
        }

        public async Task<IdentityResult> ResetPasswordAsync(User user, string token, string password)
        {
            return await this.userManager.ResetPasswordAsync(user, token, password);
        }

        #region MÉTODOS PARA EL ADMINISTRADOR DE USUARIOS
        public async Task<List<User>> GetAllUsersAsync()
        {
            return await this.userManager.Users
                .Include(u => u.City)
                .OrderBy(u => u.FirstName)
                .ThenBy(u => u.LastName)
                .ToListAsync();
        }

        public async Task RemoveUserFromRoleAsync(User user, string roleName)
        {
            await this.userManager.RemoveFromRoleAsync(user, roleName);
        }

        public async Task DeleteUserAsync(User user)
        {
            await this.userManager.DeleteAsync(user);
        }

        #endregion


    }
}
