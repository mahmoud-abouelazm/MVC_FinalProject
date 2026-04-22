using Library.Web.Core.Models;
using Library.Web.Core.ViewModel.Users;
using Library.Web.Data;
using Library.Web.Repository.IRepositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Library.Web.Repository.Repositories
{
    public class UserRepository( UserManager<ApplicationUser> userManager ,ApplicationDbContext context) :Repository<ApplicationUser>(context) , IUserRepository
    {
        
        private readonly UserManager<ApplicationUser> _userManager = userManager;
        private readonly ApplicationDbContext _context = context;

        public Task<IEnumerable<UserRowVM>> GetAllUsersAsync(int page , int pageSize, string search)
        {
            var users = _userManager.Users
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
                }).ToList();

            return Task.FromResult(users.AsEnumerable());
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

        public Task<int> CountOfActiveUsersAsync()
        {
            return _userManager.Users.Where(u => u.EmailConfirmed).CountAsync();
        }

        public Task<int> CountOfNotActiveUsersAsync()
        {
            return _userManager.Users.Where(u => !u.EmailConfirmed).CountAsync();
        }

        public Task<int> CountOfUsersAsync(string search)
        {
            return _userManager.Users.Where(u => u.FullName != null && u.FullName.Contains(search) && u.Email.Contains(search)).CountAsync();
        }

        public async Task<ApplicationUser?> FindByIdAsync(int id)
        {
         var user = await _userManager.FindByIdAsync(id.ToString());
            
           if(user is null)
                return null;
           return user;

        }

    
    }
}
