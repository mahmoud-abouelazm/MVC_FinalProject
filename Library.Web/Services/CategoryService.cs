using Library.Web.Core.Constants;
using Library.Web.Core.Models;
using Library.Web.Core.ViewModel.Category;
using Library.Web.Repository.IRepositories;

namespace Library.Web.Services
{
    public class CategoryService(ICategoryRepository repo) : ICategoryService
    {
        private readonly ICategoryRepository _repo = repo;

        public async Task<PagedResult<CategoryRowVM>> GetAllAsync(PaginationParams param)
        {
            return await _repo.GetAllAsync(param);
        }

        public async Task CreateAsync(CategoryFormVM vm)
        {
            var category = new Category
            {
                Name = vm.Name
            };

            await _repo.AddAsync(category);
        }

        public async Task<CategoryFormVM?> GetVmForEditAsync(int id)
        {
            var cat = await _repo.GetByIdAsync(id);

            if (cat == null)
                return null;

            return new CategoryFormVM
            {
                Id = cat.Id,
                Name = cat.Name
            };
        }

        public async Task UpdateAsync(CategoryFormVM vm)
        {
            var cat = await _repo.GetByIdAsync(vm.Id);

            if (cat == null)
                throw new Exception("Category not found");

            cat.Name = vm.Name;

            await _repo.UpdateAsync(cat); // SaveChanges جوه
        }

        public async Task DeleteAsync(int id)
        {
            var cat = await _repo.GetByIdAsync(id);

            if (cat == null)
                throw new Exception("Not found");

            _repo.DeleteAsync(cat);

        }
    }
}