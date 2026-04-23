using Library.Web.Core.Models;
using Library.Web.Core.ViewModel.Copies;
using Library.Web.Core.ViewModel.Users;
using Library.Web.Repository.IRepositories;

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



        }
}
