using ArizaApp.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ArizaApp.Models.Mappers
{
    public class LogRecordMap : IEntityTypeConfiguration<LogRecord>
    {
        public void Configure(EntityTypeBuilder<LogRecord> builder)
        {
            builder.HasKey(d => d.Id);
            builder.Property(d => d.Id).ValueGeneratedOnAdd();
            builder.Property(d => d.UserName).IsRequired().HasMaxLength(100);
            builder.Property(d => d.LogType).IsRequired().HasMaxLength(100);
            builder.Property(d => d.Message).HasMaxLength(200);
            builder.Property(d => d.IpAddress).HasMaxLength(20);
            builder.Property(d => d.Port).HasMaxLength(8);
            builder.Property(d => d.Date).IsRequired().HasDefaultValueSql("getdate()");
        }
    }
}