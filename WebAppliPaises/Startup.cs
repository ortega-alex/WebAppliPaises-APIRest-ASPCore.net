using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using WebAppliPaises.Models;

namespace WebAppliPaises
{
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
            //db in memory
            //services.AddDbContext<AplicationDbContext>(options => options.UseInMemoryDatabase("paisDB"));


            services.AddDbContext<AplicationDbContext>(options =>
            options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<AplicationDbContext>()
                .AddDefaultTokenProviders();

            //services the authentication for token
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = "yourdomain.com",
                        ValidAudience = "yourdomain.com",
                        IssuerSigningKey = new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes(Configuration["Llave_super_secreta"])
                        ),
                        ClockSkew = TimeSpan.Zero
                    }
                );

            services.AddMvc().AddJsonOptions(ConfiguracionJson);

        }

        private void ConfiguracionJson(MvcJsonOptions obj)
        {
            obj.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env , AplicationDbContext context)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            //midleware authentication
            app.UseAuthentication();

            app.UseMvc();

            if (!context.Paises.Any()) {              
                    context.Paises.AddRange(new List<Pais> {
                        new Pais(){Nombre = "Republica Dominicana" , Provincias = new List<Provincia>(){
                            new Provincia(){Nombre = "Azua"}
                         } } ,
                        new Pais(){Nombre = "Mexico" , Provincias = new List<Provincia>(){
                            new Provincia(){Nombre = "Puebla"},
                            new Provincia(){Nombre = "Queretaro"}
                         } } ,
                        new Pais(){Nombre = "Argentina"}
                    });

                    context.SaveChanges(); 
            }
        }
    }
}
