
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
    using Microsoft.IdentityModel.Tokens;
    using Shop.Web.Data.Repositories;
    using System.Text;

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
                //DefaultAuthenticatorProvider será nuestro proveedor de tokens
                cfg.Tokens.AuthenticatorTokenProvider = TokenOptions.DefaultAuthenticatorProvider;
                //cuando el usuario haga un SignIn requerirá confirmación de email
                cfg.SignIn.RequireConfirmedEmail = true;

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
                //adicionear el tokenProvider
        .AddDefaultTokenProviders()
        .AddEntityFrameworkStores<DataContext>();


            services.AddDbContext<DataContext>(cfg =>
            {
                cfg.UseSqlServer(this.Configuration.GetConnectionString("DefaultConnection"));
            });
            //EL AddAuthentication ES PARA FINES DEL TOKEN
            services.AddAuthentication()
    .AddCookie()
    .AddJwtBearer(cfg =>
    {
        cfg.TokenValidationParameters = new TokenValidationParameters
        {
            ValidIssuer = this.Configuration["Tokens:Issuer"],
            ValidAudience = this.Configuration["Tokens:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(this.Configuration["Tokens:Key"]))
        };
    });


            //INYECCÓN DEL SEEDB
            services.AddTransient<SeedDb>();

            //INYECTANDO EL IREPOSITORY
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<ICountryRepository, CountryRepository>();

            //inyectando el Iuserhelper
            //con eso el proyeco sabrá lo que tiene que inyectar y que implementar
            //se inyectan/configuran mis interfaces
            services.AddScoped<IUserHelper, UserHelper>();

            //configuramos la inyección del Mailhelper para
            //poder usarlos desde cualquier controlador o cualquier clase
            //le decimos que el IMailHeper será implementado con una instancia de MailHelper
            //esto nos prepara el correo para envío de correos
            services.AddScoped<IMailHelper, MailHelper>();


            //
            services.AddScoped<IOrderRepository, OrderRepository>();



            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;

            });


            services.ConfigureApplicationCookie(options =>
            {   //cuando haga login se va a la pagina de no autorizado
                options.LoginPath = "/Account/NotAuthorized";
                //y si necesita ingresar a otra pagina y no tiene permisos
                options.AccessDeniedPath = "/Account/NotAuthorized";
            });



            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            //if (env.IsDevelopment())
            //{
            //    app.UseDeveloperExceptionPage();
            //}
            //else
            //{
            //    app.UseExceptionHandler("/Home/Error");
            //    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            //    app.UseHsts();
            //}
            app.UseStatusCodePagesWithReExecute("/error/{0}");
            app.UseDeveloperExceptionPage();
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
