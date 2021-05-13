using BeekeepingStore.AppDbContext;
using BeekeepingStore.Views.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BeekeepingStore.Data
{
    public class DBInitializer : IDBInitializer
    {
        private readonly BeekeepingDbContext _db;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public DBInitializer(BeekeepingDbContext db, UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _db = db;
            _userManager = userManager;
            _roleManager = roleManager;
        }
        public async void Initialize()
        {   
            //Add pending migration if exists
            if (_db.Database.GetPendingMigrations().Count() > 0)
            {
                _db.Database.Migrate();
            }
           
            //Exit if role alredy exists
            if (_db.Roles.Any(r => r.Name == Helpers.Roles.Admin)) return;
           
            //Create Admin role
            _roleManager.CreateAsync(new IdentityRole(Helpers.Roles.Admin)).GetAwaiter().GetResult();

            //Create Admin user
            _userManager.CreateAsync(new AplicationUser
            {
                UserName = "Admin",
                Email = "Admin@gmail.com",
                EmailConfirmed = true,

            }, "Admin@123").GetAwaiter().GetResult();

            //Assign role to Admin user
            await _userManager.AddToRoleAsync(await _userManager.FindByNameAsync("Admin"), Helpers.Roles.Admin);
        }
    }
}
