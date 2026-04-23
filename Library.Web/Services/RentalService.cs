using Library.Web.Core.Constants;
using Library.Web.Core.ViewModel.Rentals;
using Library.Web.Repository.IRepositories;

namespace Library.Web.Services
{
    public class RentalService(IRentalRepository repo) : IRentalService
    {
        private readonly IRentalRepository _repo = repo;
        

        public async Task<RentalVM> GetIndexVmAsync(
            int page, int pageSize, string search, string statusFilter)
        {
            var (rows, total) = await _repo.GetPagedAsync(page, pageSize, search, statusFilter);
            var (active, overdue, returned, revenue) = await _repo.GetSummaryAsync();

            return new RentalVM
            {
                Rentals = rows,
                TotalRentals = total,
                CurrentPage = page,
                PageSize = pageSize,
                Search = search,
                StatusFilter = statusFilter,
                ActiveCount = active,
                OverdueCount = overdue,
                ReturnedCount = returned,
                TotalRevenue = revenue
            };
        }

      
        public async Task<ReturnConfirmVM?> GetReturnConfirmAsync(int rentalId)
            => await _repo.GetReturnConfirmVmAsync(rentalId);

       

        public async Task<ServiceResult> ProcessReturnAsync(int rentalId)
        {
            var rental = await _repo.GetByIdWithDetailsAsync(rentalId);

            if (rental is null)
                return new ServiceResult(false, "Rental not found.");

            if (rental.Status == RentalState.Returned)
                return new ServiceResult(false, "This rental has already been returned.");

            var now = DateTime.UtcNow;
            var daysRented = Math.Max(1, (int)Math.Ceiling((now - rental.RentedAt).TotalDays));
            var totalPrice = rental.CopyRentals.Sum(cr => cr.Copy.Book.Price);
            var isLate = now > rental.DueAt;
            var daysLate = isLate ? (int)Math.Ceiling((now - rental.DueAt).TotalDays) : 0;
            var daysOnTime = daysRented - daysLate;

         
            var baseAmount = daysOnTime * totalPrice;
            var penalty = isLate ? daysLate * totalPrice * 2 : 0;
            var totalAmount = isLate ? baseAmount + penalty : daysRented * totalPrice;

            rental.ReturnedAt = now;
            rental.Status = RentalState.Returned;
            rental.Amount = totalAmount;

           
            foreach (var cr in rental.CopyRentals)
            {
                cr.Copy.AllowToRental = true;
            }

          
            await _repo.UpdateAsync(rental);

            return new ServiceResult(true,
                $"Rental #{rental.Id} returned. Amount collected: ${totalAmount:0.00}");
        }
    }
}
