using Library.Web.Core.ViewModel.Copies;
using Library.Web.Core.ViewModel.Users;
using Microsoft.AspNetCore.Identity;

namespace Library.Web.Services
{
    public interface IUserService
    {
        Task<UserVM> GetIndexVmAsync(int page, int pageSize, string search);
        Task<bool> ConfirmEmailAsync(int userId);

        Task<bool> DeactiveEmailAsync(int userId);

        Task<UserRowVM?> GetUserDetails(int userId);

        Task<ServiceResult> AddNewUserAsync(CreateUserVM userVM);
        Task<CreateUserVM> GetCreateUserVMFormAsync();
        Task<ServiceResult> ChangeUserRoleAsync(int userId, string newRoleName);
        Task<IEnumerable<IdentityRole<int>>> GetAllRolesAsync();

        Task<EditUserRoleVM> GetEditUserRoleVMAsync(int userId);
    }
}
