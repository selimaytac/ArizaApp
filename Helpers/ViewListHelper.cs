using System.Collections.Generic;
using System.Linq;
using ArizaApp.Models.DbContexts;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace ArizaApp.Helpers
{
    public class ViewListHelper
    {
        public static IList<SelectListItem> GetRoles(ArizaDbContext dbContext)
        {
            return dbContext.Roles.AsNoTracking().Select(role => new SelectListItem {Text = role.Name, Value = role.Id})
                .ToList();
        }

        public static IList<SelectListItem> GetDepartments(ArizaDbContext dbContext)
        {
            return dbContext.Departments.AsNoTracking().Select(department => new SelectListItem
                {Text = department.DepartmentName, Value = $"{department.Id}"}).ToList();
        }
    }
}