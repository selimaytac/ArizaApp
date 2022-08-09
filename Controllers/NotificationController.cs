using System.Linq;
using ArizaApp.Models.ConstTypes;
using ArizaApp.Models.DbContexts;
using ArizaApp.Models.Entities;
using ArizaApp.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ArizaApp.Controllers
{
    public class NotificationController : BaseController
    {
        private readonly IMailSenderService _mailSenderService;
        
        public NotificationController(ArizaDbContext dbContext, IMailSenderService mailSenderService)
            : base(null, null, null, dbContext)
        {
            _mailSenderService = mailSenderService;
        }
        
        [HttpGet]
        [Authorize(Roles = RoleTypes.AllRoles)]
        public IActionResult GetNotifications()
        {
            var notifications = DbContext.ArizaModels.ToList();
            
            return View(notifications);
        }

        [HttpGet]
        [Authorize(Roles = RoleTypes.AdminEditor)]
        public IActionResult CreateArizaNotification()
        {
            return View();
        }
    }
}