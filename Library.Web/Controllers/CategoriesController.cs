using Library.Web.Core.Models;
using Library.Web.Data;
using Microsoft.AspNetCore.Mvc;

namespace Library.Web.Controllers
{
    public class CategoriesController : Controller
    {
        ApplicationDbContext db;
        public CategoriesController(ApplicationDbContext context)
        {
            db = context;
            
        }
        public IActionResult Index()
        {
            List<Category> categories = db.Categories.ToList();


            return View(categories);
        }

        public IActionResult CategoryFilter(int CategoryId)
        {

            var BooksInCatogry = db.Books.Where(b => b.CategoryId == CategoryId && b.IsDeleted==false).ToList();

            if (BooksInCatogry.Any()) 
                return View(BooksInCatogry);
            else
                return Content("No Feed");

        }

        public IActionResult Display()
        {
            return View();
        }
    }
}
