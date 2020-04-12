using System.ComponentModel.DataAnnotations;

namespace DatingApp.API.DTOs
{
    public class UserRegisterDTO
    {
        [Required]
        public string Username { get; set; }
        
        [Required]
        [StringLength(50, MinimumLength = 5, ErrorMessage = "{0} must be between {2} and {1} characters.")]
        public string Password { get; set; }
    }
}