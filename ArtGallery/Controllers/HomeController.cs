using ArtGallery.Core.Constants;
using ArtGallery.Core.Contracts;
using ArtGallery.Core.Models;
using ArtGallery.Infrastructure.Data.Models;
using ArtGallery.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace ArtGallery.Controllers
{
    public class HomeController : BaseController
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IServicesService _servicesService;
        private readonly UserManager<ApplicationUser> userManager;

        public HomeController(ILogger<HomeController> logger,
            IServicesService servicesService,
            UserManager<ApplicationUser> userManager)
        {
            this._logger = logger;
            this._servicesService = servicesService;
            this.userManager = userManager;
        }

        //public IActionResult Welcome()
        //{
        //    return this.View();
        //}

        public async Task<IActionResult> Services()
        {
            IEnumerable<ServiceModel> services = await this._servicesService.GetServicesAsync();
            return this.View(services);
        }

        public IActionResult Index()
        {
            //ViewData[MessageConstant.SuccessMesssage] = "Successfully logged in";

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}