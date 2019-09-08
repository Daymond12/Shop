

namespace Shop.Web.Data
{
    
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Entities;
    public class SeedDb
    {
        private readonly DataContext context;
        private Random random;

        //1-se le inyecta la conexión a la base datos
        public SeedDb(DataContext context)
        {
            this.context = context;
            this.random = new Random();
        }

        //2-método para alimentar la BD
        public async Task SeedAsync()
        {
            await this.context.Database.EnsureCreatedAsync();

            if (!this.context.Products.Any())
            {
                //Any, si al menos hay un registro regresa true
                this.AddProduct("iphone X");
                this.AddProduct("Magic Mouse");
                this.AddProduct("iWatch Series 4");
                await this.context.SaveChangesAsync();
            }
        }

        private void AddProduct(string name)
        {
            this.context.Products.Add(new Product
            {
                Name = name,
                Price = this.random.Next(100),
                IsAvailabe = true,
                Stock = this.random.Next(100)
            });
        }

    }
}
