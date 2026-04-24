using Library.Web.Core.Models;
using Library.Web.Core.ViewModel.Copies;

namespace Library.Web.Repository.IRepositories
{
    public interface ICopyRepository : IRepository<Copy>
    {
        Task<IEnumerable<CopyRowVM>> GetAllRowsAsync(int page, int pageSize, string search);
        Task<IEnumerable<CopyRowVM>> GetRowsByBookIdAsync(int bookId, int page, int pageSize, string search);
        Task<int> CountAsync(string search);
        Task<int> CountByBookIdAsync(int bookId ,string search);
        Task<Copy?> GetByIdWithRentalsAsync(int id);
        Task<bool> HasRentalHistoryAsync(int copyId);

        Task DeleteAsync(Copy copy);
    }
}
