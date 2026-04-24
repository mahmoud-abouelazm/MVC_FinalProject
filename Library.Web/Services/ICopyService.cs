using Library.Web.Core.ViewModel.Copies;

namespace Library.Web.Services
{
    public interface ICopyService
    {
        Task<CopyVM> GetIndexVmAsync(int page, int pageSize, string search);
        Task<CopyVM> GetIndexVmByBookAsync(int bookId, int page, int pageSize, string search);
        Task<CopyFormVM> GetCreateVmAsync(int? bookId);
        Task<CopyFormVM?> GetEditVmAsync(int id);
        Task<ServiceResult> CreateAsync(CopyFormVM vm);
        Task<ServiceResult> UpdateAsync(CopyFormVM vm);
        Task<ServiceResult> DeleteAsync(int id);
        Task<ServiceResult> ToggleRentalAsync(int id);
    }
    public record ServiceResult(bool Success, string Message);

}
