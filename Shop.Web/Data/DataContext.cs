﻿

namespace Shop.Web.Data
{
    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore;
    using Shop.Web.Data.Entities;
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

        //Creamos la conexión a la Base de datos
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }

    }
}
