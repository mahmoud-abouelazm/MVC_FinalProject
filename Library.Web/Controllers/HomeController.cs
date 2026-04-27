using Library.Web.Core.Constants;
using Library.Web.Core.ViewModel;
using Library.Web.Repository.IRepositories;
using Library.Web.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Library.Web.Controllers
{
    
    public class HomeController : Controller
    {

        IAuthorService AuthorService;
        private readonly IBookRepository bookRepository;
        private readonly ISeedService seedService;

        public HomeController(IAuthorService IAuthorService,
            IBookRepository bookRepository,
            ISeedService seedService
            )
        {
            this.AuthorService = IAuthorService;
            this.bookRepository = bookRepository;
            this.seedService = seedService;
        }


        public async Task<IActionResult>Index()
        {
            return View(await bookRepository.GetNewBooks());
        }


        public async Task<IActionResult> Authors(int page = 1 , string sortBy = "books")
        {
            var param = new PaginationParams
            {
                Page = page,
                PageSize = 6
            };

            if (string.IsNullOrEmpty(sortBy))
                sortBy = "books";


            var result  = await AuthorService.GetAllAsync( param , sortBy );

            ViewBag.SortBy = sortBy;

            return View(result);
        }

        public async Task<IActionResult> AuthorBooks(int authorId, int page = 1, string? search = null)
        {
            var param = new PaginationParams
            {
                Page = page,
                PageSize = 8
            };

            var result = await bookRepository.GetBooksByAuthorAsync(param, authorId, search);

            if (!result.Data.Any() && page == 1)
            {
                return RedirectToAction("Authors");
            }

            var authorVm = await AuthorService.GetVmForEditAsync(authorId);

            ViewBag.AuthorName = authorVm?.Name ?? "Unknown Author";
            ViewBag.AuthorId = authorId;
            ViewBag.Search = search;
            ViewBag.Page = page;

            return View(result);
        }

        [Authorize]
        public IActionResult Privacy()
        {
            return View();
        }

        [HttpGet]
        [Route("seed")]
        public async Task<IActionResult> Seed()
        {
            try
            {
                await seedService.SeedDatabaseAsync();
                return Ok(new
                {
                    success = true,
                    message = "Database seeded successfully!",
                    accounts = new[]
                    {
                        new { email = "john.doe@library.com", password = "Password@123", role = "User" },
                        new { email = "jane.smith@library.com", password = "Password@123", role = "User" },
                        new { email = "admin@library.com", password = "Password@123", role = "Admin" }
                    }
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            var errorViewModel = new ErrorViewModel
            {
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier,
                StatusCode = HttpContext.Response.StatusCode,
                StatusMessage = (string?)HttpContext.Items["StatusMessage"],
                ExceptionMessage = (string?)HttpContext.Items["ExceptionMessage"],
                ExceptionStackTrace = (string?)HttpContext.Items["ExceptionStackTrace"]
            };

            return View(errorViewModel);
        }
    }
}
