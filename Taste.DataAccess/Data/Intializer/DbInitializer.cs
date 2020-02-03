using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Test.Models;
using Test.Utility;

namespace Taste.DataAccess.Data.Intializer
{
    class DbInitializer : IDbInitializer
    {

        private readonly ApplicationDbContext _db;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;


        //assigning role to user as user created 
        public DbInitializer(ApplicationDbContext db, UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _db = db;
            _roleManager = roleManager;
            _userManager = userManager;
        }


        public void Initialize()
        {
            try
            {
                if (_db.Database.GetPendingMigrations().Count() > 0)
                {
                    //applied first pending migration to database
                    _db.Database.Migrate();
                }
            }
            catch (Exception)
            {
            }

            //checking if role alredy there will return
            if (_db.Roles.Any(r => r.Name == SD.ManagerRole)) return;
            //or else we will create role only once.
            _roleManager.CreateAsync(new IdentityRole(SD.ManagerRole)).GetAwaiter().GetResult();
            _roleManager.CreateAsync(new IdentityRole(SD.FrontDeskRole)).GetAwaiter().GetResult();
            _roleManager.CreateAsync(new IdentityRole(SD.KitchenRole)).GetAwaiter().GetResult();
            _roleManager.CreateAsync(new IdentityRole(SD.CustomerRole)).GetAwaiter().GetResult();

            //creating first admin user for Manager Role
            _userManager.CreateAsync(new ApplicationUser
            {
                UserName = "admin123@gmail.com",
                Email = "admin123@gmail.com",
                EmailConfirmed = true,
                FirstName = "Hardik",
                LastName = "Patel"
            }, "Admin@123").GetAwaiter().GetResult();

            ApplicationUser user = _db.ApplicationUser.Where
                (u => u.Email == "admin123@gmail.com").FirstOrDefault();

            _userManager.AddToRoleAsync(user, SD.ManagerRole).GetAwaiter().GetResult();
        }
    }
}
