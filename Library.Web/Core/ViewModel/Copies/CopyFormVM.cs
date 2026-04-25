using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace Library.Web.Core.ViewModel.Copies
{
    public class CopyFormVM
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Copy name is required")]
        [StringLength(200, ErrorMessage = "Name cannot exceed 200 characters")]
        [Display(Name = "Copy Name")]
        [Remote(action: "IsNameAvaliableWithThisBook", controller: "AdminCopies", AdditionalFields = "Id,BookId", ErrorMessage = "Name is already taken")]
        public string Name { get; set; } = null!;

        [Required(ErrorMessage = "Please select a book")]
        [Display(Name = "Book")]
        public int BookId { get; set; }

        [Display(Name = "Allow to Rental")]
        public bool AllowToRental { get; set; } = true;

        public IEnumerable<SelectListItem> Books { get; set; } = [];

        public string? BookTitle { get; set; }

    }
}
