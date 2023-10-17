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
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace ArizaApp.Controllers
{
    public class FirmController : BaseController
    {
        public FirmController(ArizaDbContext dbContext)
            : base(null, null, null, dbContext)
        {
        }

        [HttpGet]
        [Authorize(Roles = RoleTypes.AllRoles)]
        public async Task<IActionResult> GetFirms()
        {
            var firms = await DbContext.FirmRecords.Include(f => f.Emails).AsNoTracking().ToListAsync();

            return View(firms);
        }

        [HttpGet]
        [Authorize(Roles = RoleTypes.AdminEditor)]
        public async Task<IActionResult> CreateFirm()
        {
            var emails = await DbContext.EmailRecords.AsNoTracking().OrderBy(e => e.EmailAddress).ToListAsync();

            if (emails.Count != 0) ViewBag.Emails = emails;

            return View();
        }

        [HttpPost]
        [Authorize(Roles = RoleTypes.AdminEditor)]
        public async Task<IActionResult> CreateFirm(CreateFirmDto createFirmDto)
        {
            if (ModelState.IsValid)
            {
                var firm = await DbContext.FirmRecords.FirstOrDefaultAsync(f => f.FirmName == createFirmDto.FirmName);
                if (firm != null)
                {
                    ModelState.AddModelError("", "Firma zaten mevcut.");
                    return View(createFirmDto);
                }

                var emails = new List<EmailRecord>();

                if (createFirmDto.EmailIds != null)
                    emails = await DbContext.EmailRecords.Where(x => createFirmDto.EmailIds.Contains(x.Id))
                        .ToListAsync();

                DbContext.Add(new FirmRecord
                {
                    FirmName = createFirmDto.FirmName,
                    Emails = emails
                });

                await DbContext.SaveChangesAsync();
                
                var message = $"Firm Created: {createFirmDto.FirmName}";
                GeneralLogger.AddLog(DbContext, new LogRecord{ IpAddress = RequestHelper.GetIpAddress(Request), Date = DateTime.Now, LogType = LogTypes.FirmCreated.ToString(), Message = message, Port = RequestHelper.GetPort(Request), UserName = CurrentUser.UserName});

                return RedirectToAction("GetFirms");
            }

            ViewBag.Emails = await DbContext.EmailRecords.AsNoTracking().OrderBy(e => e.EmailAddress).ToListAsync();
            ModelState.AddModelError("", "Bir problem oluştu, lütfen daha sonra tekrar deneyiniz.");
            return View(createFirmDto);
        }

        [HttpGet]
        [Authorize(Roles = RoleTypes.AdminEditor)]
        public async Task<IActionResult> UpdateFirm(int id)
        {
            var firm = await DbContext.FirmRecords.Include(f => f.Emails).FirstOrDefaultAsync(x => x.Id == id);

            if (firm == null) return RedirectToAction("GetFirms");

            var firmDto = firm.Adapt<UpdateFirmDto>();

            var emails = await DbContext.EmailRecords.AsNoTracking()
                .ToListAsync();

            var canAddToFirm = emails.Where(e => !firm.Emails.Select(x => x.Id).Contains(e.Id)).OrderBy(e => e.EmailAddress).ToList();
            if (canAddToFirm.Count != 0)
                ViewBag.NotIncludedEmailsMultiSelectList = new MultiSelectList(canAddToFirm, "Id", "EmailAddress");

            var canDeleteFromFirm = firm.Emails.OrderBy(e => e.EmailAddress).ToList();
            if (canDeleteFromFirm.Count != 0)
                ViewBag.CurrentEmailsMultiSelectList = new MultiSelectList(canDeleteFromFirm, "Id", "EmailAddress");

            return View(firmDto);
        }

        [HttpPost]
        [Authorize(Roles = RoleTypes.AdminEditor)]
        public async Task<IActionResult> UpdateFirm(UpdateFirmDto updateFirmDto)
        {
            if (ModelState.IsValid)
            {
                var firm = await DbContext.FirmRecords.Include(f => f.Emails)
                    .FirstOrDefaultAsync(x => x.Id == updateFirmDto.Id);

                if (firm == null) return RedirectToAction("GetFirms");

                var firmName =
                    await DbContext.FirmRecords.FirstOrDefaultAsync(x => x.FirmName == updateFirmDto.FirmName);
                if (firmName != null && firmName.Id != firm.Id)
                {
                    ModelState.AddModelError("", "Firma zaten mevcut.");
                    return View(updateFirmDto);
                }

                firm.FirmName = updateFirmDto.FirmName;

                var updatedFirmEmails = firm.Emails.ToList();

                if (updateFirmDto.AddedEmailIds != null)
                    updatedFirmEmails
                        .AddRange(DbContext.EmailRecords.Where(x => updateFirmDto.AddedEmailIds.Contains(x.Id)));

                if (updateFirmDto.DeletedEmailIds != null)
                    updatedFirmEmails.RemoveAll(x => updateFirmDto.DeletedEmailIds.Contains(x.Id));

                firm.Emails = updatedFirmEmails;

                await DbContext.SaveChangesAsync();
                
                var message = $"Firm Updated: {updateFirmDto.FirmName}";
                GeneralLogger.AddLog(DbContext, new LogRecord{ IpAddress = RequestHelper.GetIpAddress(Request), Date = DateTime.Now, LogType = LogTypes.FirmUpdated.ToString(), Message = message, Port = RequestHelper.GetPort(Request), UserName = CurrentUser.UserName});
                
                return RedirectToAction("GetFirms");
            }

            ViewBag.Emails = await DbContext.EmailRecords.AsNoTracking().ToListAsync();
            ModelState.AddModelError("", "Bir problem oluştu, lütfen daha sonra tekrar deneyiniz.");
            return View(updateFirmDto);
        }

        [HttpGet]
        [Authorize(Roles = RoleTypes.AdminEditor)]
        public async Task<IActionResult> DeleteConfirmFirm(int id)
        {
            var firm = await DbContext.FirmRecords.Include(f => f.Emails).FirstOrDefaultAsync(x => x.Id == id);

            if (firm == null) return RedirectToAction("GetFirms");

            var firmDto = firm.Adapt<UpdateFirmDto>();

            ViewBag.Emails = firm.Emails.Select(e => e.EmailAddress);

            return View(firmDto);
        }

        [HttpGet]
        [Authorize(Roles = RoleTypes.AdminEditor)]
        public async Task<IActionResult> DeleteFirm(int id)
        {
            var firm = await DbContext.FirmRecords.FirstOrDefaultAsync(x => x.Id == id);

            if (firm == null) return RedirectToAction("GetFirms");

            DbContext.FirmRecords.Remove(firm);
            await DbContext.SaveChangesAsync();

            var message = $"Firm Deleted: {firm.FirmName}";
            GeneralLogger.AddLog(DbContext, new LogRecord{ IpAddress = RequestHelper.GetIpAddress(Request), Date = DateTime.Now, LogType = LogTypes.FirmDeleted.ToString(), Message = message, Port = RequestHelper.GetPort(Request), UserName = CurrentUser.UserName});

            return RedirectToAction("GetFirms");
        }
    }
}