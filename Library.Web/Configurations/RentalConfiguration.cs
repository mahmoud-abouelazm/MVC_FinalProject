using Library.Web.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Library.Web.Configurations
{
    public class RentalConfiguration : IEntityTypeConfiguration<Rental>
    {
        public void Configure(EntityTypeBuilder<Rental> builder)
        {

            builder.HasKey(r => r.Id);

            builder.Property(r => r.RentedAt)
                .IsRequired();

            builder.Property(r => r.DueAt)
                .IsRequired();

            builder.Property(r => r.ReturnedAt)
                .IsRequired(false);

            // Amount is a derived attribute (dashed ellipse in diagram)
            builder.Property(r => r.Amount)
                .HasColumnType("decimal(18,2)")
                .IsRequired();

            builder.Property(r => r.Status)
                .IsRequired()
                .HasMaxLength(50);

            // Many-to-One: Rental - ApplicationUser (Customer)
            builder.HasOne(r => r.ApplicationUser)
                .WithMany(u => u.Rentals)
                .HasForeignKey(r => r.ApplicationUserId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}