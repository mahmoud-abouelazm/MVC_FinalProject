using Library.Web.Core.Models;

namespace Library.Web.Services
{
    public interface IEmailTemplateService
    {
        Task<string> GetRentalConfirmationEmailAsync(ApplicationUser user, List<Copy> copies, DateTime dueDate, decimal totalAmount);
        Task<string> GetReturnInvoiceEmailAsync(ApplicationUser user, Rental rental);
        Task<string> GetRegistrationWelcomeEmailAsync(ApplicationUser user);
        Task<string> GetOverdueReminderEmailAsync(ApplicationUser user, Rental rental);
    }
}
