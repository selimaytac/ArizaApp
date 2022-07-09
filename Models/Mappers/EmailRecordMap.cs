using ArizaApp.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ArizaApp.Models.Mappers
{
    public class EmailRecordMap : IEntityTypeConfiguration<EmailRecord>
    {
        public void Configure(EntityTypeBuilder<EmailRecord> builder)
        {
            builder.HasKey(e => e.Id);
            builder.Property(e => e.Id).ValueGeneratedOnAdd();
            builder.Property(e => e.EmailAddress).HasMaxLength(100).IsRequired();
            builder.Property(e => e.IsActive).IsRequired();
        }
    }
}