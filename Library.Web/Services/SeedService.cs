using Library.Web.Core.Constants;
using Library.Web.Core.Models;
using Library.Web.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Library.Web.Services
{
    public interface ISeedService
    {
        Task SeedDatabaseAsync();
    }

    public class SeedService : ISeedService
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole<int>> _roleManager;

        public SeedService(ApplicationDbContext context, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole<int>> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task SeedDatabaseAsync()
        {
            try
            {
                // Clear existing data
                await ClearDatabaseAsync();

                // Seed Roles
                await SeedRolesAsync();

                // Seed Categories
                await SeedCategoriesAsync();

                // Seed Authors
                await SeedAuthorsAsync();

                // Seed Books and BookAuthors
                await SeedBooksAsync();

                // Seed Copies
                await SeedCopiesAsync();

                // Seed Users
                await SeedUsersAsync();

                // Seed Rentals
                await SeedRentalsAsync();

                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error seeding database: {ex.Message}", ex);
            }
        }

        private async Task ClearDatabaseAsync()
        {
            // Clear in reverse dependency order
            _context.CopyRentals.RemoveRange(_context.CopyRentals);
            _context.Rentals.RemoveRange(_context.Rentals);
            _context.Copies.RemoveRange(_context.Copies);
            _context.BookAuthors.RemoveRange(_context.BookAuthors);
            _context.Books.RemoveRange(_context.Books);
            _context.Authors.RemoveRange(_context.Authors);
            _context.Categories.RemoveRange(_context.Categories);
            _context.Users.RemoveRange(_context.Users.Where(u => u.Id > 0));
            _context.UserRoles.RemoveRange(_context.UserRoles);
            await _context.SaveChangesAsync();
        }

        private async Task SeedRolesAsync()
        {
            var roles = new[] { "Admin", "User" };
            foreach (var role in roles)
            {
                if (!await _roleManager.RoleExistsAsync(role))
                {
                    await _roleManager.CreateAsync(new IdentityRole<int> { Name = role });
                }
            }
        }

        private async Task SeedCategoriesAsync()
        {
            var categories = new[]
            {
                new Category { Name = "Fiction" },
                new Category { Name = "Science" },
                new Category { Name = "History" },
                new Category { Name = "Technology" },
                new Category { Name = "Philosophy" }
            };

            foreach (var category in categories)
            {
                _context.Categories.Add(category);
            }
            await _context.SaveChangesAsync();
        }

        private async Task SeedAuthorsAsync()
        {
            var authors = new[]
            {
                new Author { Name = "Jim Stephens", Bio = "Award-winning fiction writer specializing in historical narratives." },
                new Author { Name = "Murat Uhrayoglu", Bio = "Physics educator and author of popular science books." },
                new Author { Name = "Cherag Shah", Bio = "Motivational author and spiritual guide." },
                new Author { Name = "Sarah Mitchell", Bio = "Renowned computer scientist and tech entrepreneur." },
                new Author { Name = "Dr. Michael Chen", Bio = "Philosopher and ethics professor." },
                new Author { Name = "Emma Watson", Bio = "Historian specializing in world civilizations." },
                new Author { Name = "James Patterson", Bio = "Bestselling fiction author and thriller writer." }
            };

            foreach (var author in authors)
            {
                _context.Authors.Add(author);
            }
            await _context.SaveChangesAsync();
        }

        private async Task SeedBooksAsync()
        {
            var categories = await _context.Categories.ToListAsync();
            var authors = await _context.Authors.ToListAsync();

            var bookData = new Dictionary<int, List<(string title, string description, decimal price, List<int> authorIds)>>
            {
                { 1, new List<(string, string, decimal, List<int>)> {
                    ("Echoes of the Past", "Unveiling History's Secrets - A journey through important historical events and their impact on modern society.", 135.00m, new List<int> { 1 }),
                    ("The Silent Path", "A contemporary fiction novel exploring the journey within - a tale of self-discovery and personal transformation.", 120.00m, new List<int> { 1 }),
                    ("The Lost Kingdom", "An epic adventure through ancient civilizations and forgotten worlds.", 145.00m, new List<int> { 1, 7 })
                }},
                { 2, new List<(string, string, decimal, List<int>)> {
                    ("Physics in Daily Life", "Simple College Physics (Electricity and Magnetism) - Practical science concepts explained for everyone.", 150.00m, new List<int> { 2 }),
                    ("Quantum Mysteries", "Understanding the quantum world and its applications in modern technology.", 165.00m, new List<int> { 2 }),
                    ("The Universe Explained", "A comprehensive guide to cosmology and astrophysics.", 155.00m, new List<int> { 2, 5 })
                }},
                { 3, new List<(string, string, decimal, List<int>)> {
                    ("Echoes of the Past", "A journey through important historical events - Unveiling History's Secrets.", 135.00m, new List<int> { 6 }),
                    ("Ancient Empires", "The rise and fall of the world's greatest civilizations.", 140.00m, new List<int> { 6 }),
                    ("Modern History Revisited", "Uncovering hidden stories from the 20th and 21st centuries.", 125.00m, new List<int> { 6 })
                }},
                { 4, new List<(string, string, decimal, List<int>)> {
                    ("Modern Web Engineering", "Guide to building scalable web applications using microservices.", 220.00m, new List<int> { 4 }),
                    ("Cloud Computing Essentials", "Mastering AWS, Azure, and Google Cloud platforms.", 200.00m, new List<int> { 4 }),
                    ("Machine Learning in Practice", "Implementing AI and ML solutions for real-world problems.", 235.00m, new List<int> { 4, 5 }),
                    ("Cybersecurity Fundamentals", "Protecting systems and data in the digital age.", 190.00m, new List<int> { 4 })
                }},
                { 5, new List<(string, string, decimal, List<int>)> {
                    ("Questions of Meaning", "An introduction to core philosophy topics and existential questions.", 140.00m, new List<int> { 5 }),
                    ("The Path Within", "A spiritual journey exploring inner peace and self-realization.", 128.00m, new List<int> { 3 }),
                    ("Ethics in the Modern World", "Exploring moral philosophy and its applications today.", 138.00m, new List<int> { 5 }),
                    ("Wisdom Across Cultures", "Comparative study of philosophical traditions worldwide.", 150.00m, new List<int> { 5 })
                }}
            };

            int bookId = 1;
            foreach (var categoryId in bookData.Keys.OrderBy(k => k))
            {
                foreach (var (title, description, price, authorIds) in bookData[categoryId])
                {
                    var book = new Book
                    {
                        Title = title,
                        Description = description,
                        Price = price,
                        Img = GetImagePath(bookId),
                        IsDeleted = false,
                        CategoryId = categoryId
                    };

                    _context.Books.Add(book);
                    await _context.SaveChangesAsync();

                    // Add book authors
                    foreach (var authorId in authorIds)
                    {
                        _context.BookAuthors.Add(new BookAuthor { BookId = book.Id, AuthorId = authorId });
                    }

                    bookId++;
                }
            }
            await _context.SaveChangesAsync();
        }

        private string GetImagePath(int bookId)
        {
            return bookId switch
            {
                1 or 2 => "images/books/echoes-past.jpg",
                3 or 4 => "images/books/physics-daily-life.jpg",
                5 or 6 => "images/books/silent-path.jpg",
                7 or 8 => "images/books/echoes-past.jpg",
                9 or 10 => "images/books/physics-daily-life.jpg",
                11 or 12 => "images/books/silent-path.jpg",
                13 or 14 => "images/books/echoes-past.jpg",
                15 or 16 => "images/books/physics-daily-life.jpg",
                _ => "images/books/silent-path.jpg"
            };
        }

        private async Task SeedCopiesAsync()
        {
            var books = await _context.Books.ToListAsync();
            int copyId = 1;

            foreach (var book in books)
            {
                int numCopies = Random.Shared.Next(3, 6);
                for (int i = 0; i < numCopies; i++)
                {
                    _context.Copies.Add(new Copy
                    {
                        Name = $"Copy-{copyId.ToString().PadLeft(3, '0')}",
                        AllowToRental = true,
                        BookId = book.Id
                    });
                    copyId++;
                }
            }
            await _context.SaveChangesAsync();
        }

        private async Task SeedUsersAsync()
        {
            var userRole = await _roleManager.FindByNameAsync("User");
            var adminRole = await _roleManager.FindByNameAsync("Admin");

            var users = new[]
            {
                new ApplicationUser
                {
                    FullName = "John Doe",
                    UserName = "john.doe@library.com",
                    Email = "john.doe@library.com",
                    PhoneNumber = "01000000001",
                    EmailConfirmed = true,
                    PhoneNumberConfirmed = true
                },
                new ApplicationUser
                {
                    FullName = "Jane Smith",
                    UserName = "jane.smith@library.com",
                    Email = "jane.smith@library.com",
                    PhoneNumber = "01000000002",
                    EmailConfirmed = true,
                    PhoneNumberConfirmed = true
                },
                new ApplicationUser
                {
                    FullName = "Admin User",
                    UserName = "admin@library.com",
                    Email = "admin@library.com",
                    PhoneNumber = "01000000003",
                    EmailConfirmed = true,
                    PhoneNumberConfirmed = true
                }
            };

            foreach (var user in users)
            {
                var result = await _userManager.CreateAsync(user, "Password@123");
                if (result.Succeeded)
                {
                    if (user.Email == "admin@library.com")
                    {
                        await _userManager.AddToRoleAsync(user, "Admin");
                    }
                    else
                    {
                        await _userManager.AddToRoleAsync(user, "User");
                    }
                }
            }
        }

        private async Task SeedRentalsAsync()
        {
            var users = await _context.Users.Take(3).ToListAsync();
            var copies = await _context.Copies.ToListAsync();

            if (users.Count < 3 || copies.Count < 3)
                return;

            var today = DateTime.Now;

            // Rental 1: Returned
            var rental1 = new Rental
            {
                RentedAt = today.AddDays(-30),
                DueAt = today.AddDays(-23),
                ReturnedAt = today.AddDays(-25),
                Amount = 25.00m,
                Status = RentalState.Returned,
                ApplicationUserId = users[0].Id
            };
            _context.Rentals.Add(rental1);
            await _context.SaveChangesAsync();
            _context.CopyRentals.Add(new CopyRental { RentalId = rental1.Id, CopyId = copies[0].Id });

            // Rental 2: Active
            var rental2 = new Rental
            {
                RentedAt = today.AddDays(-10),
                DueAt = today.AddDays(4),
                ReturnedAt = null,
                Amount = 30.00m,
                Status = RentalState.Active,
                ApplicationUserId = users[1].Id
            };
            _context.Rentals.Add(rental2);
            await _context.SaveChangesAsync();
            _context.CopyRentals.Add(new CopyRental { RentalId = rental2.Id, CopyId = copies[1].Id });

            // Rental 3: Overdue
            var rental3 = new Rental
            {
                RentedAt = today.AddDays(-20),
                DueAt = today.AddDays(-5),
                ReturnedAt = null,
                Amount = 35.00m,
                Status = RentalState.Overdue,
                ApplicationUserId = users[2].Id
            };
            _context.Rentals.Add(rental3);
            await _context.SaveChangesAsync();
            _context.CopyRentals.Add(new CopyRental { RentalId = rental3.Id, CopyId = copies[2].Id });

            // Rental 4: Another Overdue
            var rental4 = new Rental
            {
                RentedAt = today.AddDays(-25),
                DueAt = today.AddDays(-10),
                ReturnedAt = null,
                Amount = 28.00m,
                Status = RentalState.Overdue,
                ApplicationUserId = users[0].Id
            };
            _context.Rentals.Add(rental4);
            await _context.SaveChangesAsync();
            _context.CopyRentals.Add(new CopyRental { RentalId = rental4.Id, CopyId = copies[3].Id });

            await _context.SaveChangesAsync();
        }
    }
}
