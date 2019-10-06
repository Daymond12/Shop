
namespace Shop.Web
{
    //ODIGO COMENTADO EN LA PARTE DEL SEEDB 
    //using System;
    //using System.Collections.Generic;
    //using System.IO;
    //using System.Linq;
    //using System.Threading.Tasks;
    //using Microsoft.AspNetCore;
    //using Microsoft.AspNetCore.Hosting;
    //using Microsoft.Extensions.Configuration;
    //using Microsoft.Extensions.Logging;

    //public class Program
    //{
    //    public static void Main(string[] args)
    //    {
    //        CreateWebHostBuilder(args).Build().Run();
    //    }

    //    public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
    //        WebHost.CreateDefaultBuilder(args)
    //            .UseStartup<Startup>();
    //}


    using Data;
    using Microsoft.AspNetCore;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.DependencyInjection;
    using Shop.Web.Helpers;
    using System;

    public class Program
    {
        public static void Main(string[] args)
        {
           
            try
            {
                var host = CreateWebHostBuilder(args).Build();
                RunSeeding(host);
                host.Run();
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
            }
            try
            {
                CreateWebHostBuilder(args).Build().Run();
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
            }
        }

        private static void RunSeeding(IWebHost host)
        {
            var scopeFactory = host.Services.GetService<IServiceScopeFactory>();
            using (var scope = scopeFactory.CreateScope())
            {
                var seeder = scope.ServiceProvider.GetService<SeedDb>();
                seeder.SeedAsync().Wait();
            }
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();
    }

}
