using Library.Web.Core.Constants;
using Library.Web.Core.ViewModel.Rentals;
using Library.Web.Repository.IRepositories;
using Microsoft.AspNetCore.Identity.UI.Services;

namespace Library.Web.Services
{
    public class RentalService(IRentalRepository repo, IEmailSender emailSender, IEmailTemplateService emailTemplateService) : IRentalService
    {
        private readonly IRentalRepository _repo = repo;
        private readonly IEmailSender _emailSender = emailSender;
        private readonly IEmailTemplateService _emailTemplateService = emailTemplateService;


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
            var daysRented = Math.Max(1, (int)Math.Floor((now - rental.RentedAt).TotalDays) + 1);
            var totalPrice = rental.CopyRentals.Sum(cr => cr.Copy.Book.Price);
            var isLate = now > rental.DueAt;
            var daysLate = isLate ? (int)Math.Floor((now - rental.DueAt).TotalDays) : 0;
            var daysOnTime = daysRented - daysLate;

            // Calculate base amount (only for days on time)
            var baseAmount = daysOnTime * totalPrice;

            // Calculate late penalty: 2x the daily rental price for each overdue day
            var lateFeesPerDay = 2m * totalPrice;
            var penalty = isLate ? daysLate * lateFeesPerDay : 0;

            var totalAmount = baseAmount + penalty;

            rental.ReturnedAt = now;
            rental.Status = RentalState.Returned;
            rental.Amount = totalAmount;

            foreach (var cr in rental.CopyRentals)
            {
                cr.Copy.AllowToRental = true;
            }

            await _repo.UpdateAsync(rental);

            // Send return invoice email
            try
            {
                if (rental.ApplicationUser != null)
                {
                    var emailHtml = await _emailTemplateService.GetReturnInvoiceEmailAsync(rental.ApplicationUser, rental);
                    await _emailSender.SendEmailAsync(rental.ApplicationUser.Email, "📋 Return Invoice - The Editorial Archive", emailHtml);
                }
            }
            catch (Exception ex)
            {
                // Log error but don't fail the return process
                Console.WriteLine($"Error sending return invoice email: {ex.Message}");
            }

            return new ServiceResult(true,
                $"Rental #{rental.Id} returned. Amount collected: ${totalAmount:0.00}");
        }
    }
}
