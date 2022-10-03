using Fintech.Library.Entities.Concrete;
using Microsoft.AspNetCore.Mvc;

namespace Fintech.Presentation.WEB.MerchantPanel.Controllers
{
    public class HomeController : Controller
    {
        private readonly IFixerService _fixerService;
        private readonly IUserService _userService;
        private readonly ICurrencyService _currencyService;
        public HomeController(IFixerService fixerService, IUserService userService, ICurrencyService currencyService)
        {
            _fixerService = fixerService;
            _userService = userService;
            _currencyService = currencyService;
        }
        [Authorize]
        public async Task<IActionResult> Index()
        {

            var Result = await _fixerService.GetAllCruncy();
            return View(Result);
        }

        public async Task<IActionResult> CurrencyHistory()
        {

            var Result = await _currencyService.GetAllCurrencyHistory(int.Parse(User.Claims.SingleOrDefault(x => x.Type == "UserId").Value));
            return View(Result);
        }
        [HttpPost]
        public async Task<JsonResult> AddCurrency(CurrencyHistory Model)
        {
            Model.UserId = int.Parse(User.Claims.SingleOrDefault(x => x.Type == "UserId").Value);
            var Result = await _currencyService.AddCurrencyHistory(Model);

            return Json(Result);
        }

    }
}
