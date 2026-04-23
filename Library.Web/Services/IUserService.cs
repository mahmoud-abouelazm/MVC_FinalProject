using Library.Web.Core.ViewModel.Copies;
using Library.Web.Core.ViewModel.Users;

namespace Library.Web.Services
{
    public interface IUserService
    {
        Task<UserVM> GetIndexVmAsync(int page, int pageSize, string search);
        Task<bool> ConfirmEmailAsync(int userId);

        Task<bool> DeactiveEmailAsync(int userId);

        Task<UserRowVM?> GetUserDetails(int userId);
    }
}
