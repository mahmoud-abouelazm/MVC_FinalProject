using Library.Web.Core.Models;
using Library.Web.Core.ViewModel.Users;
using Library.Web.Data;
using Library.Web.Repository.IRepositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Library.Web.Repository.Repositories
{
    public class UserRepository(UserManager<ApplicationUser> userManager, ApplicationDbContext context) : Repository<ApplicationUser>(context), IUserRepository
    {

        private readonly UserManager<ApplicationUser> _userManager = userManager;
        private readonly ApplicationDbContext _context = context;

        public async Task<IEnumerable<UserRowVM>> GetAllUsersAsync(int page, int pageSize, string search)
        {
            var users = await _userManager.Users
                .Where(u => u.FullName != null && u.FullName.Contains(search) && u.Email.Contains(search))
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(u => new UserRowVM
                {
                    Id = u.Id,
                    Name = u.FullName,
                    Email = u.Email,
                    Role = _userManager.GetRolesAsync(u).Result.FirstOrDefault() ?? "No Role",
                    IsActive = u.EmailConfirmed
                }).ToListAsync();

            return users ;
        }

        public Task ConfirmEmailAsync(ApplicationUser user)
        {
            user.EmailConfirmed = true;
            _context.SaveChanges();
            return Task.CompletedTask;
        }
        public Task DeactiveEmailAsync(ApplicationUser user)
        {
            user.EmailConfirmed = false;
            _context.SaveChanges();
            return Task.CompletedTask;
        }

        public async Task<int> CountOfActiveUsersAsync()
        {
            return await _userManager.Users.Where(u => u.EmailConfirmed).CountAsync();
        }

        public async Task<int> CountOfNotActiveUsersAsync()
        {
            return await _userManager.Users.Where(u => !u.EmailConfirmed).CountAsync();
        }

        public async Task<int> CountOfUsersAsync(string search)
        {
            return await _userManager.Users.Where(u => u.FullName != null && u.FullName.Contains(search) && u.Email.Contains(search)).CountAsync();
        }

        public async Task<ApplicationUser?> FindByIdAsync(int id)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());

            if (user is null)
                return null;
            return user;

        }


        public async Task<UserRowVM> GetUserDetails(int userId)
        {
            var user = await FindByIdAsync(userId);
            if (user is null) return null;
            return new UserRowVM
            {
                Id = user.Id,
                Name = user.FullName,
                Email = user.Email,
                Role = await _userManager.GetRolesAsync(user).ContinueWith(t => t.Result.FirstOrDefault() ?? "No Role"),
                PhoneNumber = user.PhoneNumber,
                IsActive = user.EmailConfirmed
            };

        }
    }
}
