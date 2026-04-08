using Library.Web.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Library.Web.Configurations
{
    public class CopyConfiguration : IEntityTypeConfiguration<Copy>
    {
        public void Configure(EntityTypeBuilder<Copy> builder)
        {

            builder.HasKey(c => c.Id);

            builder.Property(c => c.Name)
                .IsRequired()
                .HasMaxLength(300);

            builder.Property(c => c.AllowToRental)
                .IsRequired()
                .HasDefaultValue(true);

            // Many-to-One: Copy - Book
            builder.HasOne(c => c.Book)
                .WithMany(b => b.Copies)
                .HasForeignKey(c => c.BookId)
                .OnDelete(DeleteBehavior.Restrict);

          
        }
    }
}