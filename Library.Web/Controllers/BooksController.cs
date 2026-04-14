using Library.Web.Core.Models;
using Library.Web.Data;
using Library.Web.Repository.IRepositories;
using Library.Web.Repository.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Library.Web.Controllers
{
    public class BooksController : Controller
    {
        //ApplicationDbContext db ;

        IRepository<Book> repo;
        private readonly IBookRepository bookRepository;

        public BooksController(IRepository<Book> _repo , IBookRepository bookRepository) {
            
            repo = _repo;
            this.bookRepository = bookRepository;
        } 
        public async Task<IActionResult> IndexAsync()
        {
            var Books = await repo.GetAllAsync();

            return View(Books);
        }

        public async Task<IActionResult> BookDetails(int Id)
        {
            var book = bookRepository.GetBookDetails(Id);
            if (book is not null)
            {
                return View(book);
            }
            return RedirectToAction("index", "books");
        }
    }
}
