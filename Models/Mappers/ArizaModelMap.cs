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
            builder.Property(a => a.NotifiedBy).HasMaxLength(50).IsRequired();
            builder.Property(a => a.MailSubject).HasMaxLength(100).IsRequired();
            builder.Property(a => a.FaultType).HasMaxLength(50).IsRequired();
            builder.Property(a => a.State).HasMaxLength(30).IsRequired();
            builder.Property(a => a.Description).HasMaxLength(500).IsRequired();
            builder.Property(a => a.StartDate).IsRequired().HasDefaultValue(DateTime.Now);
            builder.Property(a => a.Priority).HasMaxLength(30).IsRequired();
            builder.Property(a => a.FailureCause).HasMaxLength(30).IsRequired();
            builder.Property(a => a.AlarmStatus).IsRequired();
            builder.Property(a => a.AffectedFirms).HasMaxLength(500).IsRequired();
            builder.Property(a => a.AffectedServices).HasMaxLength(500).IsRequired();
            builder.Property(a => a.ApprovedBy).HasMaxLength(50);
            builder.Property(a => a.SendMail).IsRequired();
            builder.HasOne<AppUser>(u => u.User).WithMany(r => r.Arizalar).HasForeignKey(u => u.UserId);
        }
    }
}