using System.ComponentModel.DataAnnotations;

namespace AuthenticationAuthorizationpractice.Models
{
    public class UserRole
    {

        public string UserId { get; set; }
        public string UserName { get; set; }
        public bool IsSelected { get; set; }
    }

}

   