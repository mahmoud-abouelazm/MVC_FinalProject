using Library.Web.Core.Constants;
using Library.Web.Core.Models;
using Library.Web.Core.ViewModel.Author;
using Library.Web.Repository.IRepositories;

namespace Library.Web.Services
{
    public class AuthorService(IAuthorRepository repo) : IAuthorService
    {
        private readonly IAuthorRepository _repo = repo;

        public async Task<PagedResult<AuthorRowVM>> GetAllAsync(PaginationParams param , string? sortBy = "")
        {
            return await _repo.GetAllAsync(param, sortBy);
        }

        public async Task CreateAsync(AuthorFormVM vm)
        {
            var author = new Author
            {
                Name = vm.Name,
                Bio = vm.Bio
            };

            await _repo.AddAsync(author); 
        }

        public async Task<AuthorFormVM?> GetVmForEditAsync(int id)
        {
            var author = await _repo.GetByIdAsync(id);

            if (author == null)
                return null;

            return new AuthorFormVM
            {
                Id = author.Id,
                Name = author.Name,
                Bio = author.Bio
            };
        }

        public async Task UpdateAsync(AuthorFormVM vm)
        {
            var author = await _repo.GetByIdAsync(vm.Id);

            if (author == null)
                throw new Exception("Author not found");

            author.Name = vm.Name;
            author.Bio = vm.Bio;

            await _repo.UpdateAsync(author); // SaveChanges جوه
        }

        public async Task DeleteAsync(int id)
        {
            var author = await _repo.GetByIdAsync(id);

            if (author == null)
                throw new Exception("Not found");
           
            await _repo.DeleteAsync(author); 
                                            
        }
    }
}
