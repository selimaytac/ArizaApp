using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ArizaApp.Helpers;
using ArizaApp.Models.DbContexts;
using ArizaApp.Models.Dtos;
using ArizaApp.Models.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ArizaApp.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : BaseController
    {
        public AdminController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager,
            RoleManager<AppRole> roleManager, ArizaDbContext dbContext)
            : base(userManager, null, roleManager, dbContext)
        {
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }


        [HttpGet]
        public IActionResult CreateUser()
        {
            ViewBag.DepartmentList = ViewListHelper.GetDepartments(DbContext);
            ViewBag.RoleList = ViewListHelper.GetRoles(DbContext);

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser(CreateUserDto createUserDto)
        {
            if (ModelState.IsValid)
            {
                // check username or email are unique
                if (UserManager.FindByNameAsync(createUserDto.UserName).Result == null)
                {
                    if (UserManager.FindByEmailAsync(createUserDto.Email).Result == null)
                    {
                        // create user
                        var user = new AppUser
                        {
                            UserName = createUserDto.UserName,
                            Email = createUserDto.Email,
                            Name = createUserDto.Name,
                            Surname = createUserDto.Surname,
                            DepartmentId = Convert.ToInt32(createUserDto.DepartmentId),
                            Note = createUserDto.Note
                        };
                        var result = await UserManager.CreateAsync(user, createUserDto.Password);

                        if (result.Succeeded)
                        {
                            var role = RoleManager.Roles.FirstOrDefault(r => r.Id == createUserDto.RoleId);

                            // add user to role || if role not exist, add Viewer role to the new user
                            await UserManager.AddToRoleAsync(user, role!.Name ?? "Viewer");
                            return RedirectToAction("Index");
                        }

                        foreach (var error in result.Errors) ModelState.AddModelError("", error.Description);
                    }
                    else
                    {
                        ModelState.AddModelError("", "Email adresi zaten kayıtlı.");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Kullanıcı adı zaten kayıtlı.");
                }
            }

            ViewBag.DepartmentList = ViewListHelper.GetDepartments(DbContext);
            ViewBag.RoleList = ViewListHelper.GetRoles(DbContext);

            return View(createUserDto);
        }
    }
}