using System;
using System.Linq;
using System.Threading.Tasks;
using Shop.Web.Data.Entities;
using Shop.Web.Helpers;
using Microsoft.AspNetCore.Identity;
using Shop.Web.Data;

public class SeedDb
{
    private readonly DataContext context;
    private readonly IUserHelper userHelper;
    private readonly Random random;

    public SeedDb(DataContext context, IUserHelper userHelper)
    {
        this.context = context;
        this.userHelper = userHelper;
        this.random = new Random();
    }

    public async Task SeedAsync()
    {
        //verifica que la BD esté creada
        /*porque si yo borro la base datos y corro de nuevo él la crea
         EF tiene el poder de crear la BD si no la encuentra, siempre y cuando no sea
         la de azure, por la de azure requiere permisos*/
        await this.context.Database.EnsureCreatedAsync();

        //chequea Roles
        await this.CheckRoles();
        //verifica si hay paises y ciudades
        if (!this.context.Countries.Any())
        {
            await this.AddCountriesAndCitiesAsync();
        }

        //Yo puedo llamar a un metodo e ignorar lo que devuelve
        //ejemplo chekUser devuelve user pero lo puedo llamar como void->ignoro lo que me manda

        await this.CheckUserAsync("brad@gmail.com", "Brad", "Pit", "Customer");
        await this.CheckUserAsync("angelina@gmail.com", "Angelina", "Jolie", "Customer");
        //acá si le adigno
        /*lo que devulve, al menos para ligar a un ususario los productos*/
        var user = await this.CheckUserAsync("drogermoises@gmail.com", "Róger", "Dávila", "Admin");

        // Add products
        if (!this.context.Products.Any())
        {
            this.AddProduct("AirPods", 159, user);
            this.AddProduct("Blackmagic eGPU Pro", 1199, user);
            this.AddProduct("iPad Pro", 799, user);
            this.AddProduct("iMac", 1398, user);
            this.AddProduct("iPhone X", 749, user);
            this.AddProduct("iWatch Series 4", 399, user);
            this.AddProduct("Mac Book Air", 789, user);
            this.AddProduct("Mac Book Pro", 1299, user);
            this.AddProduct("Mac Mini", 708, user);
            this.AddProduct("Mac Pro", 2300, user);
            this.AddProduct("Magic Mouse", 47, user);
            this.AddProduct("Magic Trackpad 2", 140, user);
            this.AddProduct("USB C Multiport", 145, user);
            this.AddProduct("Wireless Charging Pad", 67.67M, user);
            await this.context.SaveChangesAsync();
        }
    }

    private async Task<User> CheckUserAsync(string userName, string firstName, string lastName, string role)
    {
        // Add user
        var user = await this.userHelper.GetUserByEmailAsync(userName);
        //si el user es nulo lo crea
        if (user == null)
        {
            user = await this.AddUser(userName, firstName, lastName, role);
        }

        //verifica si pertenece al rol/si no pertence lo asigna
        var isInRole = await this.userHelper.IsUserInRoleAsync(user, role);
        if (!isInRole)
        {
            await this.userHelper.AddUserToRoleAsync(user, role);
        }

        return user;
    }

    //Adduser crea el usuario ya con los campos mandados
    //y aparte de eso
    private async Task<User> AddUser(string userName, string firstName, string lastName, string role)
    {

        var user = new User
        {
        FirstName = firstName,
            LastName = lastName,
            Email = userName,
            UserName = userName,
            Address = "Calle Luna Calle Sol",
            PhoneNumber =  "350 634 2747",
            CityId = this.context.Countries.FirstOrDefault().Cities.FirstOrDefault().Id,
            City = this.context.Countries.FirstOrDefault().Cities.FirstOrDefault()
        };
      
        var result = await this.userHelper.AddUserAsync(user, "123456");
        if (result != IdentityResult.Success)
        {
            throw new InvalidOperationException("Could not create the user in seeder");
        }

        await this.userHelper.AddUserToRoleAsync(user, role);
        var token = await this.userHelper.GenerateEmailConfirmationTokenAsync(user);
        await this.userHelper.ConfirmEmailAsync(user, token);
        return user;
    }

    private async Task AddCountriesAndCitiesAsync()
    {
        this.AddCountry("Colombia", new string[] { "Medellín", "Bogota", "Calí", "Barranquilla", "Bucaramanga", "Cartagena", "Pereira" });
        this.AddCountry("Argentina", new string[] { "Córdoba", "Buenos Aires", "Rosario", "Tandil", "Salta", "Mendoza" });
        this.AddCountry("Estados Unidos", new string[] { "New York", "Los Ángeles", "Chicago", "Washington", "San Francisco", "Miami", "Boston" });
        this.AddCountry("Ecuador", new string[] { "Quito", "Guayaquil", "Ambato", "Manta", "Loja", "Santo" });
        this.AddCountry("Peru", new string[] { "Lima", "Arequipa", "Cusco", "Trujillo", "Chiclayo", "Iquitos" });
        this.AddCountry("Chile", new string[] { "Santiago", "Valdivia", "Concepcion", "Puerto Montt", "Temucos", "La Sirena" });
        this.AddCountry("Uruguay", new string[] { "Montevideo", "Punta del Este", "Colonia del Sacramento", "Las Piedras" });
        this.AddCountry("Bolivia", new string[] { "La Paz", "Sucre", "Potosi", "Cochabamba" });
        this.AddCountry("Venezuela", new string[] { "Caracas", "Valencia", "Maracaibo", "Ciudad Bolivar", "Maracay", "Barquisimeto" });
        this.AddCountry("Paraguay", new string[] { "Asunción", "Ciudad del Este", "Encarnación", "San  Lorenzo", "Luque", "Areguá" });
        this.AddCountry("Brasil", new string[] { "Rio de Janeiro", "São Paulo", "Salvador", "Porto Alegre", "Curitiba", "Recife", "Belo Horizonte", "Fortaleza" });
        this.AddCountry("Panamá", new string[] { "Chitré", "Santiago", "La Arena", "Agua Dulce", "Monagrillo", "Ciudad de Panamá", "Colón", "Los Santos" });
        this.AddCountry("México", new string[] { "Guadalajara", "Ciudad de México", "Monterrey", "Ciudad Obregón", "Hermosillo", "La Paz", "Culiacán", "Los Mochis" });
        await this.context.SaveChangesAsync();
    }

