using Library.Web.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Library.Web.Configurations
{
    public class CopyRentalConfiguration : IEntityTypeConfiguration<CopyRental>
    {
        public void Configure(EntityTypeBuilder<CopyRental> builder)
        {
            builder.ToTable("CopyRentals");

            builder.HasKey(cr => new { cr.CopyId, cr.RentalId });

            builder.HasOne(cr => cr.Copy)
                .WithMany(c => c.CopyRentals)
                .HasForeignKey(cr => cr.CopyId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(cr => cr.Rental)
                .WithMany(r => r.CopyRentals)
                .HasForeignKey(cr => cr.RentalId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}