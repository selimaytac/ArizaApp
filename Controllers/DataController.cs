using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
    public class DataController : BaseController
    {
        public DataController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager,
            RoleManager<AppRole> roleManager, ArizaDbContext dbContext)
            : base(userManager, null, roleManager, dbContext)
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
        public async Task<IActionResult> CreateEmail()
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

            return RedirectToAction("GetEmails");
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
            var emails = await DbContext.EmailRecords.AsNoTracking().ToListAsync();

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
                return RedirectToAction("GetFirms");
            }

            ViewBag.Emails = await DbContext.EmailRecords.AsNoTracking().ToListAsync();
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

            var canAddToFirm = emails.Where(e => !firm.Emails.Select(x => x.Id).Contains(e.Id)).ToList();
            if (canAddToFirm.Count != 0)
                ViewBag.NotIncludedEmailsMultiSelectList = new MultiSelectList(canAddToFirm, "Id", "EmailAddress");

            var canDeleteFromFirm = firm.Emails.ToList();
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

            ViewBag.Emails = firm.Emails;

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

            return RedirectToAction("GetFirms");
        }
    }
}