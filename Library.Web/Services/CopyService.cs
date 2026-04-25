using Library.Web.Core.Models;
using Library.Web.Core.ViewModel.Copies;
using Library.Web.Data;
using Library.Web.Repository.IRepositories;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Library.Web.Services
{
    public class CopyService(ICopyRepository repo, ApplicationDbContext context) : ICopyService
    {
        public async Task<CopyVM> GetIndexVmAsync(int page, int pageSize, string search)
        {
            int total = await repo.CountAsync(search);
            var rows = await repo.GetAllRowsAsync(page, pageSize, search);
            return new CopyVM
            {
                Copies = rows,
                TotalCopies = total,
                CurrentPage = page,
                PageSize = pageSize,
                Search = search
            };
        }

    


        public async Task<CopyVM> GetIndexVmByBookAsync(
           int bookId, int page, int pageSize, string search)
        {
            var book = await context.Books.FindAsync(bookId);
            int total = await repo.CountByBookIdAsync(bookId, search);
            var rows = await repo.GetRowsByBookIdAsync(bookId, page, pageSize, search);
            return new CopyVM
            {
                Copies = rows,
                TotalCopies = total,
                CurrentPage = page,
                PageSize = pageSize,
                Search = search,
                FilterBookId = bookId,
                FilterBookTitle = book?.Title ?? ""
            };
        }


        public async Task<CopyFormVM> GetCreateVmAsync(int? bookId)
        {
            var vm = new CopyFormVM
            {
                AllowToRental = true,
                Books = await BookSelectListAsync()
            };

            if (bookId.HasValue)
            {
                vm.BookId = bookId.Value;
                var book = await context.Books.FindAsync(bookId.Value);
                vm.BookTitle = book?.Title;
            }

            return vm;
        }

        public async Task<ServiceResult> CreateAsync(CopyFormVM vm)
        {
            var copy = new Copy
            {
                Name = vm.Name.Trim(),
                BookId = vm.BookId,
                AllowToRental = vm.AllowToRental
            };

            await repo.AddAsync(copy);

            return new ServiceResult(true, $"Copy '{copy.Name}' added successfully.");
        }


        public async Task<CopyFormVM?> GetEditVmAsync(int id)
        {
            var copy = await repo.GetByIdAsync(id);
            if (copy is null) return null;

            var full = await repo.GetByIdWithRentalsAsync(id);

            return new CopyFormVM
            {
                Id = copy.Id,
                Name = copy.Name,
                BookId = copy.BookId,
                BookTitle = full?.Book.Title,
                AllowToRental = copy.AllowToRental,
                Books = await BookSelectListAsync()
            };
        }

        public async Task<ServiceResult> UpdateAsync(CopyFormVM vm)
        {
            var copy = await repo.GetByIdAsync(vm.Id);
            if (copy is null)
                return new ServiceResult(false, "Copy not found.");

            copy.Name = vm.Name.Trim();
            copy.BookId = vm.BookId;
            copy.AllowToRental = vm.AllowToRental;

            await repo.UpdateAsync(copy);

            return new ServiceResult(true, $"Copy '{copy.Name}' updated successfully.");
        }


        public async Task<ServiceResult> DeleteAsync(int id)
        {
            var copy = await repo.GetByIdWithRentalsAsync(id);
            if (copy is null)
                return new ServiceResult(false, "Copy not found.");

            if (await repo.HasRentalHistoryAsync(id))
                return new ServiceResult(false,
                    $"Cannot delete '{copy.Name}' — it has rental history. Restrict it from rental instead.");

            await repo.DeleteAsync(copy);

            return new ServiceResult(true, $"Copy '{copy.Name}' deleted.");
        }


        public async Task<ServiceResult> ToggleRentalAsync(int id)
        {
            var copy = await repo.GetByIdAsync(id);
            if (copy is null)
                return new ServiceResult(false, "Copy not found.");

            copy.AllowToRental = !copy.AllowToRental;

            await repo.UpdateAsync(copy);

            var label = copy.AllowToRental ? "available for rental" : "restricted from rental";
            return new ServiceResult(true, $"'{copy.Name}' is now {label}.");
        }


        private async Task<IEnumerable<SelectListItem>> BookSelectListAsync()
            => await context.Books
                .Where(b => !b.IsDeleted)
                .OrderBy(b => b.Title)
                .Select(b => new SelectListItem(b.Title, b.Id.ToString()))
                .ToListAsync();

        public async Task<bool> IsNameAvaliableWithThisBookAsync(int bookId, string name, int? copyId)
        {
            return await repo.IsNameAvaliableWithThisBookAsync(bookId, name, copyId);
        }

    }
}
