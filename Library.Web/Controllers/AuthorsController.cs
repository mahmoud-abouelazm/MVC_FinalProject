using Library.Web.Core.Models;
using Library.Web.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Library.Web.Controllers
{
    public class AuthorsController : Controller
    {
        ApplicationDbContext db;
        public AuthorsController(ApplicationDbContext context)
        {
            db = context;
        }
        public IActionResult Index()
        {
            var authors = db.Authors.ToList();
            if (authors.Any())
                return View(authors);
            else
                return Content("No Feeeeeed");
        }

        public IActionResult Details(int Id)
        {

            var author = db.Authors
                      .Include(a => a.BookAuthors)        // load related BookAuthors rows
                      .ThenInclude(ba => ba.Book)         // then from each, load the Book
                      .FirstOrDefault(a => a.Id == Id);

            if (author == null)
                return NotFound();

            return View(author);

        }
    }
}
