using System;
using System.Linq;
using System.Threading.Tasks;
using ArizaApp.Helpers;
using ArizaApp.Models.ConstTypes;
using ArizaApp.Models.DbContexts;
using ArizaApp.Models.Dtos;
using ArizaApp.Models.Entities;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ArizaApp.Controllers
{
    public class DepartmentController : BaseController
    {
        public DepartmentController(ArizaDbContext dbContext)
            : base(null, null, null, dbContext)
        {
        }

        [HttpGet]
        [Authorize(Roles = RoleTypes.AllRoles)]
        public async Task<IActionResult> GetDepartments()
        {
            var departments = await DbContext.Departments.AsNoTracking().ToListAsync();

            return View(departments);
        }

        [HttpGet]
        [Authorize(Roles = RoleTypes.AdminEditor)]
        public IActionResult CreateDepartment()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = RoleTypes.AdminEditor)]
        public async Task<IActionResult> CreateDepartment(CreateDepartmentDto createDepartmentDto)
        {
            if (ModelState.IsValid)
            {
                var departmanExist =
                    await DbContext.Departments.AnyAsync(x => x.DepartmentName == createDepartmentDto.DepartmentName);
                if (departmanExist)
                {
                    ModelState.AddModelError("Name", "Bu departman zaten mevcut!");
                    return View(createDepartmentDto);
                }

                var department = createDepartmentDto.Adapt<Department>();

                await DbContext.Departments.AddAsync(department);
                await DbContext.SaveChangesAsync();
                
                var message = $"Department Created: {department.DepartmentName}";
                GeneralLogger.AddLog(DbContext, new LogRecord{ IpAddress = RequestHelper.GetIpAddress(Request), Date = DateTime.Now, LogType = LogTypes.CreateDepartment.ToString(), Message = message, Port = RequestHelper.GetPort(Request), UserName = CurrentUser.UserName});

                return RedirectToAction("GetDepartments");
            }

            ModelState.AddModelError("", "Departman eklenemedi lütfen tekrar deneyiniz.");
            return View(createDepartmentDto);
        }

        [HttpGet]
        [Authorize(Roles = RoleTypes.AdminEditor)]
        public async Task<IActionResult> UpdateDepartment(int id)
        {
            var deparment = await DbContext.Departments.FindAsync(id);

            if (deparment == null) return RedirectToAction("GetDepartments");

            var departmentDto = deparment.Adapt<UpdateDepartmentDto>();

            return View(departmentDto);
        }

        [HttpPost]
        [Authorize(Roles = RoleTypes.AdminEditor)]
        public async Task<IActionResult> UpdateDepartment(UpdateDepartmentDto department)
        {
            if (ModelState.IsValid)
            {
                var departmanExist =
                    await DbContext.Departments.AnyAsync(x => x.DepartmentName == department.DepartmentName);
                if (departmanExist)
                {
                    ModelState.AddModelError("Name", "Bu departman zaten mevcut!");
                    return View(department);
                }

                var deparment = await DbContext.Departments.FindAsync(department.Id);

                if (deparment == null) return RedirectToAction("GetDepartments");

                deparment.DepartmentName = department.DepartmentName;
                deparment.Description = department.Description;

                await DbContext.SaveChangesAsync();
                var message = $"Department Updated: {department.DepartmentName}";
                GeneralLogger.AddLog(DbContext, new LogRecord{ IpAddress = RequestHelper.GetIpAddress(Request), Date = DateTime.Now, LogType = LogTypes.UpdateDepartment.ToString(), Message = message, Port = RequestHelper.GetPort(Request), UserName = CurrentUser.UserName});

                return RedirectToAction("GetDepartments");
            }

            ModelState.AddModelError("", "Departman güncellenemedi lütfen tekrar deneyiniz.");
            return View(department);
        }

        [HttpGet]
        [Authorize(Roles = RoleTypes.AdminEditor)]
        public async Task<IActionResult> DeleteConfirmDepartment(int id)
        {
            var department = await DbContext.Departments.Include(u => u.Users).FirstOrDefaultAsync(d => d.Id == id);

            if (department == null) return RedirectToAction("GetDepartments");

            return View(department);
        }
        
        [HttpGet]
        [Authorize(Roles = RoleTypes.AdminEditor)]
        public async Task<IActionResult> DeleteDepartment(int id)
        {
            var department = await DbContext.Departments.Include(u => u.Users).FirstOrDefaultAsync(d => d.Id == id);

            if (department == null) return RedirectToAction("GetDepartments");
            
            if(department.Users.Any())
            {
                ModelState.AddModelError("", "Departmanda bulunan kullanıcıları lütfen başka departmana taşıyınız.");
                return View("DeleteConfirmDepartment",department);
            }

            DbContext.Departments.Remove(department);
            await DbContext.SaveChangesAsync();

            var message = $"Department Deleted: {department.DepartmentName}";
            GeneralLogger.AddLog(DbContext, new LogRecord{ IpAddress = RequestHelper.GetIpAddress(Request), Date = DateTime.Now, LogType = LogTypes.DeleteDepartment.ToString(), Message = message, Port = RequestHelper.GetPort(Request), UserName = CurrentUser.UserName});
            
            return RedirectToAction("GetDepartments");
        }
    }
}