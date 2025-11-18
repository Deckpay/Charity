using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Charity.Core.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Charity.Infrastructure.Data
{
    public class CharityDbContext : IdentityDbContext<ApplicationUser>
    {
        public CharityDbContext(DbContextOptions<CharityDbContext> options) : base(options) { }

        // Táblák (DbSet-ek)
        public DbSet<City> Cities { get; set; }
        public DbSet<Category> Categories { get; set; }
    }
}
