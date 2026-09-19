using Microsoft.AspNetCore.Mvc;
using MotorHub_DataAccess.Repository.IRepository;
using MotorHub_Models.VM;
using System.Security.Claims;
using MotorHub_Models;
using MotorHub_Utility;

namespace MotorHub.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class CartController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public CartController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;

            var cartItems = _unitOfWork.ShoppingCart.GetAll(
                u => u.ApplicationUserId == userId,
                includeProperties: "Car").ToList();

            var vm = new ShoppingCartVM
            {
                ShoppingCartList = cartItems,
                OrderHeader = new OrderHeader
                {
                    OrderTotal = cartItems.Sum(u => u.Car.CarPrice * u.Count)
                }
            };

            return View(vm);
        }

        public IActionResult Plus(int cartId)
        {
            var cartItem = _unitOfWork.ShoppingCart.Get(u => u.Id == cartId);
            cartItem.Count += 1;
            _unitOfWork.ShoppingCart.Update(cartItem);
            _unitOfWork.Save();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Minus(int cartId)
        {
            var cartItem = _unitOfWork.ShoppingCart.Get(u => u.Id == cartId);
            if (cartItem.Count <= 1)
            {
                _unitOfWork.ShoppingCart.Remove(cartItem);
            }
            else
            {
                cartItem.Count -= 1;
                _unitOfWork.ShoppingCart.Update(cartItem);
            }
            _unitOfWork.Save();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Remove(int cartId)
        {
            var cartItem = _unitOfWork.ShoppingCart.Get(u => u.Id == cartId);
            _unitOfWork.ShoppingCart.Remove(cartItem);
            _unitOfWork.Save();
            return RedirectToAction(nameof(Index));
        }
        public IActionResult Summary()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;

            var cartItems = _unitOfWork.ShoppingCart.GetAll(
                u => u.ApplicationUserId == userId,
                includeProperties: "Car").ToList();

            var user = _unitOfWork.ApplicationUser.Get(u => u.Id == userId);

            var vm = new ShoppingCartVM
            {
                ShoppingCartList = cartItems,
                OrderHeader = new OrderHeader
                {
                    Name = user?.Name,
                    PhoneNumber = user?.PhoneNumber,
                    StreetAddress = user?.StreetAddress,
                    City = user?.City,
                    State = user?.State,
                    PostalCode = user?.PostalCode,
                    OrderTotal = cartItems.Sum(u => u.Car.CarPrice * u.Count)
                }
            };

            return View(vm);
        }

        [HttpPost]
        [ActionName("Summary")]
        [ValidateAntiForgeryToken]
        public IActionResult SummaryPOST(ShoppingCartVM vm)
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;

            var cartItems = _unitOfWork.ShoppingCart.GetAll(
                u => u.ApplicationUserId == userId,
                includeProperties: "Car").ToList();

            vm.OrderHeader.ApplicationUserId = userId;
            vm.OrderHeader.OrderDate = DateTime.Now;
            vm.OrderHeader.ShippingDate = DateTime.Now.AddDays(7);
            vm.OrderHeader.OrderTotal = cartItems.Sum(u => u.Car.CarPrice * u.Count);
            vm.OrderHeader.OrderStatus = SD.StatusPending;
            vm.OrderHeader.PaymentStatus = SD.PaymentStatusPending;

            _unitOfWork.OrderHeader.Add(vm.OrderHeader);
            _unitOfWork.Save();

            foreach (var item in cartItems)
            {
                var orderDetail = new OrderDetail
                {
                    CarId = item.CarId,
                    OrderHeaderId = vm.OrderHeader.Id,
                    Price = (double)item.Car.CarPrice,
                    Count = item.Count
                };
                _unitOfWork.OrderDetail.Add(orderDetail);
            }
            _unitOfWork.Save();

            // امسح السلة
            _unitOfWork.ShoppingCart.RemoveRange(cartItems);
            _unitOfWork.Save();

            return RedirectToAction("OrderConfirmation", new { id = vm.OrderHeader.Id });
        }

        public IActionResult OrderConfirmation(int id)
        {
            return View(id);
        }
    }
}
