

namespace Shop.Web.Helpers
{
    using Microsoft.AspNetCore.Identity;
    using Shop.Web.Data.Entities;
    using Shop.Web.Models;
    using System;
    using System.Threading.Tasks;
    public class UserHelper : IUserHelper
    {
        private readonly UserManager<User> userManager;
        private readonly SignInManager<User> singInManager;

        //Inyectamos el usar managaer en el contructor

        public UserHelper(UserManager<User> userManager, 
            SignInManager<User> singInManager)
        {
            this.userManager = userManager;
            //El singInManager es para login y logout
            this.singInManager = singInManager;
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
    }
}
