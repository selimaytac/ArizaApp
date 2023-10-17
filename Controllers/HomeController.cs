﻿using System;
using System.Diagnostics;
using System.Threading.Tasks;
using ArizaApp.Helpers;
using ArizaApp.Models.ConstTypes;
using ArizaApp.Models.DbContexts;
using ArizaApp.Models.Dtos;
using ArizaApp.Models.Entities;
using ArizaApp.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ArizaApp.Controllers
{
    public class HomeController : BaseController
    {
        public HomeController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager,
            RoleManager<AppRole> roleManager, ArizaDbContext dbContext)
            : base(userManager, signInManager, roleManager, dbContext)
        {
        }

        [Authorize(Roles = RoleTypes.AllRoles)]
        public IActionResult Index()
        {
            ViewBag.CurrentUser = CurrentUser;
            var message = $"User Login: {CurrentUser.UserName}";
            GeneralLogger.AddLog(DbContext, new LogRecord{ IpAddress = RequestHelper.GetIpAddress(Request), Date = DateTime.Now, LogType = LogTypes.Login.ToString(), Message = message, Port = RequestHelper.GetPort(Request), UserName = CurrentUser.UserName});

            return View();
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult LogIn(string returnUrl)
        {
            if (User.Identity is {IsAuthenticated: true}) return RedirectToAction("Index");
            
            TempData["ReturnUrl"] = returnUrl;
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<ActionResult> LogIn(LoginDto userLogin)
        {
            if (ModelState.IsValid)
            {
                var user = await UserManager.FindByEmailAsync(userLogin.Email);

                if (user != null)
                {
                    #region Hesap kilitlenmis mi?

                    // if (await UserManager.IsLockedOutAsync(user))
                    // {
                    //     ModelState.AddModelError("",
                    //         "Hesabınız bir süreliğine kilitlenmiştir. Lütfen daha sonra tekrar deneyiniz.");
                    //
                    //     return View(userLogin);
                    // }

                    #endregion

                    await SignInManager.SignOutAsync();

                    var result = await SignInManager.PasswordSignInAsync(user,
                        userLogin.Password, userLogin.RememberMe, false);

                    if (result.Succeeded)
                    {
                        // await UserManager.ResetAccessFailedCountAsync(user);
                        
                        if (TempData["ReturnUrl"] != null) return Redirect(TempData["ReturnUrl"].ToString());

                        return RedirectToAction("Index", "Home");
                    }

                    #region Hesap kilitleme kontrolü

                    // await UserManager.AccessFailedAsync(user);

                    // var fail = await UserManager.GetAccessFailedCountAsync(user);

                    // if (fail < 3) ModelState.AddModelError("", $"{fail} kez başarısız giriş yaptınız");

                    // if (fail >= 3)
                    // {
                    //     await UserManager.SetLockoutEndDateAsync(user,
                    //         new DateTimeOffset(DateTime.Now.AddMinutes(20)));
                    //
                    //     // Bu değişebilir mesala 6. denemede direkt hesap kitlenebilir. (Şu anlık sıfırlıyorum sayacı.)
                    //     await UserManager.ResetAccessFailedCountAsync(user);
                    //     ModelState.AddModelError("",
                    //         "Hesabınız 3 başarısız girişten dolayı 20 dakika süreyle kitlnemiştir. Lütfen daha sonra tekrar deneyiniz.");
                    // }
                    // else

                    #endregion

                    {
                        ModelState.AddModelError("", "Email adresiniz veya şifreniz yanlış.");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Bu email adresine kayıtlı bir kullanıcı bulunamamıştır.");
                }
            }

            return View(userLogin);
        }

        public IActionResult LogOut()
        {
            SignInManager.SignOutAsync();
            var message = $"User Logout: {CurrentUser.UserName}";
            GeneralLogger.AddLog(DbContext, new LogRecord{ IpAddress = RequestHelper.GetIpAddress(Request), Date = DateTime.Now, LogType = LogTypes.Logout.ToString(), Message = message, Port = RequestHelper.GetPort(Request), UserName = CurrentUser.UserName});

            return RedirectToAction("LogIn");
        }

        public IActionResult AccessDenied()
        {
            return View();
        }

        [Authorize(Roles = RoleTypes.AllRoles)]
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel {RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});
        }
    }
}