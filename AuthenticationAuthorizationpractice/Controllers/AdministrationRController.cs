using AuthenticationAuthorizationpractice.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace AuthenticationAuthorizationpractice.Controllers
{
    public class AdministrationRController : Controller
    {

        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<IdentityUser> _userManager;
        IEmployeetRepository _employeetRepository;

        public AdministrationRController(RoleManager<IdentityRole> roleManager,
        UserManager<IdentityUser> userManager,
        IEmployeetRepository employeetRepository)
        {
            this._roleManager = roleManager;
            this._userManager = userManager;
            this._employeetRepository = employeetRepository;
        }
      
       
        [HttpGet]
        public async Task<IActionResult> ManageRoleClaims(string roleId)
        {
            var role = await _roleManager.FindByIdAsync(roleId);

            if (role == null)
            {
                ViewBag.ErrorMessage = $"User with Id = {roleId} cannot be found";
                return View("NotFound");
            }

            // UserManager service GetClaimsAsync method gets all the current claims of the user
            var existingRoleClaims = await _roleManager.GetClaimsAsync(role);

            var model = new RoleClaimsViewModel
            {
                RoleId = roleId
            };

            // Loop through each claim we have in our application
            foreach (Claim claim in ClaimsStore.AllClaims)
            {
                RoleClaim roleClaim = new RoleClaim
                {
                    ClaimType = claim.Type
                };

                // If the user has the claim, set IsSelected property to true, so the checkbox
                if (existingRoleClaims.Any(c => c.Type == claim.Type))
                {
                    roleClaim.IsSelected = true;
                }

                model.RCliams.Add(roleClaim);
            }

            return View(model);

        }


        [HttpPost]
        public async Task<IActionResult> ManageRoleClaims(RoleClaimsViewModel model)
        {
            var role = await _roleManager.FindByIdAsync(model.RoleId);

            if (role == null)
            {
                ViewBag.ErrorMessage = $"User with Id = {model.RoleId} cannot be found";
                return View("NotFound");
            }
            // Get all the user existing claims and delete them
            var claims = await _roleManager.GetClaimsAsync(role);
            int x = claims.Count;
            int  result = 0;
            if (claims !=null)
            {
                for (int i=0;i< x; i++)
                {
                    Claim claim12 = claims[i];
                    await _roleManager.RemoveClaimAsync(role, claim12);
                    result = 1;
                }
            }
            var existingRoleClaims = model.RCliams;
            foreach (Claim claim in ClaimsStore.AllClaims)
            {
                foreach (var claimvar in existingRoleClaims)
                {
                  if(claim.Type == claimvar.ClaimType && claimvar.IsSelected==true)
                  {
                    await _roleManager.AddClaimAsync(role, claim);
                  }

                }
            }

            return RedirectToAction("ListRoles", "Role");

        }


    }
}
