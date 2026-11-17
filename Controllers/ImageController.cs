using GR50322.UI.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace GR50322.UI.Controllers
{
    public class ImageController(UserManager<ApplicationUser> userManager, IWebHostEnvironment env) : Controller
    {
        public  async Task <IActionResult> GetAvatar()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var user = await userManager.FindByIdAsync(userId);
            if (user.Avatar != null)
                return File(user.Avatar, "image/*");
           
            return File ("images/default-profile-picture.jpg", "image/*");
            
        }
    }
}
