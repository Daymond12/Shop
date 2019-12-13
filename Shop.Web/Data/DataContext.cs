

namespace Shop.Web.Data
{
    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore;
    using Shop.Web.Data.Entities;
    using System.Linq;

    // public class DataContext: DbContext
    /// <summary>
    /// Se ha modificado el datacontext para user
    /// ya que IdentityDbContext incluye las tablas de usuarios<U ser> es mi modelo
    /// </summary>
    public class DataContext : IdentityDbContext<User>
    {
        //DbSet me tira la propiedad de Prodcut a la base de datos
        //Mis datos no los trataré como si fueran tablas, sino como colección de objetos
        public DbSet<Product> Products { get; set; }

        //creamos la tabla de pa[ises
        //En este punto he modificado la base de datos y debo correr el modo comando     
        public DbSet<Country> Countries { get; set; }

        public DbSet<Order> Orders { get; set; }

        public DbSet<OrderDetail> OrderDetails { get; set; }

        public DbSet<OrderDetailTemp> OrderDetailTemps { get; set; }

        //mandamos la tabla City a la base de datos(City es una colección)
        public DbSet<City> Cities { get; set; }

        //Creamos la conexión a la Base de datos
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //para evitar el warning
            /*En el model builder hay una entidad producto
           * ,mapea price como decimal obligatoriamente, sql lo está haciendo por defecto*/
            modelBuilder.Entity<Product>()
        .Property(p => p.Price)
        .HasColumnType("decimal(18,2)");

            //Boorado en cascada
            var cascadeFKs = modelBuilder.Model
        .G­etEntityTypes()
        .SelectMany(t => t.GetForeignKeys())
        .Where(fk => !fk.IsOwnership && fk.DeleteBehavior == DeleteBehavior.Casca­de);
            foreach (var fk in cascadeFKs)
            {
                fk.DeleteBehavior = DeleteBehavior.Restr­ict;
            }


            base.OnModelCreating(modelBuilder);
        }

    }
}
