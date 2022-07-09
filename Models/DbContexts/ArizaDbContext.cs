using ArizaApp.Models.Entities;
using ArizaApp.Models.Mappers;
using ArizaApp.Models.Seed;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace ArizaApp.Models.DbContexts
{
    public class ArizaDbContext : IdentityDbContext<AppUser, AppRole, string>
    {

        // public DbSet<ArizaModel> ArizaModels { get; set; }
        public DbSet<Department> Departments { get; set; }
        public ArizaDbContext(DbContextOptions<ArizaDbContext> options) : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new DepartmentMap());
            base.OnModelCreating(modelBuilder);
        }
    }
}