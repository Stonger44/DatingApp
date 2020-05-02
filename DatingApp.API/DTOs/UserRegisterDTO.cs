using System.ComponentModel.DataAnnotations;

namespace DatingApp.API.DTOs
{
    public class UserRegisterDTO
    {
        [Required]
        [StringLength(50, ErrorMessage = "{0} must be {1} characters or less.")]
        public string Username { get; set; }
        
        [Required]
        [StringLength(20, MinimumLength = 5, ErrorMessage = "{0} must be between {2} and {1} characters.")]
        public string Password { get; set; }
    }
}