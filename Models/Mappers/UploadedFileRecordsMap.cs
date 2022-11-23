using System;
using ArizaApp.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ArizaApp.Models.Mappers
{
    public class UploadedFileRecordsMap : IEntityTypeConfiguration<UploadedFileRecords>
    {
        public void Configure(EntityTypeBuilder<UploadedFileRecords> builder)
        {
            builder.HasKey(f => f.Id);
            builder.Property(f => f.Id).ValueGeneratedOnAdd();
            builder.Property(f => f.FileName).IsRequired().HasMaxLength(250);
            builder.Property(f => f.FilePath).IsRequired().HasMaxLength(300);
            builder.Property(f => f.FileExtension).IsRequired().HasMaxLength(50);
            builder.Property(f => f.UploadDate).IsRequired().HasDefaultValue(DateTime.Now);
            builder.Property(f => f.FileSize).IsRequired();
            builder.HasOne<AppUser>(f => f.User).WithMany(f => f.UploadedFileRecords).HasForeignKey(f => f.UserId);
            builder.HasOne<ArizaModel>(f => f.Notification).WithMany(f => f.UploadedFileRecords).HasForeignKey(f => f.NotificationId);
            builder.ToTable("UploadedFiles");
        }
    }
}