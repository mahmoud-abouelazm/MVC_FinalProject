using Library.Web.Core.Models;
using Library.Web.Core.ViewModel.Users;
using Library.Web.Data;
using Library.Web.Repository.IRepositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Library.Web.Repository.Repositories
{
    public class UserRepository(UserManager<ApplicationUser> userManager, ApplicationDbContext context , RoleManager<IdentityRole<int>> roleManager) : Repository<ApplicationUser>(context), IUserRepository
    {

        private readonly UserManager<ApplicationUser> _userManager = userManager;
        private readonly ApplicationDbContext _context = context;
        private readonly RoleManager<IdentityRole<int>> _roleManager = roleManager;

        public async Task<IEnumerable<UserRowVM>> GetAllUsersAsync(int page, int pageSize, string search)
        {
         
            var query = _context.Users
          .Where(u => u.FullName != null &&
                     (string.IsNullOrEmpty(search) ||
                      u.FullName.Contains(search) ||
                      u.Email.Contains(search)))
          .Select(u => new UserRowVM
          {
              Id = u.Id,
              Name = u.FullName,
              Email = u.Email,
              IsActive = u.EmailConfirmed,

              Role = (from ur in _context.UserRoles
                      join r in _context.Roles on ur.RoleId equals r.Id
                      where ur.UserId == u.Id
                      select r.Name).FirstOrDefault() ?? "No Role",
              PhoneNumber = u.PhoneNumber
          });

            var users = await query
                .AsNoTracking()
                .OrderBy(u => u.Name)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return users;
        }

        public async Task ConfirmEmailAsync(ApplicationUser user)
        {
            user.EmailConfirmed = true;
            await _context.SaveChangesAsync();
        }
        public async Task DeactiveEmailAsync(ApplicationUser user)
        {
            user.EmailConfirmed = false;
            await _context.SaveChangesAsync();
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
            return await _userManager.Users
                .Where(u => u.FullName != null &&
                            (string.IsNullOrEmpty(search) ||
                             u.FullName.Contains(search) ||
                             u.Email.Contains(search)))
                .CountAsync();
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

        public async Task AddNewUserAsync(CreateUserVM user, string password, string roleName)
        {
            var newUser = new ApplicationUser
            {
                UserName = user.Email,
                Email = user.Email,
                FullName = user.FullName,
                PhoneNumber = user.PhoneNumber,
                EmailConfirmed = true
            };

            var result = await _userManager.CreateAsync(newUser, password);

            if (!result.Succeeded)
            {
                throw new Exception(string.Join(", ", result.Errors.Select(e => e.Description)));
            }

            var createdUser = await _userManager.FindByEmailAsync(user.Email);

            if (createdUser is null)
            {
                throw new Exception("User creation succeeded but user could not be found.");
            }

            await _userManager.AddToRoleAsync(createdUser, roleName);
        }

        public async Task<IEnumerable<IdentityRole<int>>> GetAllRolesAsync()
        {
            return await _roleManager.Roles.ToListAsync();
        }

        public async Task ChangeUserRoleAsync(int userId, string newRoleName)
        {
            var user = await FindByIdAsync(userId);
            if (user is null)
            {
                throw new Exception("User not found.");
            }

            var currentRoles = await _userManager.GetRolesAsync(user);

            if (currentRoles.Any())
            {
                await _userManager.RemoveFromRolesAsync(user, currentRoles);
            }

            await _userManager.AddToRoleAsync(user, newRoleName);
        }


    }
}
