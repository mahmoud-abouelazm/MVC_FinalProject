using Library.Web.Core.Models;
using Library.Web.Data;
using Library.Web.Repository.IRepositories;
using Microsoft.EntityFrameworkCore;

namespace Library.Web.Repository.Repositories
{
	public class RentalRepository : IRentalRepository
	{
		private readonly ApplicationDbContext dbcontext;

		public RentalRepository(ApplicationDbContext dbcontext)
		{
			this.dbcontext = dbcontext;
		}
		public async Task<IEnumerable<Rental>> GetAllAsync()
		{
			return await dbcontext.Rentals.Include(r => r.CopyRentals).ThenInclude(cr => cr.Copy).ThenInclude(c => c.Book).ToListAsync();
		}

		public async Task<IEnumerable<Rental>> GetAllRentalsForSpecificUserAsync(int userId)
		{
			return await  dbcontext.Rentals.Where(r=>r.ApplicationUserId==userId).Include(r=>r.CopyRentals).ThenInclude(cr=>cr.Copy).ThenInclude(c=>c.Book).ToListAsync();
		}

		
	}
}
