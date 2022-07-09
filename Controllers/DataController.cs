using System.Linq;
using System.Threading.Tasks;
using ArizaApp.Models.DbContexts;
using ArizaApp.Models.Dtos;
using ArizaApp.Models.Entities;
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
        public async Task<IActionResult> GetEmails()
        {
            var emails = await DbContext.EmailRecords.AsNoTracking().ToListAsync();

            return View(emails);
        }

        [HttpGet]
        public async Task<IActionResult> CreateEmail()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateEmail(CreateEmailDto createEmailDto)
        {
            if (ModelState.IsValid)
            {
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
        public async Task<IActionResult> GetFirms()
        {
            var firms = await DbContext.FirmRecords.Include(f => f.Emails).AsNoTracking().ToListAsync();

            return View(firms);
        }

        [HttpGet]
        public async Task<IActionResult> CreateFirm()
        {
            var emails = await DbContext.EmailRecords.AsNoTracking().ToListAsync();

            if (emails.Count != 0)
            {
                ViewBag.Emails = emails;
            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateFirm(CreateFirmDto createFirmDto)
        {
            if (ModelState.IsValid)
            {
                // get all emails with id from createFirmDto.EmailIdS
                var emails = await DbContext.EmailRecords.Where(x => createFirmDto.EmailIds.Contains(x.Id))
                    .ToListAsync();

                DbContext.Add(new FirmRecord
                {
                    FirmName = createFirmDto.FirmName,
                    Emails = emails
                });

                await DbContext.SaveChangesAsync();
                return RedirectToAction("GetFirms");
            }

            ModelState.AddModelError("", "Bir problem oluştu, lütfen daha sonra tekrar deneyiniz.");
            return View(createFirmDto);
        }
    }
}