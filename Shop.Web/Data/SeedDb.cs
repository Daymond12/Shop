
namespace Shop.Web.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Entities;
    using Microsoft.AspNetCore.Identity;
    using Shop.Web.Helpers;

    public class SeedDb
    {
        private readonly DataContext context;
        private readonly IUserHelper userHelper;

        //private readonly UserManager<User> userManager;
        private Random random;
        //El SeedDb es para almacenar datos de prueba

        //1-se le inyecta la conexión a la base datos
        //2-UserManager<User> userManager, esa inyección viene embeida en el core
        //cambiamos la inyección del UserManager x el UserHelper
        public SeedDb(DataContext context,IUserHelper userHelper /*UserManager<User> userManager*/)
        {
            this.context = context;
            this.userHelper = userHelper;
            // this.userManager = userManager;
            this.random = new Random();
        }

        //2-método para alimentar la BD
        public async Task SeedAsync()
        {
            await this.context.Database.EnsureCreatedAsync();

            /// verifica si en la userHelper existe el Rol Admin y Customer

            await this.userHelper.CheckRoleAsync("Admin");
            await this.userHelper.CheckRoleAsync("Customer");

            //agregar algo en países

            //pregunta si nuestra colección de países tiene alguno
            if (!this.context.Countries.Any())
            {
                //si no hay agrega una lista de ciudades
                //los Id no los agrego, son autonuméricos
                var cities = new List<City>();
                cities.Add(new City { Name = "Carazo" });
                cities.Add(new City { Name = "Masaya" });
                cities.Add(new City { Name = "Managua" });
                //luego crea un pais, llamado colombia con esas tres ciudades
                this.context.Countries.Add(new Country
                {
                    Cities = cities,
                    Name = "Nicaragua"
                });

                await this.context.SaveChangesAsync();
            }



            //Crear un usario con el método de userManager llamado FindByEmailAsync
            // var user = await this.userManager.FindByEmailAsync("drogermoises@gmail.com");
            var user = await this.userHelper.GetUserByEmailAsync("drogermoises@gmail.com");
            if (user == null)
            {
                user = new User
                {
                    //Datos extendidos
                    FirstName = "Roger",
                    LastName = "Davila",
                    //Datos añadidos
                    Email = "drogermoises@gmail.com",
                    UserName = "drogermoises@gmail.com",
                    PhoneNumber="81806119",
                    Address = "Barrio la Competencia",
                    CityId = this.context.Countries.FirstOrDefault().Cities.FirstOrDefault().Id,
                    City = this.context.Countries.FirstOrDefault().Cities.FirstOrDefault()

                };

               // var result = await this.userManager.CreateAsync(user, "123456");
                var result = await this.userHelper.AddUserAsync(user, "123456");
                if (result != IdentityResult.Success)
                {
                    throw new InvalidOperationException("Could not create the user in seeder");
                }
                await this.userHelper.AddUserToRoleAsync(user, "Admin");
            }

            var isInRole = await this.userHelper.IsUserInRoleAsync(user, "Admin");
            if (!isInRole)
            {
                await this.userHelper.AddUserToRoleAsync(user, "Admin");
            }



            if (!this.context.Products.Any())
            {
                //Any, si al menos hay un registro regresa true
                this.AddProduct("iphone X",user);
                this.AddProduct("Magic Mouse", user);
                this.AddProduct("iWatch Series 4", user);
                await this.context.SaveChangesAsync();
            }
        }



        //los productos llevan un usuario por eso pasamos user como parámeto
        private void AddProduct(string name, User user)
        {
            this.context.Products.Add(new Product
            {
                Name = name,
                Price = this.random.Next(100),
                IsAvailabe = true,
                Stock = this.random.Next(100),
                User=user
            });
        }

    }
}
