using Library.Web.Core.Models;
using Library.Web.Core.ViewModel.BookVMs;
using Library.Web.Data;
using Library.Web.Repository.IRepositories;
using Microsoft.EntityFrameworkCore;

namespace Library.Web.Repository.Repositories
{
    public class BooksRepository : Repository<Book> , IBookRepository
    {
        private readonly ApplicationDbContext context;

        public BooksRepository(ApplicationDbContext context) : base(context)
        {
            this.context = context;
        }

        public BookDetailsVM GetBookDetails(int id)
        {
            var sourceBook = context.Books
                .Include(x => x.Copies)
                .Include(x => x.Category)
                .Include(x => x.BookAuthors)
                    .ThenInclude(ba => ba.Author)
                .FirstOrDefault(b => b.Id == id);

            if (sourceBook == null)
                return null;

            return new BookDetailsVM()
            {
                Id = sourceBook.Id,
                Category = sourceBook.Category.Name,
                copiesNumber = sourceBook.Copies.Count(),
                Description = sourceBook.Description,
                Img = sourceBook.Img,
                Price = sourceBook.Price,
                Title = sourceBook.Title,
                Authors = sourceBook.BookAuthors.Select(ba => ba.Author).ToList()
            };
        }


    }
}
