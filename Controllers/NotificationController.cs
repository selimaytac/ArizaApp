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
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ArizaApp.Controllers
{
    public class NotificationController : BaseController
    {
        private readonly IMailSenderService _mailSenderService;
        private readonly IFileUploadService _fileUploadService;

        public NotificationController(ArizaDbContext dbContext, IMailSenderService mailSenderService,
            UserManager<AppUser> userManager, IFileUploadService fileUploadService)
            : base(userManager, null, null, dbContext)
        {
            _mailSenderService = mailSenderService;
            _fileUploadService = fileUploadService;
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
            ViewBag.Firms = await DbContext.FirmRecords.OrderBy(f => f.FirmName).ToListAsync();

            return View();
        }

        [HttpPost]
        [Authorize(Roles = RoleTypes.AdminEditor)]
        [RequestSizeLimit(100_000_000)]
        public async Task<IActionResult> CreateArizaNotification(CreateArizaNotificationDto createDto)
        {
            if (ModelState.IsValid)
            {
                var notificationModel = createDto.Adapt<ArizaModel>();
                notificationModel.User = CurrentUser;
                notificationModel.UserId = CurrentUser.Id;
               
                var firms = await DbContext.FirmRecords
                        .Include(x => x.Emails)
                        .Where(x => createDto.FirmIdS.Contains(x.Id))
                        .ToListAsync();

                notificationModel.Firms = firms;

                // Add record notification to db before sending mail
                
                await DbContext.AddAsync(notificationModel);
                await DbContext.SaveChangesAsync();

                if(createDto.UploadedFiles is { Count: > 0 })
                {
                    var files = await _fileUploadService.UploadFileAsync(createDto.UploadedFiles, notificationModel.Id, CurrentUser);
                    notificationModel.UploadedFileRecords = files;
                }
                
                if (createDto.SendMail)
                {
                    var emails = firms
                        .SelectMany(x => x.Emails)
                        .Select(x => x.EmailAddress)
                        .Distinct().ToList();

                    await _mailSenderService.SendEmailAsync(emails, createDto.MailSubject, notificationModel);
                    CurrentUser.SendCount++;
                }
                
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
                var arizaModel = await DbContext.ArizaModels.Include(x => x.Firms)
                    .FirstOrDefaultAsync(f => f.Id == updateDto.Id);
                arizaModel = updateDto.Adapt(arizaModel);

                if (updateDto.SendMailAgain)
                {
                    var firms = await DbContext.FirmRecords
                        .Include(x => x.Emails)
                        .Where(x => arizaModel.Firms.Select(fi => fi.FirmName).Contains(x.FirmName))
                        .ToListAsync();
                    
                    var emails = firms
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

            if (arizaModel == null) return RedirectToAction("GetNotifications");

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