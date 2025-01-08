using System.ComponentModel.DataAnnotations;

namespace FarmersConnect.Core.Models
{
    public class SignInRequest
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }
}