using Library.Web.Core.Models;

namespace Library.Web.Repository.IRepositories
{
	public interface IRentalRepository /*: IRepository<Rental>*/
	{
		Task<IEnumerable<Rental>> GetAllAsync();
		Task<IEnumerable<Rental>> GetAllRentalsForSpecificUserAsync(int userId);
	}
}