    private void AddCountry(string country, string[] cities)
    {
        var theCities = cities.Select(c => new City { Name = c }).ToList();
        this.context.Countries.Add(new Country
        {
            Cities = theCities,
            Name = country
        });
    }

    private async Task CheckRoles()
    {
        await this.userHelper.CheckRoleAsync("Admin");
        await this.userHelper.CheckRoleAsync("Customer");
    }

    private void AddProduct(string name, decimal price, User user)
    {
        this.context.Products.Add(new Product
        {
            Name = name,
            Price = price,
            IsAvailabe = true,
            Stock = this.random.Next(100),
            User = user,
            ImageUrl = $"~/images/Products/{name}.png"
        });
    }
}



























#region CÓDIGO ANTERIOR
///namespace Shop.Web.Data
//{
//    using System;
//    using System.Collections.Generic;
//    using System.Linq;
//    using System.Threading.Tasks;
//    using Entities;
//    using Microsoft.AspNetCore.Identity;
//    using Shop.Web.Helpers;

//    public class SeedDb
//    {
//        private readonly DataContext context;
//        private readonly IUserHelper userHelper;

//        //private readonly UserManager<User> userManager;
//        private Random random;
//        //El SeedDb es para almacenar datos de prueba

//        //1-se le inyecta la conexión a la base datos
//        //2-UserManager<User> userManager, esa inyección viene embeida en el core
//        //cambiamos la inyección del UserManager x el UserHelper
//        public SeedDb(DataContext context,IUserHelper userHelper /*UserManager<User> userManager*/)
//        {
//            this.context = context;
//            this.userHelper = userHelper;
//            // this.userManager = userManager;
//            this.random = new Random();
//        }

////2-método para alimentar la BD
//public async Task SeedAsync()
//{
//    await this.context.Database.EnsureCreatedAsync();

//    /// verifica si en la userHelper existe el Rol Admin y Customer

//    await this.userHelper.CheckRoleAsync("Admin");
//    await this.userHelper.CheckRoleAsync("Customer");

//    //agregar algo en países

//    //pregunta si nuestra colección de países tiene alguno
//    if (!this.context.Countries.Any())
//    {
//        //si no hay agrega una lista de ciudades
//        //los Id no los agrego, son autonuméricos
//        var cities = new List<City>();
//        cities.Add(new City { Name = "Carazo" });
//        cities.Add(new City { Name = "Masaya" });
//        cities.Add(new City { Name = "Managua" });
//        //luego crea un pais, llamado colombia con esas tres ciudades
//        this.context.Countries.Add(new Country
//        {
//            Cities = cities,
//            Name = "Nicaragua"
//        });

//        await this.context.SaveChangesAsync();
//    }



//    //Crear un usario con el método de userManager llamado FindByEmailAsync
//    // var user = await this.userManager.FindByEmailAsync("drogermoises@gmail.com");
//    var user = await this.userHelper.GetUserByEmailAsync("drogermoises@gmail.com");
//    if (user == null)
//    {
//        user = new User
//        {
//            //Datos extendidos
//            FirstName = "Roger",
//            LastName = "Davila",
//            //Datos añadidos
//            Email = "drogermoises@gmail.com",
//            UserName = "drogermoises@gmail.com",
//            PhoneNumber = "81806119",
//            Address = "Barrio la Competencia",
//            CityId = this.context.Countries.FirstOrDefault().Cities.FirstOrDefault().Id,
//            City = this.context.Countries.FirstOrDefault().Cities.FirstOrDefault()

//        };

//        // var result = await this.userManager.CreateAsync(user, "123456");
//        var result = await this.userHelper.AddUserAsync(user, "123456");
//        if (result != IdentityResult.Success)
//        {
//            throw new InvalidOperationException("Could not create the user in seeder");
//        }
//        await this.userHelper.AddUserToRoleAsync(user, "Admin");
//        //alimentamos el Seeder
//        var token = await this.userHelper.GenerateEmailConfirmationTokenAsync(user);
//        await this.userHelper.ConfirmEmailAsync(user, token);


//    }

//    var isInRole = await this.userHelper.IsUserInRoleAsync(user, "Admin");
//    if (!isInRole)
//    {
//        await this.userHelper.AddUserToRoleAsync(user, "Admin");
//    }



//    if (!this.context.Products.Any())
//    {
//        //Any, si al menos hay un registro regresa true
//        this.AddProduct("iphone X", user);
//        this.AddProduct("Magic Mouse", user);
//        this.AddProduct("iWatch Series 4", user);
//        await this.context.SaveChangesAsync();
//    }
//}



////los productos llevan un usuario por eso pasamos user como parámeto
//private void AddProduct(string name, User user)
//{
//    this.context.Products.Add(new Product
//    {
//        Name = name,
//        Price = this.random.Next(100),
//        IsAvailabe = true,
//        Stock = this.random.Next(100),
//        User = user
//    });
//}

//    }
//}

#endregion
