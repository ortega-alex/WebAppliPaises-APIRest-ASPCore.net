using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
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
            services.AddDbContext<AplicationDbContext>(options => options.UseInMemoryDatabase("paisDB"));
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
