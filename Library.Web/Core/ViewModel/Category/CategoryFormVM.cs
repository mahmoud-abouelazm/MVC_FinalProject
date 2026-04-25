using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Library.Web.Core.ViewModel.Category
{
    public class CategoryFormVM
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Name is required")]
        [StringLength(100)]
        [Remote(action: "IsNameAvailable", controller: "Categories", AdditionalFields = "Id", ErrorMessage = "Name is already taken")]
        public string Name { get; set; } = null!;
    }
}
