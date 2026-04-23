using Library.Web.Core.Models;
using Library.Web.Core.ViewModel.Copies;
using Library.Web.Core.ViewModel.Users;
using Library.Web.Repository.IRepositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Validation;

namespace Library.Web.Services
{
    public class UserService(IUserRepository userRepository) : IUserService
    {
        private readonly IUserRepository _userRepository = userRepository;

        public async Task<UserVM> GetIndexVmAsync(int page, int pageSize, string search)
        {
            int total = await _userRepository.CountOfUsersAsync(search);
            var rows = await _userRepository.GetAllUsersAsync(page, pageSize, search);

            return new UserVM
            {
                Users = rows,
                TotalUsers = total,
                CurrentPage = page,
                PageSize = pageSize,
                Search = search,
                ActiveUsers = await _userRepository.CountOfActiveUsersAsync(),
                NotActiveUsers = await _userRepository.CountOfNotActiveUsersAsync()
                
            };
        }

        public async Task<bool> ConfirmEmailAsync(int userId)
        {
            var user = await _userRepository.FindByIdAsync(userId);
            if (user is null) return false;
            await _userRepository.ConfirmEmailAsync(user);
            return true;
        }

        public async Task<bool> DeactiveEmailAsync(int userId)
        {
            var user = await _userRepository.FindByIdAsync(userId);
            if (user is null) return false;
            await _userRepository.DeactiveEmailAsync(user);
            return true;
        }

        public async Task<UserRowVM?> GetUserDetails(int userId)
        {
            var user = await _userRepository.GetUserDetails(userId);
            if (user is null) return null;

            return user;
        }

        public async Task<CreateUserVM> GetCreateUserVMFormAsync()
        {
            var roles = await _userRepository.GetAllRolesAsync();
            var vm = new CreateUserVM
            {
                Roles = roles.Select(r => new SelectListItem
                {
                    Text = r.Name,
                    Value = r.Id.ToString()
                }).ToList()
            };
            return vm;
        }

        public async Task<ServiceResult> AddNewUserAsync(CreateUserVM userVM)
        {
            try
            {
                var roles = await _userRepository.GetAllRolesAsync();
                var selectedRole = roles.FirstOrDefault(r => r.Id.ToString() == userVM.RoleId.ToString());

                if (selectedRole is null)
                {
                    return new ServiceResult(false, "Selected role not found.");
                }

                await _userRepository.AddNewUserAsync(userVM, userVM.Password, selectedRole.Name!);
                return new ServiceResult(true, "User added successfully.");
            }
            catch (Exception ex)
            {
                return new ServiceResult(false, ex.Message);
            }
        }

        public async Task<ServiceResult> ChangeUserRoleAsync(int userId, string newRoleName)
        {
            try
            {
                var roles = await _userRepository.GetAllRolesAsync();
                var roleExists = roles.Any(r => r.Name == newRoleName);

                if (!roleExists)
                {
                    return new ServiceResult(false, "Role not found.");
                }

                await _userRepository.ChangeUserRoleAsync(userId, newRoleName);
                return new ServiceResult(true, "User role changed successfully.");
            }
            catch (Exception ex)
            {
                return new ServiceResult(false, ex.Message);
            }
        }

        public async Task<IEnumerable<IdentityRole<int>>> GetAllRolesAsync()
        {
            return await _userRepository.GetAllRolesAsync();
        }

        public async Task<EditUserRoleVM> GetEditUserRoleVMAsync(int userId)
        {
            var user = await GetUserDetails(userId);
            if (user is null) throw new Exception("User not found");

            var roles = await GetAllRolesAsync();
            var vm = new EditUserRoleVM
            {
                UserId = userId,
                UserName = user.Name,
                CurrentRole = user.Role,
                Roles = roles.Select(r => new SelectListItem
                {
                    Text = r.Name,
                    Value = r.Name,
                    Selected = user.Role == r.Name
                }).ToList()
            };

            return vm;
        }
    }
}
