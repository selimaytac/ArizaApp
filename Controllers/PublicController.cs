﻿using System.Linq;
using ArizaApp.Models.DbContexts;
using Microsoft.AspNetCore.Mvc;

namespace ArizaApp.Controllers
{
    public class PublicController : Controller
    {
        private readonly ArizaDbContext _dbContext;

        public PublicController(ArizaDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IActionResult AllNotifications()
        {
            ViewBag.ArizaNotfList = _dbContext.ArizaModels.Where(x => x.FaultType == "Arıza").OrderByDescending(x => x.CreatedDate).ToList();
            ViewBag.PlanliNotList = _dbContext.ArizaModels.Where(x => x.FaultType == "Planlı Çalışma").OrderByDescending(x => x.CreatedDate).ToList();
            
            return View();
        }

        public IActionResult NotificationDetail(int id)
        {
            var result = _dbContext.ArizaModels.FirstOrDefault(x => x.Id == id);
            
            return View(result);
        }

        public IActionResult GetNotificationList(string notfType)
        {
            var result = _dbContext.ArizaModels.Where(x => x.FaultType == notfType).OrderByDescending(x => x.CreatedDate).ToList();

            return View(result);
        }
    }
}