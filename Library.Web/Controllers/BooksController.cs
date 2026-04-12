using Library.Web.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Library.Web.Controllers
{
    public class BooksController : Controller
    {
        ApplicationDbContext db;
        public BooksController(ApplicationDbContext context)
        {
            db = context;   
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult getBooksByAuthorID(int authorID)
        {
            var BooksByAuthor = db.Books.Include(ba => ba.BookAuthors)
                             .ThenInclude(a => a.Author)
                          ;

            return View(BooksByAuthor);
        }
    }
}
