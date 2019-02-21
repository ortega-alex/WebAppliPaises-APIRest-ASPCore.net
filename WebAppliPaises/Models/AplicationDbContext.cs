using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAppliPaises.Models
{
    //public class AplicationDbContext : DbContext
    public class AplicationDbContext : IdentityDbContext<ApplicationUser>
    {

        public AplicationDbContext(DbContextOptions<AplicationDbContext> options)
            :base(options)
        {

        }
        public DbSet<Pais> Paises { get; set; }
        public DbSet<Provincia> Provincias { get; set; }
    }
}
