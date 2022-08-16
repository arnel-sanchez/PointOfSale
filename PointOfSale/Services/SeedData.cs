using PointOfSale.DataAccess;
using PointOfSale.Definitions;
using PointOfSale.Models.DataBaseModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PointOfSale.Services
{
    public class SeedData
    {
        public static async Task CreateAdminAccount(UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
            var rol = new IdentityRole(Roles.Admin);
            if(await userManager.FindByNameAsync("Admin")==null)
            {
                if(await roleManager.FindByNameAsync(Roles.Admin) == null)
                {
                    await roleManager.CreateAsync(rol);
                }
                User user = new User
                {
                    UserName = "Admin",
                    Email = "admin@gmail.com",
                    LastName = "Admin",
                    Name = "Admin",
                    Role = Roles.Admin,
                    EmailConfirmed = true,
                    LockoutEnabled = false
                };
                var result = await userManager.CreateAsync(user, "Admin123*");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, Roles.Admin);
                }
            }
        }

        public static async Task CreateSellerAccount(UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
            var rol = new IdentityRole(Roles.Seller);
            if (await userManager.FindByNameAsync("Seller") == null)
            {
                if (await roleManager.FindByNameAsync(Roles.Seller) == null)
                {
                    await roleManager.CreateAsync(rol);
                }
                User user = new User
                {
                    UserName = "Seller",
                    Email = "seller@gmail.com",
                    LastName = "Seller",
                    Name = "Seller",
                    Role = Roles.Seller,
                    EmailConfirmed = true,
                    LockoutEnabled = false
                };
                var result = await userManager.CreateAsync(user, "Seller123*");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, Roles.Seller);
                }
            }
        }

        public static async Task CreateAdministrativeAccount(UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
            var rol = new IdentityRole(Roles.Administrative);
            if (await userManager.FindByNameAsync("Administrative") == null)
            {
                if (await roleManager.FindByNameAsync(Roles.Administrative) == null)
                {
                    await roleManager.CreateAsync(rol);
                }
                User user = new User
                {
                    UserName = "Administrative",
                    Email = "administrative@gmail.com",
                    LastName = "Administrative",
                    Name = "Administrative",
                    Role = Roles.Administrative,
                    EmailConfirmed = true,
                    LockoutEnabled = false
                };
                var result = await userManager.CreateAsync(user, "Administrative123*");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, Roles.Administrative);
                }
            }
        }

        public static async Task CreateRoles(RoleManager<IdentityRole> roleManager)
        {
            var rol = new IdentityRole(Roles.Admin);
            if (await roleManager.FindByNameAsync(Roles.Admin) == null)
            {
                await roleManager.CreateAsync(rol);
            }
            rol = new IdentityRole(Roles.Seller);
            if (await roleManager.FindByNameAsync(Roles.Seller) == null)
            {
                await roleManager.CreateAsync(rol);
            }
            rol = new IdentityRole(Roles.Administrative);
            if (await roleManager.FindByNameAsync(Roles.Administrative) == null)
            {
                await roleManager.CreateAsync(rol);
            }
        }

        public static void CreateItems(IItemsDataAccess itemsDataAcces)
        {
            if(itemsDataAcces.GetItems().Count == 0)
            {
                itemsDataAcces.AddItem("Item1", 1.40, "Probando los Items", 50, "Category1", "", "1234567", new List<string>());
                itemsDataAcces.AddItem("Item2", 1.50, "Probando los Items", 10, "Category2", "", "1234568", new List<string>());
                itemsDataAcces.AddItem("Item3", 1.60, "Probando los Items", 20, "Category4", "", "1234569", new List<string>());
            }
        }
    }
}
