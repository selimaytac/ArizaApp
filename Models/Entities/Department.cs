using System.Collections;
using System.Collections.Generic;

namespace ArizaApp.Models.Entities
{
    public class Department
    {
        public int Id { get; set; }
        public string DepartmentName { get; set; }
        public string Description { get; set; }
        public IEnumerable<AppUser> Users { get; set; }
    }
}