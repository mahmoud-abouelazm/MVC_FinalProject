using Library.Web.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Library.Web.Configurations
{
    public class BookConfiguration : IEntityTypeConfiguration<Book>
    {
        public void Configure(EntityTypeBuilder<Book> builder)
        {

            builder.HasKey(b => b.Id);

            builder.Property(b => b.Title)
                .IsRequired()
                .HasMaxLength(300);

            builder.Property(b => b.Description)
                .HasMaxLength(2000);

            builder.Property(b => b.Img)
                .HasMaxLength(500);

            builder.Property(b => b.Price)
                .HasColumnType("decimal(18,2)")
                .IsRequired();

            builder.Property(b => b.IsDeleted)
                .IsRequired()
                .HasDefaultValue(false);

            // One-to-Many: Category - Books
            builder.HasOne(b => b.Category)
                .WithMany(c => c.Books)
                .HasForeignKey(b => b.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
