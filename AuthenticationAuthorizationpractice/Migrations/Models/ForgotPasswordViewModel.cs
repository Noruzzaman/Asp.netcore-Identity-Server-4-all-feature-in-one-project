using System.ComponentModel.DataAnnotations;

namespace AuthenticationAuthorizationpractice.Models
{
    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
