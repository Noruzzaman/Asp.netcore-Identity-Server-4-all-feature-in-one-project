namespace AuthenticationAuthorizationpractice.Models
{
    public class RoleClaimsViewModel
    {
        public RoleClaimsViewModel()
        {
            RCliams = new List<RoleClaim>();
        }

        public string RoleId { get; set; }
        public List<RoleClaim> RCliams { get; set; }
    }
}
