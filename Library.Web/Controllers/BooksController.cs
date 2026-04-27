using Library.Web.Core.Constants;
using Library.Web.Core.Models;
using Library.Web.Core.ViewModel.Book;
using Library.Web.Data;
using Library.Web.Migrations;
using Library.Web.Repository.IRepositories;
using Library.Web.Repository.Repositories;
using Library.Web.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Runtime.InteropServices;

namespace Library.Web.Controllers
{
    public class BooksController : Controller
    {
        //ApplicationDbContext db ;

        IRepository<Book> repo;
        private readonly IBookRepository bookRepository;
        private readonly IRentalRepository rentalRepo;
        private readonly ApplicationDbContext appContext;
        private readonly ICategoryRepository categoryRepository;
        private readonly IEmailSender emailSender;
        private readonly IEmailTemplateService emailTemplateService;
        private readonly UserManager<ApplicationUser> userManager;

        public BooksController(IRepository<Book> _repo 
            , IBookRepository bookRepository
            , IRentalRepository rentalRepo
            , ApplicationDbContext appContext
            , ICategoryRepository categoryRepository
            , IEmailSender emailSender
            , IEmailTemplateService emailTemplateService
            , UserManager<ApplicationUser> userManager
            ) {

            repo = _repo;
            this.bookRepository = bookRepository;
            this.rentalRepo = rentalRepo;
            this.appContext = appContext;
            this.categoryRepository = categoryRepository;
            this.emailSender = emailSender;
            this.emailTemplateService = emailTemplateService;
            this.userManager = userManager;
        }
        public async Task<IActionResult> Index(int page = 1 , int? categoryId = null, string? search = null)
        {
            //var Books = await repo.GetAllAsync();


            var param = new PaginationParams
            {
                Page = page,
                PageSize = 8
            };

            var result = await bookRepository.GetAllBooksAsync(param , categoryId, search);


            var categories = await categoryRepository.GetAllAsync();

            ViewBag.Categories = categories;
            ViewBag.SelectedCategory = categoryId;
            ViewBag.Search = search;
            ViewBag.CurrentPage = page;

            return View(result);


            
        }

        public IActionResult Details(int Id)
        {
            var book = bookRepository.GetBookDetails(Id);
            if (book is not null)
            {
                return View(book);
            }
            return RedirectToAction("index", "books");
        }
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> rentBook (int Id , int copiesNumber, DateTime returnDate)
        {
            var copies = bookRepository.GetNBookCopies(Id, copiesNumber);

            if (!copies.Any())
            {
                return RedirectToAction(nameof(Details), "Books", new { Id });
            }

            var userId = int.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier).Value);
            var user = await userManager.FindByIdAsync(userId.ToString());

            Rental rental = new()
            {
                ApplicationUserId = userId,
                DueAt = returnDate,
                RentedAt = DateTime.UtcNow,
                Status = Core.Constants.RentalState.Active,
                CopyRentals = copies.Select(c => new CopyRental
                {
                    CopyId = c.Id
                }).ToList()
            };
            foreach(var copy in copies)
            {
                copy.AllowToRental = false;
            }
            appContext.UpdateRange(copies);
            await rentalRepo.AddAsync(rental);

            // Calculate total amount (inclusive of both rental and return date)
            var rentalDays = (int)Math.Floor((returnDate - DateTime.UtcNow).TotalDays) + 1;
            var totalAmount = copies.Sum(c => c.Book.Price) * rentalDays;

            // Send rental confirmation email
            bool emailSent = false;
            try
            {
                var emailHtml = await emailTemplateService.GetRentalConfirmationEmailAsync(user, copies, returnDate, totalAmount);
                await emailSender.SendEmailAsync(user.Email, "📚 Rental Confirmation - The Editorial Archive", emailHtml);
                emailSent = true;
            }
            catch (Exception ex)
            {
                // Log error but don't fail the rental
                Console.WriteLine($"Error sending rental email: {ex.Message}");
            }

            // Store confirmation data in TempData to pass to confirmation page
            TempData["RentalId"] = rental.Id;
            TempData["EmailSent"] = emailSent;

            return RedirectToAction(nameof(RentalConfirmation), new { rentalId = rental.Id });
        }

        [Authorize]
        public async Task<IActionResult> RentalConfirmation(int rentalId)
        {
            var userId = int.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier).Value);

            // Get rental details with includes
            var rental = await rentalRepo.GetByIdWithDetailsAsync(rentalId);

            if (rental == null || rental.ApplicationUserId != userId)
            {
                return RedirectToAction(nameof(Index));
            }

            // Calculate rental days and total amount
            var rentalDays = (int)Math.Floor((rental.DueAt - rental.RentedAt).TotalDays) + 1;
            var totalAmount = rental.CopyRentals?.Sum(cr => cr.Copy.Book.Price) * rentalDays ?? 0;

            // Check if email was sent from TempData
            var emailSent = TempData["EmailSent"] as bool? ?? false;

            ViewBag.EmailSent = emailSent;
            ViewBag.TotalAmount = totalAmount;
            ViewBag.RentalDays = rentalDays;

            return View(rental);
        }
    }
}
