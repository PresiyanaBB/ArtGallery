using ArtGallery.Core.Contracts;
using ArtGallery.Infrastructure.Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ArtGallery.Controllers
{
    [Authorize]
    public class ManageController : Controller
    {
        private IStoreService storeService;
        private UserManager<ApplicationUser> userManager;

        public ManageController(IStoreService storeService,
            UserManager<ApplicationUser> userManager)
        {
            this.storeService = storeService;
            this.userManager = userManager;
        }

        public async Task<IActionResult> Orders()
        {
            var user = await this.userManager.GetUserAsync(this.User);
            var orders = await this.storeService.GetOrdersByUserAsync(user.Id);

            return View(orders);
        }
    }
}