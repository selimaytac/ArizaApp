using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ArizaApp.Helpers;
using ArizaApp.Models.ConstTypes;
using ArizaApp.Models.DbContexts;
using ArizaApp.Models.Dtos;
using ArizaApp.Models.Entities;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace ArizaApp.Controllers
{
    public class AdminController : BaseController
    {
        public AdminController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager,
            RoleManager<AppRole> roleManager, ArizaDbContext dbContext)
            : base(userManager, signInManager, roleManager, dbContext)
        {
        }

        [HttpGet]
        [Authorize(Roles = RoleTypes.Admin)]
        public IActionResult Index()
        {
            return View();
        }


        [HttpGet]
        [Authorize(Roles = RoleTypes.Admin)]
        public IActionResult CreateUser()
        {
            ViewBag.DepartmentList = ViewListHelper.GetDepartments(DbContext);
            ViewBag.RoleList = ViewListHelper.GetRoles(DbContext);

            return View();
        }

        [HttpGet]
        [Authorize(Roles = RoleTypes.Admin)]
        public IActionResult GetUsers()
        {
            var users = UserManager.Users.Include(r => r.Department).ToList();

            var userDtos = (from user in users
                select new UserDto
                {
                    UserName = user.UserName,
                    Email = user.Email,
                    Name = user.Name,
                    Surname = user.Surname,
                    DepartmentName = user.Department.DepartmentName,
                    RoleName = (from role in UserManager.GetRolesAsync(user).Result select role).FirstOrDefault() ??
                               "Rol Yok",
                    Note = user.Note,
                    SendCount = user.SendCount
                }).ToList();

            return View(userDtos);
        }

        [HttpPost]
        [Authorize(Roles = RoleTypes.Admin)]
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
                            return RedirectToAction("GetUsers");
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

        [HttpGet]
        [Authorize(Roles = RoleTypes.AllRoles)]
        public IActionResult ProfileSettings()
        {
            return View(CurrentUser);
        }

        [HttpGet]
        [Authorize(Roles = RoleTypes.AllRoles)]
        public IActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = RoleTypes.AllRoles)]
        public async Task<IActionResult> ChangePassword(ChangePasswordDto changePasswordDto)
        {
            if (ModelState.IsValid)
            {
                var user = CurrentUser;

                if (user != null)
                {
                    var isOldPasswordTrue = UserManager.CheckPasswordAsync(user, changePasswordDto.PasswordOld)
                        .Result;

                    if (isOldPasswordTrue)
                    {
                        var result = UserManager.ChangePasswordAsync(
                            user, changePasswordDto.PasswordOld,
                            changePasswordDto.PasswordNew).Result;

                        if (result.Succeeded)
                        {
                            // Security stamp her 30 dakikada bir kontrol ediliyor.
                            // O yüzden biz veritabanındaki stampi güncellemezek
                            // kullanıcı eski stampi ile sitede gezmeye devam edebilir.
                            await UserManager.UpdateSecurityStampAsync(user);

                            // Stamp güncellendiği için 30 dakika sonra sistemden kontrol edip atacaktır.
                            // Burada kullanıcıya sistemden arka planda çıkış yaptırıp, tekrar giriş yaptırıyoruz.
                            // Böylece cookie verileri güncelleniyor ve 30 dakika sonra sistemden atılması engelleniyor.
                            await SignInManager.SignOutAsync();
                            await SignInManager.PasswordSignInAsync(user, changePasswordDto.PasswordNew, true, false);

                            ViewBag.success = "True";
                        }
                        else
                        {
                            AddModelError(result);
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("", "Eski şifrenizi yanlış girdiniz.");
                    }
                }
            }

            return View(changePasswordDto);
        }
    }
}