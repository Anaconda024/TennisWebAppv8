using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using TennisWebApp8.Data;


namespace TennisWebApp8
{
    public static class SeedData
    {
        public static void Seed(UserManager<AccountUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            SeedRoles(roleManager);
            SeedUser(userManager);
        }

        private static void SeedUser(UserManager<AccountUser> userManager)
        {
            if (userManager.FindByNameAsync("admin").Result == null)
            {
                var user = new AccountUser
                {
                    UserName = "admin@localhost.com",
                    Email = "admin@localhost.com",
                    FirstName = "admin"
                };
                var result = userManager.CreateAsync(user, "Password1#").Result;
                
                if (result.Succeeded)
                {
                    userManager.AddToRoleAsync(user, "Administrator").Wait();
                }
            }
        }

        private static void SeedRoles(RoleManager<IdentityRole> roleManager)
        {
            if (!roleManager.RoleExistsAsync("Administrator").Result)
            {
                var role = new IdentityRole
                {
                    Name = "Administrator"
                };
                var result = roleManager.CreateAsync(role).Result;
            }
            if (!roleManager.RoleExistsAsync("Coach").Result)
            {
                var role = new IdentityRole
                {
                    Name = "Coach"
                };
                var result = roleManager.CreateAsync(role).Result;
            }
            if (!roleManager.RoleExistsAsync("ClientPlayer").Result)
            {
                var role = new IdentityRole
                {
                    Name = "ClientPlayer"
                };
                var result = roleManager.CreateAsync(role).Result;
            }
        }
    }
}

