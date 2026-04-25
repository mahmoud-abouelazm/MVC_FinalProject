using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace Library.Web.Core.ViewModel.Book
{
    public class BookFormVM
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Title is required")]
        [StringLength(200)]
        [Remote(action: "IsTitleAvailable", controller: "Admin", ErrorMessage = "Title is already taken")]
        public string Title { get; set; } = null!;

        public string? Description { get; set; }
        public string? Img { get; set; }

        [Required(ErrorMessage = "Price is required")]
        [Range(0.01, 99999.99, ErrorMessage = "Price must be a positive number")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "Category is required")]
        public int CategoryId { get; set; }

        [Required(ErrorMessage = "Select at least one author")]
        public List<int> AuthorIds { get; set; } = [];

        public IEnumerable<SelectListItem> Categories { get; set; } = [];
        public IEnumerable<SelectListItem> Authors { get; set; } = [];
    }
}
