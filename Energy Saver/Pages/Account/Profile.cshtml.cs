using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;

namespace acme.Pages
{
    public class ProfileModel : PageModel
    {
        public string UserName { get; set; }
        public string UserEmailAddress { get; set; }
        public string UserProfileImage { get; set; }

        public void OnGet()
        {
            UserName = User.FindFirst(c => c.Type == "nickname")?.Value;
            UserEmailAddress = User.FindFirst(c => c.Type == ClaimTypes.Email)?.Value;
            UserProfileImage = User.FindFirst(c => c.Type == "picture")?.Value;
        }
    }
}
