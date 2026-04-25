using Library.Web.Core.Constants;
using Library.Web.Core.Models;
using Library.Web.Core.ViewModel.Author;

namespace Library.Web.Repository.IRepositories
{
    public interface IAuthorRepository : IRepository<Author>
    {
        Task<PagedResult<AuthorRowVM>> GetAllAsync(PaginationParams param, string? sortBy="");
        Task<Author?> GetByIdAsync(int id);
        Task DeleteAsync(Author? author);
        Task<bool> IsNameAvailableAsync(int id, string name);
    }
}
