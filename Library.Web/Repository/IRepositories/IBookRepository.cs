using Library.Web.Core.Models;
using Library.Web.Core.ViewModel.BookVMs;

namespace Library.Web.Repository.IRepositories
{
    public interface IBookRepository
    {
        BookDetailsVM GetBookDetails(int Id);
        List<Copy> GetNBookCopies(int Id , int n);
    }
}
