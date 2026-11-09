using System.ComponentModel.DataAnnotations;

namespace POE_RaceDay_MVC.Models
{
    public class ProfileViewModel
    {
        public int UserID { get; set; }

        public string FirstName { get; set; } = "";

        public string LastName { get; set; } = "";

        public string Email { get; set; } = "";

        public string Role { get; set; } = "";

        public string ProfileImage { get; set; } = "";

        [Display(Name = "Choose Profile Picture")]
        public IFormFile? ImageFile { get; set; }
    }
}
