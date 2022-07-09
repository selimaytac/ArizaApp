using ArizaApp.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ArizaApp.Models.Mappers
{
    public class FirmRecordMap : IEntityTypeConfiguration<FirmRecord>
    {
        public void Configure(EntityTypeBuilder<FirmRecord> builder)
        {
            builder.HasKey(d => d.Id);
            builder.Property(d => d.Id).ValueGeneratedOnAdd();
            builder.Property(d => d.FirmName).IsRequired().HasMaxLength(100);
            builder.HasMany<EmailRecord>(er => er.Emails).WithMany(fr => fr.Firms);
        }
    }
}