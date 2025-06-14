using Homeoffice.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Homeoffice.DataAccess.Data.Config
{
    public class HomeOfficeEntryConfiguration : IEntityTypeConfiguration<HomeOfficeEntry>
    {
        public void Configure(EntityTypeBuilder<HomeOfficeEntry> builder)
        {
            builder.Property(e => e.StartTime)
                .IsRequired()
                .HasColumnType("datetimeoffset");

            builder.Property(e => e.EndTime)
                .IsRequired(false)
                .HasColumnType("datetimeoffset");

            builder.Property(e => e.Description)
                .IsRequired(false)
                .HasMaxLength(500);

            builder.Property(e => e.IsEmailSent)
                .IsRequired()
                .HasDefaultValue(false);

            builder.HasOne(e => e.User).WithMany(u => u.HomeOfficeEntries)
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(e => e.UserId);
        }
    }
}
