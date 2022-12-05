using ArtGallery.Core.Contracts;
using ArtGallery.Infrastructure.Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ArtGallery.Controllers
{
    [Authorize]
    public class CartController : Controller
    {
        private readonly IStoreService storeService;
        private readonly UserManager<ApplicationUser> userManager;

        public CartController(IStoreService storeService,
            UserManager<ApplicationUser> userManager)
        {
            this.storeService = storeService;
            this.userManager = userManager;
        }
        public async Task<IActionResult> Cart()
        {
            try
            {
                var user = this.userManager.GetUserId(this.User);
                var cart = await this.storeService.GetCartAsync(user);
                return this.View(cart);
            }
            catch (Exception)
            {
                return base.NotFound();
            }
        }

        public async Task<IActionResult> RemoveFromCart(string id)
        {
            try
            {
                var user = this.userManager.GetUserId(this.User);
                await this.storeService.RemoveFromCartAsync(user, id);
                return RedirectToAction("Cart");
            }
            catch (Exception)
            {
                return base.NotFound();
            }
        }

    }
}
