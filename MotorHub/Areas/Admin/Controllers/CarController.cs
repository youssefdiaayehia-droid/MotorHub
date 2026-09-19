using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MotorHub_DataAccess.Repository.IRepository;
using MotorHub_Models;

namespace MotorHub.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class CarController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public CarController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;
        }

        public IActionResult Index()
        {
            var cars = _unitOfWork.Car.GetAll().ToList();
            return View(cars);
        }

        public IActionResult Upsert(int? id)
        {
            if (id == null || id == 0)
            {
                return View(new Car());
            }
            else
            {
                var car = _unitOfWork.Car.Get(u => u.Carid == id);
                if (car == null) return NotFound();
                return View(car);
            }
        }

        [HttpPost]
        public IActionResult Upsert(Car car, IFormFile? file)
        {
            if (ModelState.IsValid)
            {
                string wwwRootPath = _webHostEnvironment.WebRootPath;

                // لو فيه صورة جديدة
                if (file != null)
                {
                    // اسم فريد للصورة
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    string carPath = Path.Combine(wwwRootPath, "images", "cars");

                    // اعمل الفولدر لو مش موجود
                    if (!Directory.Exists(carPath))
                        Directory.CreateDirectory(carPath);

                    // امسح الصورة القديمة (في حالة التعديل)
                    if (!string.IsNullOrEmpty(car.CarImage))
                    {
                        var oldImagePath = Path.Combine(
                            wwwRootPath,
                            car.CarImage.TrimStart('\\', '/'));

                        if (System.IO.File.Exists(oldImagePath))
                            System.IO.File.Delete(oldImagePath);
                    }

                    // احفظ الصورة الجديدة
                    using (var fileStream = new FileStream(Path.Combine(carPath, fileName), FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                    }

                    // سجل المسار (بـ / مش \)
                    car.CarImage = "/images/cars/" + fileName;
                }

                if (car.Carid == 0)
                {
                    _unitOfWork.Car.Add(car);
                    TempData["success"] = "Car created successfully";
                }
                else
                {
                    _unitOfWork.Car.Update(car);
                    TempData["success"] = "Car updated successfully";
                }
                _unitOfWork.Save();
                return RedirectToAction("Index");
            }
            return View(car);
        }

        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0) return NotFound();

            var car = _unitOfWork.Car.Get(u => u.Carid == id);
            if (car == null) return NotFound();

            return View(car);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeletePOST(int? id)
        {
            var car = _unitOfWork.Car.Get(u => u.Carid == id);
            if (car == null) return NotFound();

            // امسح الصورة
            if (!string.IsNullOrEmpty(car.CarImage))
            {
                var imagePath = Path.Combine(
                    _webHostEnvironment.WebRootPath,
                    car.CarImage.TrimStart('\\', '/'));

                if (System.IO.File.Exists(imagePath))
                    System.IO.File.Delete(imagePath);
            }

            _unitOfWork.Car.Remove(car);
            _unitOfWork.Save();
            TempData["success"] = "Car deleted successfully";
            return RedirectToAction("Index");
        }
    }
}