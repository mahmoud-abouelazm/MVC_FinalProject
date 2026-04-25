using Library.Web.Core.Models;
using Library.Web.Core.ViewModel.Users;
using Microsoft.AspNetCore.Identity;

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

        Task<UserRowVM> GetUserDetails(int userId);
        Task AddNewUserAsync(CreateUserVM user, string password, string roleName);
        Task ChangeUserRoleAsync(int userId, string newRoleName);
        Task<IEnumerable<IdentityRole<int>>> GetAllRolesAsync();
        Task<bool> CheckIfEmailExistsAsync(string email);

    }
}
