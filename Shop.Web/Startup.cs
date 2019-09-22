
namespace Shop.Web
{
    using Data;
    using Data.Entities;
    using Helpers;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            //le decimos que la clase user implemantara el IdentityRole
            //aca fongiguramos las restricciones y un viajao de  cosas
            services.AddIdentity<User, IdentityRole>(cfg =>
            {
                //unico usuario con un email
                cfg.User.RequireUniqueEmail = true;
                //no requiere numeros
                cfg.Password.RequireDigit = false;
                //no requiere caracteres especiales
                cfg.Password.RequiredUniqueChars = 0;
                //no requiere minusculas
                cfg.Password.RequireLowercase = false;
                //no requiere letras que no seal alfabéticas
                cfg.Password.RequireNonAlphanumeric = false;
                //no requiere mayúsculas
                cfg.Password.RequireUppercase = false;
                //longitud mínima del password
                cfg.Password.RequiredLength = 6;
            })
        .AddEntityFrameworkStores<DataContext>();


            services.AddDbContext<DataContext>(cfg =>
            {
                cfg.UseSqlServer(this.Configuration.GetConnectionString("DefaultConnection"));
            });

            //INYECCÓN DEL SEEDB
            services.AddTransient<SeedDb>();

            //INYECTANDO EL IREPOSITORY
            services.AddScoped<IRepository, Repository>();

            //inyectando el Iuserhelper
            //con eso el proyeco sabrá lo que tiene que inyectar y que implementar
            //se inyectan/configuran mis interfaces
            services.AddScoped<IUserHelper, UserHelper>();



            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });


            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseAuthentication();//requiere autenticación|
            app.UseCookiePolicy();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
