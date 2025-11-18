using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Charity.Core.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Charity.Infrastructure.Data.Seed
{
    public class DbInitializer
    {
        private readonly CharityDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public DbInitializer(
            CharityDbContext context,
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        //Ez a metódus indítja a seed folyamatot
        //Seed: az adatbázis előzetes feltöltése alapadatokkal, hogy az alkalmazás induláskor működjön.
        public async Task InitializeAsync() //Aszinkron metódus a metódus nem blokkolja a fő szálat miközben vár valamilyen műveletre.
            //CharityDbContext context, //Az adatbázisunk, amin keresztül írunk/olvasunk
            //UserManager<ApplicationUser> userManager,
            //RoleManager<IdentityRole> roleManager)
        {
            //Amikor először indítod az alkalmazást, így nem kell külön migráció és Update-Database — az app maga gondoskodik arról, hogy az adatbázis naprakész legyen.
            await _context.Database.MigrateAsync(); // Lefuttatja a migrációkat, ha még nem történtek meg. (adatbázis migráció élesítése induláskor)

            //Alep szerepkörök listája, amiknek léteznie kell
            string[] roles = { "Admin", "Adományozó", "Rászoruló" };

            //Végigmegyünk a listán és letrehozzuk őket
            foreach (var role in roles)
            {
                //megvizsgálja létezik e már az adatbázisban
                if (!await _roleManager.RoleExistsAsync(role))
                {
                    //ha nem létezik létrehozza
                    await _roleManager.CreateAsync(new IdentityRole(role));
                }
            }

            if (!_context.Cities.Any())
            {
                var cities = new[]
                {
                    "Budapest", "Debrecen", "Szeged", "Győr",
                    "Miskolc", "Pécs", "Kecskemét", "Nyíregyháza",
                    "Székesfehérvár", "Eger"
                };

                //hozzáadjuk a táblához
                foreach (var city in cities)
                {
                    _context.Cities.Add(new City { CityName = city });
                }
                
                //mentjuk
                await _context.SaveChangesAsync();
            }

            if (!_context.Categories.Any())
            {
                var categoris = new[] { "Ruhák", "Háztartási gépek", "Tartós élelmiszer", "Bútorok", "Iskolaszerek", "Elektronikai cikkek", "Baba cuccok" };

                foreach (var category in categoris)
                {
                    _context.Categories.Add(new Category { CategorName = category });
                }

                await _context.SaveChangesAsync();
            }

            string adminEmail = "admin@charity.hu";
            string adminPassword = "Admin123!";

            // Find user by email (Identity built-in method)
            if (await _userManager.FindByEmailAsync(adminEmail) == null)
            {
                var admin = new ApplicationUser
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    FullName = "Rendszer Admin",
                    UserType = "Admin",
                    CityId = 1,
                    CreatedAt = DateTime.Now,
                };
                //Felhasználó létrehozása
                var result = await _userManager.CreateAsync(admin, adminPassword);

                //Ha sikeres, adjuk hozzá a szerephez
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(admin, "Admin");
                }
            }
        }
    }
}
