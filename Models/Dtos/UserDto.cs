using ArizaApp.Models.Entities;

namespace ArizaApp.Models.Dtos
{
    public class UserDto
    {
      
        public string UserName { get; set; }

        public string Email { get; set; }

        public string Name { get; set; }

        public string Surname { get; set; }

        public string DepartmentName { get; set; }

        public string Note { get; set; }

        public string RoleName { get; set; }
        
        public int SendCount { get; set; }
    }
}