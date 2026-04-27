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

        public HomeController(IAuthorService IAuthorService,
            IBookRepository bookRepository
            )
        {
            this.AuthorService = IAuthorService;
            this.bookRepository = bookRepository;
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

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
