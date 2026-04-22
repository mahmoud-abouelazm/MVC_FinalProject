using Library.Web.Core.Models;
using Library.Web.Core.ViewModel.Users;

namespace Library.Web.Repository.IRepositories
{
    public interface IUserRepository :IRepository<ApplicationUser>
    {

        Task<IEnumerable<UserRowVM>> GetAllUsersAsync(int page, int pageSize, string search);
        Task<int> CountOfUsersAsync(string search);
        Task<int> CountOfActiveUsersAsync();
        Task<int> CountOfNotActiveUsersAsync();
        Task ConfirmEmailAsync(ApplicationUser user);
        Task DeactiveEmailAsync(ApplicationUser user);

        Task<ApplicationUser?> FindByIdAsync(int id);




    }
}
