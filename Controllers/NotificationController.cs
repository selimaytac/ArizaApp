using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ArizaApp.Helpers;
using ArizaApp.Migrations;
using ArizaApp.Models.ConstTypes;
using ArizaApp.Models.DbContexts;
using ArizaApp.Models.Dtos;
using ArizaApp.Models.Entities;
using ArizaApp.Models.Options;
using ArizaApp.Services.Interfaces;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
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
            var notifications = await DbContext.ArizaModels.OrderByDescending(x => x.CreatedDate).ToListAsync();

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
        public async Task<IActionResult> CreateArizaNotification(CreateArizaNotificationDto createDto, IList<IFormFile> attachments)
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

                notificationModel.Firms = firms ?? new List<FirmRecord>();

                // Add record notification to db before sending mail
                
                await DbContext.AddAsync(notificationModel);
                await DbContext.SaveChangesAsync();

                if(attachments is { Count: > 0 })
                {
                    var files = await _fileUploadService.UploadFileAsync(attachments, notificationModel.Id, CurrentUser);
                    notificationModel.UploadedFileRecords = files;
                }
                
                if (createDto.SendMail)
                {
                    var emails = firms
                        .SelectMany(x => x.Emails)
                        .Select(x => x.EmailAddress)
                        .Distinct().ToList();

                    await _mailSenderService.SendEmailAsync(emails, createDto.MailSubject, notificationModel, attachments);
                    CurrentUser.SendCount++;
                    var message = $"Notification Created with subject: {createDto.MailSubject}";
                    GeneralLogger.AddLog(DbContext, new LogRecord{ IpAddress = RequestHelper.GetIpAddress(Request), Date = DateTime.Now, LogType = LogTypes.CreateNotification.ToString(), Message = message, Port = RequestHelper.GetPort(Request), UserName = CurrentUser.UserName});
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
            var notificationModel = await DbContext.ArizaModels
                .Include(x => x.UploadedFileRecords)
                .Include(x => x.Firms)
                .FirstOrDefaultAsync(x => x.Id == id);
            
            ViewBag.AllFirms = 
                await DbContext.FirmRecords
                    .Where(x => !notificationModel.Firms.Select(f => f.Id).Contains(x.Id))
                    .OrderBy(f => f.FirmName)
                    .ToListAsync();
            
            return View(notificationModel.Adapt<UpdateArizaNotificationDto>());
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

                var message = $"{updateDto.MailSubject} Notification Updated";
                GeneralLogger.AddLog(DbContext, new LogRecord{ IpAddress = RequestHelper.GetIpAddress(Request), Date = DateTime.Now, LogType = LogTypes.UpdateNotification.ToString(), Message = message, Port = RequestHelper.GetPort(Request), UserName = CurrentUser.UserName});
                
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
            var notificationModel = await DbContext.ArizaModels.FindAsync(id);

            if (notificationModel == null)
                return RedirectToAction("GetNotifications");

            DbContext.ArizaModels.Remove(notificationModel);
            await DbContext.SaveChangesAsync();
            
            var message = $"{notificationModel.MailSubject} Notification deleted";
            GeneralLogger.AddLog(DbContext, new LogRecord{ IpAddress = RequestHelper.GetIpAddress(Request), Date = DateTime.Now, LogType = LogTypes.DeleteNotification.ToString(), Message = message, Port = RequestHelper.GetPort(Request), UserName = CurrentUser.UserName});
            
            return RedirectToAction("GetNotifications");
        }

        [HttpGet]
        [Authorize(Roles = RoleTypes.AllRoles)]
        public async Task<IActionResult> GetNotificationDetails(int id)
        {
            var notificationModel = await DbContext.ArizaModels
                .Include(x => x.UploadedFileRecords)
                .Include(x => x.Firms)
                .FirstOrDefaultAsync(x => x.Id == id);

            ViewBag.Emails = await DbContext.FirmRecords
                    .Include(x => x.Emails)
                    .Where(x => notificationModel.Firms.Select(f => f.FirmName).Contains(x.FirmName))
                    .SelectMany(x => x.Emails)
                    .Select(x => x.EmailAddress)
                    .Distinct()
                    .ToListAsync();

            return View(notificationModel);
        }
        
        [HttpGet]
        [Authorize(Roles = RoleTypes.AllRoles)]
        public async Task<IActionResult> DownloadFile(int id)
        {
            var file = await DbContext.UploadedFileRecords.FindAsync(id);
            var fileBytes = await System.IO.File.ReadAllBytesAsync(file.FilePath);
            
            var fileProvider = new FileExtensionContentTypeProvider();
            
            fileProvider.TryGetContentType(file.FilePath, out var contentType);
            
            return File(fileBytes, contentType ?? "application/json", file.FileName);
            
        }
    }
}