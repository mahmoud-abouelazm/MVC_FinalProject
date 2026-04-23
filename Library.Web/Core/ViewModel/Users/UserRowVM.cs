using Library.Web.Core.Models;

namespace Library.Web.Core.ViewModel.Users
{
    public class UserRowVM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
        public string PhoneNumber { get; set; }
        public bool IsActive { get; set; } 


        
    }
}
