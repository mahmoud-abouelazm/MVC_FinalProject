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
        public BooksController(IRepository<Book> _repo) {
            
            repo = _repo;
        } 
        public async Task<IActionResult> IndexAsync()
        {
            var Books = await repo.GetAllAsync();

            return View(Books);
        }


    }
}
