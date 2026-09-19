using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MotorHub_DataAccess.Repository.IRepository;
using MotorHub_Models;
using MotorHub_Models.VM;
using MotorHub_Utility;
using Microsoft.AspNetCore.Authorization;


namespace MotorHub.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class OrderController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public OrderController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            var orders = _unitOfWork.OrderHeader.GetAll(
                includeProperties: "ApplicationUser").ToList();

            return View(orders);
        }

        public IActionResult Details(int id)
        {
            var vm = new OrderVM
            {
                OrderHeader = _unitOfWork.OrderHeader.Get(
                    u => u.Id == id,
                    includeProperties: "ApplicationUser"),
                OrderDetail = _unitOfWork.OrderDetail.GetAll(
                    u => u.OrderHeaderId == id,
                    includeProperties: "Car").ToList()
            };

            return View(vm);
        }

        [HttpPost]
        public IActionResult UpdateOrderStatus(OrderVM vm)
        {
            var orderHeaderFromDb = _unitOfWork.OrderHeader.Get(u => u.Id == vm.OrderHeader.Id);

            orderHeaderFromDb.Name = vm.OrderHeader.Name;
            orderHeaderFromDb.PhoneNumber = vm.OrderHeader.PhoneNumber;
            orderHeaderFromDb.StreetAddress = vm.OrderHeader.StreetAddress;
            orderHeaderFromDb.City = vm.OrderHeader.City;
            orderHeaderFromDb.State = vm.OrderHeader.State;
            orderHeaderFromDb.PostalCode = vm.OrderHeader.PostalCode;

            if (!string.IsNullOrEmpty(vm.OrderHeader.Carrier))
            {
                orderHeaderFromDb.Carrier = vm.OrderHeader.Carrier;
            }

            if (!string.IsNullOrEmpty(vm.OrderHeader.TrackingNumber))
            {
                orderHeaderFromDb.TrackingNumber = vm.OrderHeader.TrackingNumber;
            }

            orderHeaderFromDb.OrderStatus = vm.OrderHeader.OrderStatus;

            if (orderHeaderFromDb.PaymentStatus == SD.PaymentStatusDelayedPayment)
            {
                orderHeaderFromDb.PaymentStatus = SD.PaymentStatusApproved;
            }

            _unitOfWork.OrderHeader.Update(orderHeaderFromDb);
            _unitOfWork.Save();

            TempData["success"] = "Order updated successfully";
            return RedirectToAction(nameof(Details), new { id = orderHeaderFromDb.Id });
        }
    }
}