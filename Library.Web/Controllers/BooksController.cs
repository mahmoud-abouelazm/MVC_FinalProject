using Library.Web.Core.Constants;
using Library.Web.Core.Models;
using Library.Web.Core.ViewModel.Book;
using Library.Web.Data;
using Library.Web.Migrations;
using Library.Web.Repository.IRepositories;
using Library.Web.Repository.Repositories;
using Microsoft.AspNetCore.Authorization;
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
        private readonly IRepository<Rental> rentalRepo;
        private readonly ApplicationDbContext appContext;
        private readonly ICategoryRepository categoryRepository;

        public BooksController(IRepository<Book> _repo 
            , IBookRepository bookRepository
            ,IRepository<Rental> rentalRepo
            ,ApplicationDbContext appContext
            , ICategoryRepository categoryRepository
            ) {
            
            repo = _repo;
            this.bookRepository = bookRepository;
            this.rentalRepo = rentalRepo;
            this.appContext = appContext;
            this.categoryRepository = categoryRepository;
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

            Rental rental = new()
            {
                ApplicationUserId = int.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier).Value),
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

            return RedirectToAction("index", "books");
        }
    }
}
