using Library.Web.Core.Models;
using Library.Web.Core.Constants;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;

namespace Library.Web.Data
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext<ApplicationUser, IdentityRole<int>, int>(options)
    {

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            
            builder.Entity<Book>()
                .HasQueryFilter(b => !b.IsDeleted);


            builder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);

            builder.Entity<IdentityRole<int>>().HasData(
                new IdentityRole<int> { Id = 1, Name = "Admin", NormalizedName = "ADMIN", ConcurrencyStamp = "0d4e27db-c6b4-4b22-b17e-8dc6c81cb101" },
                new IdentityRole<int> { Id = 2, Name = "User", NormalizedName = "USER", ConcurrencyStamp = "0d4e27db-c6b4-4b22-b17e-8dc6c81cb102" }
            );

            builder.Entity<ApplicationUser>().HasData(
                new ApplicationUser { Id = 1, FullName = "Alice Johnson", UserName = "alice.johnson", NormalizedUserName = "ALICE.JOHNSON", Email = "alice.johnson@library.com", NormalizedEmail = "ALICE.JOHNSON@LIBRARY.COM", EmailConfirmed = true, PhoneNumber = "01000000001", PhoneNumberConfirmed = true, SecurityStamp = "f8e2a9a2-c93f-4dbf-a0f9-13f4739db001", ConcurrencyStamp = "7e9b7c8e-f8f5-4ec7-88e1-01fc8cb98001" },
                new ApplicationUser { Id = 2, FullName = "Bob Smith", UserName = "bob.smith", NormalizedUserName = "BOB.SMITH", Email = "bob.smith@library.com", NormalizedEmail = "BOB.SMITH@LIBRARY.COM", EmailConfirmed = true, PhoneNumber = "01000000002", PhoneNumberConfirmed = true, SecurityStamp = "f8e2a9a2-c93f-4dbf-a0f9-13f4739db002", ConcurrencyStamp = "7e9b7c8e-f8f5-4ec7-88e1-01fc8cb98002" },
                new ApplicationUser { Id = 3, FullName = "Carla Davis", UserName = "carla.davis", NormalizedUserName = "CARLA.DAVIS", Email = "carla.davis@library.com", NormalizedEmail = "CARLA.DAVIS@LIBRARY.COM", EmailConfirmed = true, PhoneNumber = "01000000003", PhoneNumberConfirmed = true, SecurityStamp = "f8e2a9a2-c93f-4dbf-a0f9-13f4739db003", ConcurrencyStamp = "7e9b7c8e-f8f5-4ec7-88e1-01fc8cb98003" },
                new ApplicationUser { Id = 4, FullName = "David Brown", UserName = "david.brown", NormalizedUserName = "DAVID.BROWN", Email = "david.brown@library.com", NormalizedEmail = "DAVID.BROWN@LIBRARY.COM", EmailConfirmed = true, PhoneNumber = "01000000004", PhoneNumberConfirmed = true, SecurityStamp = "f8e2a9a2-c93f-4dbf-a0f9-13f4739db004", ConcurrencyStamp = "7e9b7c8e-f8f5-4ec7-88e1-01fc8cb98004" },
                new ApplicationUser { Id = 5, FullName = "Eva Wilson", UserName = "eva.wilson", NormalizedUserName = "EVA.WILSON", Email = "eva.wilson@library.com", NormalizedEmail = "EVA.WILSON@LIBRARY.COM", EmailConfirmed = true, PhoneNumber = "01000000005", PhoneNumberConfirmed = true, SecurityStamp = "f8e2a9a2-c93f-4dbf-a0f9-13f4739db005", ConcurrencyStamp = "7e9b7c8e-f8f5-4ec7-88e1-01fc8cb98005" },
                new ApplicationUser { Id = 6, FullName = "Frank Miller", UserName = "frank.miller", NormalizedUserName = "FRANK.MILLER", Email = "frank.miller@library.com", NormalizedEmail = "FRANK.MILLER@LIBRARY.COM", EmailConfirmed = true, PhoneNumber = "01000000006", PhoneNumberConfirmed = true, SecurityStamp = "f8e2a9a2-c93f-4dbf-a0f9-13f4739db006", ConcurrencyStamp = "7e9b7c8e-f8f5-4ec7-88e1-01fc8cb98006" },
                new ApplicationUser { Id = 7, FullName = "Grace Lee", UserName = "grace.lee", NormalizedUserName = "GRACE.LEE", Email = "grace.lee@library.com", NormalizedEmail = "GRACE.LEE@LIBRARY.COM", EmailConfirmed = true, PhoneNumber = "01000000007", PhoneNumberConfirmed = true, SecurityStamp = "f8e2a9a2-c93f-4dbf-a0f9-13f4739db007", ConcurrencyStamp = "7e9b7c8e-f8f5-4ec7-88e1-01fc8cb98007" }
            );

            builder.Entity<Category>().HasData(
                new Category { Id = 1, Name = "Fiction" },
                new Category { Id = 2, Name = "Science" },
                new Category { Id = 3, Name = "History" },
                new Category { Id = 4, Name = "Technology" },
                new Category { Id = 5, Name = "Philosophy" },
                new Category { Id = 6, Name = "Biography" },
                new Category { Id = 7, Name = "Art" }
            );

            builder.Entity<Author>().HasData(
                new Author { Id = 1, Name = "Nora Allen", Bio = "Award-winning fiction writer." },
                new Author { Id = 2, Name = "Samir Hassan", Bio = "Researcher in applied sciences." },
                new Author { Id = 3, Name = "Linda Carter", Bio = "Historian focused on modern history." },
                new Author { Id = 4, Name = "Omar Farouk", Bio = "Software architect and technical author." },
                new Author { Id = 5, Name = "Hana Youssef", Bio = "Philosophy lecturer and essayist." },
                new Author { Id = 6, Name = "James Walker", Bio = "Biographer of notable leaders." },
                new Author { Id = 7, Name = "Mia Chen", Bio = "Art critic and curator." }
            );

            builder.Entity<Book>().HasData(
                new Book { Id = 1, Title = "The Silent Path", Description = "A contemporary fiction novel.", Img = "images/books/silent-path.jpg", Price = 120.00m, IsDeleted = false, CategoryId = 1 },
                new Book { Id = 2, Title = "Physics in Daily Life", Description = "Practical science concepts for everyone.", Img = "images/books/physics-daily-life.jpg", Price = 150.00m, IsDeleted = false, CategoryId = 2 },
                new Book { Id = 3, Title = "Echoes of the Past", Description = "A journey through important historical events.", Img = "images/books/echoes-past.jpg", Price = 135.00m, IsDeleted = false, CategoryId = 3 },
                new Book { Id = 4, Title = "Modern Web Engineering", Description = "Guide to building scalable web apps.", Img = "images/books/web-engineering.jpg", Price = 220.00m, IsDeleted = false, CategoryId = 4 },
                new Book { Id = 5, Title = "Questions of Meaning", Description = "An introduction to core philosophy topics.", Img = "images/books/questions-meaning.jpg", Price = 140.00m, IsDeleted = false, CategoryId = 5 },
                new Book { Id = 6, Title = "Life of a Pioneer", Description = "Biography of an influential innovator.", Img = "images/books/life-pioneer.jpg", Price = 160.00m, IsDeleted = false, CategoryId = 6 },
                new Book { Id = 7, Title = "Seeing Through Colors", Description = "Understanding art movements and styles.", Img = "images/books/seeing-colors.jpg", Price = 145.00m, IsDeleted = false, CategoryId = 7 }
            );

            builder.Entity<BookAuthor>().HasData(
                new BookAuthor { BookId = 1, AuthorId = 1 },
                new BookAuthor { BookId = 2, AuthorId = 2 },
                new BookAuthor { BookId = 3, AuthorId = 3 },
                new BookAuthor { BookId = 4, AuthorId = 4 },
                new BookAuthor { BookId = 5, AuthorId = 5 },
                new BookAuthor { BookId = 6, AuthorId = 6 },
                new BookAuthor { BookId = 7, AuthorId = 7 }
            );

            builder.Entity<Copy>().HasData(
                new Copy { Id = 1, Name = "Copy-001", AllowToRental = true, BookId = 1 },
                new Copy { Id = 2, Name = "Copy-002", AllowToRental = true, BookId = 1 },
                new Copy { Id = 3, Name = "Copy-003", AllowToRental = true, BookId = 1 },
                new Copy { Id = 4, Name = "Copy-004", AllowToRental = true, BookId = 1 },

                new Copy { Id = 5, Name = "Copy-005", AllowToRental = true, BookId = 2 },
                new Copy { Id = 6, Name = "Copy-006", AllowToRental = true, BookId = 2 },
                new Copy { Id = 7, Name = "Copy-007", AllowToRental = true, BookId = 2 },
                new Copy { Id = 8, Name = "Copy-008", AllowToRental = true, BookId = 2 },

                new Copy { Id = 9, Name = "Copy-009", AllowToRental = true, BookId = 3 },
                new Copy { Id = 10, Name = "Copy-010", AllowToRental = true, BookId = 3 },
                new Copy { Id = 11, Name = "Copy-011", AllowToRental = true, BookId = 3 },
                new Copy { Id = 12, Name = "Copy-012", AllowToRental = true, BookId = 3 },

                new Copy { Id = 13, Name = "Copy-013", AllowToRental = true, BookId = 4 },
                new Copy { Id = 14, Name = "Copy-014", AllowToRental = true, BookId = 4 },
                new Copy { Id = 15, Name = "Copy-015", AllowToRental = true, BookId = 4 },
                new Copy { Id = 16, Name = "Copy-016", AllowToRental = true, BookId = 4 },

                new Copy { Id = 17, Name = "Copy-017", AllowToRental = true, BookId = 5 },
                new Copy { Id = 18, Name = "Copy-018", AllowToRental = true, BookId = 5 },
                new Copy { Id = 19, Name = "Copy-019", AllowToRental = true, BookId = 5 },
                new Copy { Id = 20, Name = "Copy-020", AllowToRental = true, BookId = 5 },

                new Copy { Id = 21, Name = "Copy-021", AllowToRental = true, BookId = 6 },
                new Copy { Id = 22, Name = "Copy-022", AllowToRental = true, BookId = 6 },
                new Copy { Id = 23, Name = "Copy-023", AllowToRental = true, BookId = 6 },
                new Copy { Id = 24, Name = "Copy-024", AllowToRental = true, BookId = 6 },

                new Copy { Id = 25, Name = "Copy-025", AllowToRental = true, BookId = 7 },
                new Copy { Id = 26, Name = "Copy-026", AllowToRental = true, BookId = 7 },
                new Copy { Id = 27, Name = "Copy-027", AllowToRental = true, BookId = 7 },
                new Copy { Id = 28, Name = "Copy-028", AllowToRental = true, BookId = 7 }
            );

            builder.Entity<Rental>().HasData(
                new Rental { Id = 1, RentedAt = new DateTime(2026, 1, 1), DueAt = new DateTime(2026, 1, 8), ReturnedAt = new DateTime(2026, 1, 6), Amount = 25.00m, Status = RentalState.Returned, ApplicationUserId = 1 },
                new Rental { Id = 2, RentedAt = new DateTime(2026, 1, 3), DueAt = new DateTime(2026, 1, 10), ReturnedAt = null, Amount = 30.00m, Status = RentalState.Active, ApplicationUserId = 2 },
                new Rental { Id = 3, RentedAt = new DateTime(2026, 1, 5), DueAt = new DateTime(2026, 1, 12), ReturnedAt = null, Amount = 35.00m, Status = RentalState.Overdue, ApplicationUserId = 3 },
                new Rental { Id = 4, RentedAt = new DateTime(2026, 1, 7), DueAt = new DateTime(2026, 1, 14), ReturnedAt = new DateTime(2026, 1, 13), Amount = 28.00m, Status = RentalState.Returned, ApplicationUserId = 4 },
                new Rental { Id = 5, RentedAt = new DateTime(2026, 1, 9), DueAt = new DateTime(2026, 1, 16), ReturnedAt = null, Amount = 26.00m, Status = RentalState.Active, ApplicationUserId = 5 },
                new Rental { Id = 6, RentedAt = new DateTime(2026, 1, 11), DueAt = new DateTime(2026, 1, 18), ReturnedAt = new DateTime(2026, 1, 17), Amount = 32.00m, Status = RentalState.Returned, ApplicationUserId = 6 },
                new Rental { Id = 7, RentedAt = new DateTime(2026, 1, 13), DueAt = new DateTime(2026, 1, 20), ReturnedAt = null, Amount = 29.00m, Status = RentalState.Active, ApplicationUserId = 7 }
            );

            builder.Entity<CopyRental>().HasData(
                new CopyRental { CopyId = 1, RentalId = 1 },
                new CopyRental { CopyId = 2, RentalId = 2 },
                new CopyRental { CopyId = 3, RentalId = 3 },
                new CopyRental { CopyId = 4, RentalId = 4 },
                new CopyRental { CopyId = 5, RentalId = 5 },
                new CopyRental { CopyId = 6, RentalId = 6 },
                new CopyRental { CopyId = 7, RentalId = 7 }
            );

            builder.Entity<IdentityUserRole<int>>().HasData(
                new IdentityUserRole<int> { UserId = 1, RoleId = 1 },
                new IdentityUserRole<int> { UserId = 2, RoleId = 2 },
                new IdentityUserRole<int> { UserId = 3, RoleId = 2 },
                new IdentityUserRole<int> { UserId = 4, RoleId = 2 },
                new IdentityUserRole<int> { UserId = 5, RoleId = 2 },
                new IdentityUserRole<int> { UserId = 6, RoleId = 2 },
                new IdentityUserRole<int> { UserId = 7, RoleId = 2 }
            );

        }


        public DbSet<Book> Books { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Author> Authors { get; set; }
        public DbSet<BookAuthor> BookAuthors { get; set; }
        public DbSet<Copy> Copies { get; set; }
        public DbSet<Rental> Rentals { get; set; }
        public DbSet<CopyRental> CopyRentals { get; set; }










    }

   
    
}
