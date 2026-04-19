using Library.Web.Core.Constants;
using Library.Web.Core.Models;
using Library.Web.Core.ViewModel.Category;

namespace Library.Web.Repository.IRepositories
{
    public interface ICategoryRepository : IRepository<Category>
    {

        Task<PagedResult<CategoryRowVM>> GetAllAsync(PaginationParams param);
        Task<List<CategoryRowVM>> GetAllAsync();
        Task<Category?> GetByIdAsync(int id);
        Task DeleteAsync(Category category);
    }
}
