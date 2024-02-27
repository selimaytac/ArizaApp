using System;
using System.Linq;
using System.Threading.Tasks;
using ArizaApp.Helpers;
using ArizaApp.Models.DbContexts;
using ArizaApp.Models.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace ArizaApp.Models.Seed
{
    public class SeedDb
    {
        public static async Task InitializeUser(IServiceProvider serviceProvider, SeedObject seedObject)
        {
            var context = serviceProvider.GetService<ArizaDbContext>();
            
            // if any users exist in the database, do not seed the database with the default user
            if (context.Users.Any()) return;
            
            var userManager = serviceProvider.GetService<UserManager<AppUser>>();

            context.Departments.Add(new Department
            {
                DepartmentName = seedObject.Department
            });

            await context.SaveChangesAsync();
            
            var user = new AppUser
            {
                UserName = seedObject.UserName,
                Email = seedObject.Email,
                Name = seedObject.Name,
                Surname = seedObject.Surname,
                Note = seedObject.Note,
                DepartmentId = 1 
            };
            
            if (!context.Users.Any(u => u.UserName == user.UserName))
            {
                await userManager!.CreateAsync(user, seedObject.Password);
                await userManager.AddToRoleAsync(user, "Admin");
            }
        }

        public static async Task InitializeRoles(IServiceProvider serviceProvider)
        {
            var context = serviceProvider.GetService<ArizaDbContext>();

            if (context.Roles.Count() < 4)
            {
                var roles = new string[] {"Admin", "Editor", "Viewer", "PlannedEditor"};

                foreach (var role in roles)
                {
                    var roleStore = new RoleStore<AppRole>(context);

                    if (!context.Roles.Any(r => r.Name == role))
                    {
                        await roleStore.CreateAsync(new AppRole
                            {Name = role, NormalizedName = role.TurkishToEnglishToUpper()});
                    }
                }
            }
        }
    }
}