using Library.Web.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Library.Web.Configurations
{
    public class ApplicationUserConfiguration : IEntityTypeConfiguration<ApplicationUser>
    {
        public void Configure(EntityTypeBuilder<ApplicationUser> builder)
        {
            //builder.Property(u=>u.Id).ValueGeneratedOnAdd();
            
            builder.Property(u => u.FullName)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(u => u.PhoneNumber)
                .HasMaxLength(20);

           
        }
    }

}
