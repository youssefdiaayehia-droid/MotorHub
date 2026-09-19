using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MotorHub_DataAccess.Repository.IRepository;
using MotorHub_Models;
using MotorHub_Models.VM;
using System.Security.Claims;

namespace MotorHub.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> logger;
        private readonly IUnitOfWork unitOfWork;
        public HomeController(ILogger<HomeController> _logger , IUnitOfWork _unitOfWork)
        {
            logger = _logger;
            unitOfWork = _unitOfWork;
        }
        public IActionResult Index()
        {
            CustomerHomeVm customerHomeVm = new()
            {
                CarList = unitOfWork.Car.GetAll(),
                BrandList = unitOfWork.Brand.GetAll().Select(u => new SelectListItem
                {
                    Text = u.BrandName,
                    Value = u.Brandid.ToString()
                })
            };

            return View(customerHomeVm);
        }
        public IActionResult Details(int Carid)
        {
            ShoppingCart cart = new()
            {
                Car = unitOfWork.Car.Get(u => u.Carid == Carid),
                CarId = Carid,
                Count = 1
            };
            if (cart.Car == null)
                return NotFound();

            return View(cart);

        }
        [HttpPost]
        [ActionName("Details")]
        public IActionResult AddToCart(ShoppingCart shoppingCart)
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;

            shoppingCart.ApplicationUserId = userId;

            var cartFromDb = unitOfWork.ShoppingCart.Get(
                u => u.ApplicationUserId == userId
                     && u.CarId == shoppingCart.CarId);

            if (cartFromDb != null)
            {
                cartFromDb.Count += shoppingCart.Count;
                unitOfWork.ShoppingCart.Update(cartFromDb);
            }
            else
            {
                unitOfWork.ShoppingCart.Add(shoppingCart);
            }
            unitOfWork.Save();
            TempData["success"] = "Successfully added to the cart.";
            return RedirectToAction(nameof(Index));
        }
    }
}
