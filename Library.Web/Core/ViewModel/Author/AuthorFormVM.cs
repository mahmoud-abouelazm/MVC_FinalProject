using System.ComponentModel.DataAnnotations;

namespace Library.Web.Core.ViewModel.Author
{
    public class AuthorFormVM
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Name is required")]
        [StringLength(150)]
        public string Name { get; set; } = null!;

        [StringLength(1000)]
        public string? Bio { get; set; }
    }
}
