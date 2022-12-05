using ArtGallery.Core.Contracts;
using ArtGallery.Core.Models;
using ArtGallery.Infrastructure.Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ArtGallery.Controllers
{
    public class StoreController : BaseController
    {
        private const int DefaultPageSize = 4;
        private const int DefaultPage = 1;
        private readonly IStoreService storeService;
        private readonly UserManager<ApplicationUser> userManager;

        public StoreController(IStoreService storeService,
               UserManager<ApplicationUser> userManager)
        {
            this.storeService = storeService;
            this.userManager = userManager;
        }

        public async Task<IActionResult> Explore(int page, int size, string category) //
        {
            if (size <= 0) size = DefaultPageSize;
            if (page <= 0) page = DefaultPage;

            var products = await this.storeService.GetStorePageAsync(page - 1, size, category);
            return this.View(products);
        }
        public async Task<IActionResult> Product(string id, string? errors) //
        {
            if (errors != null)
            {
                return BadRequest(errors);
            }

            var painting = await this.storeService.GetPaintingPageAsync(id);

            return this.View(painting);
        }
        [HttpGet]
        public IActionResult AddProductToCart(string returnUrl)
        {
            return Redirect(returnUrl);
        }
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> AddProductToCart(string paintingId, int quantity) //
        {
            if (quantity == 0) quantity = 1;
            var user = this.userManager.GetUserId(this.User);
            var (success, errors) = await this.storeService.AddPaintingToCartAsync(user, paintingId, quantity);
            if (success)
                return RedirectToAction("Explore");

            return this.RedirectToAction("Product", routeValues: new { paintingId, errors });
        }
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> PlaceOrder(PlaceOrderPaintingModel[]? painting)
        {
            var user = this.userManager.GetUserId(this.User);

            var (success, errors) = await this.storeService.PlaceOrderAsync(painting, user);
            if (!success)
            {
                return BadRequest(errors);
            }

            return RedirectToAction("Index", "Home");
        }

    }
}
