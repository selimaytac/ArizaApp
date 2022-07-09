using ArizaApp.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ArizaApp.Models.Mappers
{
    public class DepartmentMap : IEntityTypeConfiguration<Department>
    {
        public void Configure(EntityTypeBuilder<Department> builder)
        {
            builder.HasKey(d => d.Id);
            builder.Property(d => d.Id).ValueGeneratedOnAdd();
            builder.Property(d => d.DepartmentName).IsRequired().HasMaxLength(70);
            builder.Property(d => d.Description).HasMaxLength(100);
            builder.HasMany<AppUser>(u => u.Users).WithOne(d => d.Department).HasForeignKey(u => u.DepartmentId);
        }
    }
}