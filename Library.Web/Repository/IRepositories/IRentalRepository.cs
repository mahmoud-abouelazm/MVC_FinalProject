using Library.Web.Core.Models;
using Library.Web.Core.ViewModel.Rentals;

namespace Library.Web.Repository.IRepositories
{
    public interface IRentalRepository : IRepository<Rental>
    {
        Task<(IEnumerable<RentalRowVM> Rows, int Total)> GetPagedAsync(
            int page, int pageSize, string search, string statusFilter);

        Task<ReturnConfirmVM?> GetReturnConfirmVmAsync(int rentalId);

        Task<Rental?> GetByIdWithDetailsAsync(int rentalId);

        // Summary counts for cards
        Task<(int Active, int Overdue, int Returned, decimal Revenue)> GetSummaryAsync();

        Task<Rental?> GetActiveRentalByCopyIdAsync(int copyId);
    }
}
