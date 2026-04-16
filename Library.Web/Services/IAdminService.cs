using Library.Web.Core.Constants;
using Library.Web.Core.ViewModel.Book;

namespace Library.Web.Services
{
    public interface IAdminService
    {
        Task<PagedResult<BookRowVM>> GetAllAsync(PaginationParams param);
        Task CreateAsync(BookFormVM vm, IFormFile? file);
        Task UpdateAsync(BookFormVM vm, IFormFile? file);
        Task DeleteAsync(int id);
        Task<BookFormVM> BuildFormAsync(BookFormVM vm);
        Task<BookFormVM> GetForEditAsync(int id);

    }
}
