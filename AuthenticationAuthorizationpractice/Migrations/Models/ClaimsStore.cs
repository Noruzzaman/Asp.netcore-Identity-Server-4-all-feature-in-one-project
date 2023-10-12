using System.Security.Claims;

namespace AuthenticationAuthorizationpractice.Models
{
    public static class ClaimsStore
    {
        public static List<Claim> AllClaims = new List<Claim>()
        {
            new Claim("Read Role","Read Role"),
            new Claim("Create Role", "Create Role"),
            new Claim("Edit Role","Edit Role"),
            new Claim("Delete Role","Delete Role")
          
        };


        public static Claim Rolclaim { get; set; }
    }
}
