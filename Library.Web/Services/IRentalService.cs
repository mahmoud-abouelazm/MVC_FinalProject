using Library.Web.Core.Models;
using Library.Web.Core.ViewModel.Rentals;
using Library.Web.Repository.IRepositories;

namespace Library.Web.Services
{
    public interface IRentalService
    {
        Task<RentalVM> GetIndexVmAsync(int page, int pageSize, string search, string statusFilter);
        Task<ReturnConfirmVM?> GetReturnConfirmAsync(int rentalId);
        Task<ServiceResult> ProcessReturnAsync(int rentalId);
    }
}
