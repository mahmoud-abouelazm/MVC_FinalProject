using Library.Web.Core.Models;
using Library.Web.Core.ViewModel.BookVMs;
using Library.Web.Data;
using Library.Web.Repository.IRepositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;

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
                copiesNumber = sourceBook.Copies.Count(c=>c.AllowToRental),
                Description = sourceBook.Description,
                Img = sourceBook.Img,
                Price = sourceBook.Price,
                Title = sourceBook.Title,
                Authors = sourceBook.BookAuthors.Select(ba => ba.Author).ToList()
            };
        }

        public List<Copy> GetNBookCopies(int Id ,int n)
        {
            return context.Copies
                .Where(c => c.BookId == Id && c.AllowToRental)
                .OrderBy(c => c.Id)
                .Take(n)
                .ToList();
        }
    }
}
