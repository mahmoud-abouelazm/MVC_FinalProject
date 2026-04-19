using Library.Web.Core.Constants;
using Library.Web.Core.Models;
using Library.Web.Core.ViewModel.Book;
using Library.Web.Core.ViewModel.BookVMs;

namespace Library.Web.Repository.IRepositories
{
    public interface IBookRepository: IRepository<Book>
    {
        BookDetailsVM GetBookDetails(int Id);
        List<Copy> GetNBookCopies(int Id , int n);

        Task<PagedResult<BookRowVM>> GetAllBooksAsync(PaginationParams param, int? categoryId);
        Task<Book?> GetWithAuthorsAsync(int id);

    }
}
