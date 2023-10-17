using System;
using System.Threading.Tasks;
using ArizaApp.Helpers;
using ArizaApp.Models.ConstTypes;
using ArizaApp.Models.DbContexts;
using ArizaApp.Models.Dtos;
using ArizaApp.Models.Entities;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ArizaApp.Controllers
{
    public class MailController : BaseController
    {
        public MailController(ArizaDbContext dbContext, UserManager<AppUser> userManager)
            : base(userManager, null, null, dbContext)
        {
        }

        [HttpGet]
        [Authorize(Roles = RoleTypes.AllRoles)]
        public async Task<IActionResult> GetEmails()
        {
            var emails = await DbContext.EmailRecords.AsNoTracking().ToListAsync();

            return View(emails);
        }

        [HttpGet]
        [Authorize(Roles = RoleTypes.AdminEditor)]
        public IActionResult CreateEmail()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = RoleTypes.AdminEditor)]
        public async Task<IActionResult> CreateEmail(CreateEmailDto createEmailDto)
        {
            if (ModelState.IsValid)
            {
                var emailExist =
                    await DbContext.EmailRecords.FirstOrDefaultAsync(x =>
                        x.EmailAddress == createEmailDto.EmailAddress);
                if (emailExist != null)
                {
                    ModelState.AddModelError("", "Email adresi zaten mevcut!");
                    return View(createEmailDto);
                }

                var email = new EmailRecord
                {
                    EmailAddress = createEmailDto.EmailAddress,
                    EmailDescription = createEmailDto.EmailDescription
                };

                DbContext.EmailRecords.Add(email);
                await DbContext.SaveChangesAsync();
                
                var message = $"Email Created: {email.EmailAddress}";
                GeneralLogger.AddLog(DbContext, new LogRecord{ IpAddress = RequestHelper.GetIpAddress(Request), Date = DateTime.Now, LogType = LogTypes.CreateEmail.ToString(), Message = message, Port = RequestHelper.GetPort(Request), UserName = CurrentUser.UserName});

                return RedirectToAction("GetEmails");
            }

            ModelState.AddModelError("", "Email eklenemedi lütfen tekrar deneyiniz.");
            return View(createEmailDto);
        }

        [HttpGet]
        [Authorize(Roles = RoleTypes.AdminEditor)]
        public async Task<IActionResult> UpdateEmail(int id)
        {
            var email = await DbContext.EmailRecords.FindAsync(id);

            if (email == null) return RedirectToAction("GetEmails");

            var emailDto = email.Adapt<UpdateEmailDto>();

            return View(emailDto);
        }

        [HttpPost]
        [Authorize(Roles = RoleTypes.AdminEditor)]
        public async Task<IActionResult> UpdateEmail(UpdateEmailDto updateEmailDto)
        {
            if (ModelState.IsValid)
            {
                var emailExist =
                    await DbContext.EmailRecords.FirstOrDefaultAsync(x =>
                        x.EmailAddress == updateEmailDto.EmailAddress);

                if (emailExist != null && emailExist.Id != updateEmailDto.Id)
                {
                    ModelState.AddModelError("", "Bu isimle başka bir Email adresi zaten mevcut!");
                    return View(updateEmailDto);
                }

                var email = await DbContext.EmailRecords.FindAsync(updateEmailDto.Id);

                if (email == null) return RedirectToAction("GetEmails");

                email.EmailAddress = updateEmailDto.EmailAddress;
                email.EmailDescription = updateEmailDto.EmailDescription;

                await DbContext.SaveChangesAsync();
                var message = $"Email {emailExist} Updated: {email.EmailAddress}";
                GeneralLogger.AddLog(DbContext, new LogRecord{ IpAddress = RequestHelper.GetIpAddress(Request), Date = DateTime.Now, LogType = LogTypes.UpdateEmail.ToString(), Message = message, Port = RequestHelper.GetPort(Request), UserName = CurrentUser.UserName});

                
                
                return RedirectToAction("GetEmails");
            }

            ModelState.AddModelError("", "Email güncellenemedi lütfen tekrar deneyiniz.");
            return View(updateEmailDto);
        }

        [HttpGet]
        [Authorize(Roles = RoleTypes.AdminEditor)]
        public async Task<IActionResult> DeleteConfirmEmail(int id)
        {
            var email = await DbContext.EmailRecords.FindAsync(id);

            if (email == null) return RedirectToAction("GetEmails");

            var emailDto = email.Adapt<UpdateEmailDto>();

            return View(emailDto);
        }

        [HttpGet]
        [Authorize(Roles = RoleTypes.AdminEditor)]
        public async Task<IActionResult> DeleteEmail(int id)
        {
            var email = await DbContext.EmailRecords.FindAsync(id);

            if (email == null) return RedirectToAction("GetEmails");

            DbContext.EmailRecords.Remove(email);
            await DbContext.SaveChangesAsync();
            
            var message = $"Email Deleted: {email.EmailAddress}";
            GeneralLogger.AddLog(DbContext, new LogRecord{ IpAddress = RequestHelper.GetIpAddress(Request), Date = DateTime.Now, LogType = LogTypes.DeleteEmail.ToString(), Message = message, Port = RequestHelper.GetPort(Request), UserName = CurrentUser.UserName});

            return RedirectToAction("GetEmails");
        }
    }
}