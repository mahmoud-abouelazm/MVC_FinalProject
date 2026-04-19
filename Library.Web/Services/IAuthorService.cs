using Library.Web.Core.Constants;
using Library.Web.Core.ViewModel.Author;

namespace Library.Web.Services
{
    public interface IAuthorService
    {
        Task<PagedResult<AuthorRowVM>> GetAllAsync(PaginationParams param, string? sortBy ="");
        Task CreateAsync(AuthorFormVM vm);
        Task<AuthorFormVM?> GetVmForEditAsync(int id); 
        Task UpdateAsync(AuthorFormVM vm);
        Task DeleteAsync(int id);
    }
}
