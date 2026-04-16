using Library.Web.Core.Constants;
using Library.Web.Core.ViewModel.Category;

namespace Library.Web.Services
{
    public interface ICategoryService
    {
        Task<PagedResult<CategoryRowVM>> GetAllAsync(PaginationParams param);
        Task CreateAsync(CategoryFormVM vm);
        Task<CategoryFormVM?> GetVmForEditAsync(int id); 
        Task UpdateAsync(CategoryFormVM vm);
        Task DeleteAsync(int id);
    }
}
