using Library.Web.Core.ViewModel.BookVMs;

namespace Library.Web.Repository.IRepositories
{
    public interface IBookRepository
    {
        BookDetailsVM GetBookDetails(int Id);
    }
}
