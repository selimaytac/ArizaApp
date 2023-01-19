using System;
using ArizaApp.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ArizaApp.Models.Mappers
{
    public class ArizaModelMap : IEntityTypeConfiguration<ArizaModel>
    {
        public void Configure(EntityTypeBuilder<ArizaModel> builder)
        {
            builder.HasKey(a => a.Id);
            builder.Property(a => a.Id).ValueGeneratedOnAdd();
            builder.Property(a => a.NotifiedBy).HasMaxLength(150).IsRequired();
            builder.Property(a => a.MailSubject).HasMaxLength(200).IsRequired();
            builder.Property(a => a.FaultType).HasMaxLength(100).IsRequired();
            builder.Property(a => a.State).HasMaxLength(50).IsRequired();
            builder.Property(a => a.Description).HasMaxLength(700).IsRequired();
            builder.Property(a => a.StartDate).HasMaxLength(70).IsRequired();
            builder.Property(a => a.EndDate).HasMaxLength(70);
            builder.Property(a => a.Priority).HasMaxLength(50).IsRequired();
            builder.Property(a => a.FailureCause).HasMaxLength(300).IsRequired();
            builder.Property(a => a.AlarmStatus).IsRequired();
            builder.Property(a => a.AffectedFirms).HasMaxLength(700).IsRequired();
            builder.Property(a => a.AffectedServices).HasMaxLength(700).IsRequired();
            builder.Property(a => a.ApprovedBy).HasMaxLength(100);
            builder.Property(a => a.SendMail).IsRequired();
            builder.Property(a => a.CreatedDate).IsRequired().HasDefaultValueSql("getdate()");
            builder.HasOne<AppUser>(u => u.User).WithMany(r => r.Arizalar).HasForeignKey(u => u.UserId);
            builder.HasMany<FirmRecord>(er => er.Firms).WithMany(fr => fr.ArizaModels);
        }
    }
}