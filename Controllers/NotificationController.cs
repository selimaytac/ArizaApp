using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ArizaApp.Models.ConstTypes;
using ArizaApp.Models.DbContexts;
using ArizaApp.Models.Dtos;
using ArizaApp.Models.Entities;
using ArizaApp.Services.Interfaces;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ArizaApp.Controllers
{
    public class NotificationController : BaseController
    {
        private readonly IMailSenderService _mailSenderService;

        public NotificationController(ArizaDbContext dbContext, IMailSenderService mailSenderService,
            UserManager<AppUser> userManager)
            : base(userManager, null, null, dbContext)
        {
            _mailSenderService = mailSenderService;
        }

        [HttpGet]
        [Authorize(Roles = RoleTypes.AllRoles)]
        public async Task<IActionResult> GetNotifications()
        {
            var notifications = await DbContext.ArizaModels.ToListAsync();

            return View(notifications);
        }

        [HttpGet]
        [Authorize(Roles = RoleTypes.AdminEditor)]
        public async Task<IActionResult> CreateArizaNotification()
        {
            ViewBag.Firms = await DbContext.FirmRecords.ToListAsync();

            return View();
        }

        [HttpPost]
        [Authorize(Roles = RoleTypes.AdminEditor)]
        public async Task<IActionResult> CreateArizaNotification(CreateArizaNotificationDto createDto)
        {
            if (ModelState.IsValid)
            {
                var ariza = createDto.Adapt<ArizaModel>();
                ariza.User = CurrentUser;
                ariza.UserId = CurrentUser.Id;

                if (createDto.SendMail)
                {
                    var firms = await DbContext.FirmRecords
                        .Include(x => x.Emails)
                        .Where(x => createDto.FirmIdS.Contains(x.Id))
                        .ToListAsync();

                    ariza.Firms = firms;

                    var emails = firms
                        .SelectMany(x => x.Emails)
                        .Select(x => x.EmailAddress)
                        .Distinct().ToList();

                    await _mailSenderService.SendEmailAsync(emails, createDto.MailSubject, ariza);
                    CurrentUser.SendCount++;
                }

                await DbContext.AddAsync(ariza);
                await DbContext.SaveChangesAsync();

                return RedirectToAction("GetNotifications");
            }

            ViewBag.Firms = DbContext.FirmRecords.ToList();
            return View(createDto);
        }
        
        [HttpGet]
        [Authorize(Roles = RoleTypes.AdminEditor)]
        public async Task<IActionResult> UpdateArizaNotification(int id)
        {
            var arizaModel = await DbContext.ArizaModels.FindAsync(id);
            return View(arizaModel.Adapt<UpdateArizaNotificationDto>());
        }
        
        [HttpPost]
        [Authorize(Roles = RoleTypes.AdminEditor)]
        public async Task<IActionResult> UpdateArizaNotification(UpdateArizaNotificationDto updateDto)
        {
            if (ModelState.IsValid)
            {
                var arizaModel = await DbContext.ArizaModels.FindAsync(updateDto.Id);
                arizaModel = updateDto.Adapt(arizaModel);

                if (updateDto.SendMailAgain)
                {
                    var emails = arizaModel.Firms
                        .SelectMany(x => x.Emails)
                        .Select(x => x.EmailAddress)
                        .Distinct().ToList();

                    await _mailSenderService.SendEmailAsync(emails, arizaModel.MailSubject, arizaModel);
                }
                
                DbContext.ArizaModels.Update(arizaModel);
                await DbContext.SaveChangesAsync();
                return RedirectToAction("GetNotifications");
            }

            return View(updateDto);
        }

        [HttpGet]
        [Authorize(Roles = RoleTypes.AdminEditor)]
        public async Task<IActionResult> DeleteConfirmArizaNotification(int id)
        {
            var arizaModel = await DbContext.ArizaModels.FindAsync(id);
            
            if(arizaModel == null) return RedirectToAction("GetNotifications");

            return View(arizaModel.Adapt<UpdateArizaNotificationDto>());
        }
        
        [HttpGet]
        [Authorize(Roles = RoleTypes.AdminEditor)]
        public async Task<IActionResult> DeleteArizaNotification(int id)
        {
            var arizaModel = await DbContext.ArizaModels.FindAsync(id);

            if (arizaModel == null)
                return RedirectToAction("GetNotifications");
            
            DbContext.ArizaModels.Remove(arizaModel);
            await DbContext.SaveChangesAsync();
            return RedirectToAction("GetNotifications");
        }
    }
}