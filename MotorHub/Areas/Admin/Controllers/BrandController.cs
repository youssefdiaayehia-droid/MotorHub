using Microsoft.AspNetCore.Mvc;
using MotorHub.Areas.Customer.Controllers;
using MotorHub_DataAccess.Repository.IRepository;
using MotorHub_Models;
using Microsoft.AspNetCore.Authorization;

namespace MotorHub.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class BrandController : Controller
    {
        private readonly IUnitOfWork unitOfWork;
        public BrandController(IUnitOfWork _unitOfWork)
        {
            unitOfWork = _unitOfWork;
        }
        public IActionResult Index()
        {
            var Brand = unitOfWork.Brand.GetAll().ToList();
            return View(Brand);
        }
        public IActionResult Upsert(int? id)
        {
            if (id == 0 || id == null)
            {
                return View(new Brand());
            }
            else
            {
                var Brand = unitOfWork.Brand.Get(u => u.Brandid == id);
                if (Brand == null)
                {
                    return NotFound();
                }
                return View(Brand); 
            }
        }
        [HttpPost]
        public IActionResult Upsert(Brand brand)
        {
            if (ModelState.IsValid)
            {
                if (brand.Brandid == 0)
                {
                    unitOfWork.Brand.Add(brand);
                    TempData["success"] = "Brand created successfully";
                }
                else
                {
                    unitOfWork.Brand.Update(brand);
                    TempData["success"] = "Brand updated successfully";
                }
                unitOfWork.Save();
                return RedirectToAction("Index");
            }
            return View(brand); 
        }
        public IActionResult Delete(int? id)
        {
            if (id == 0 || id == null)
            {
                return NotFound();
            }
                var brand = unitOfWork.Brand.Get(u => u.Brandid == id);
            if (brand == null)
            {
                return NotFound();
            }
            return View(brand);

        }
        [HttpPost, ActionName("Delete")]
        public IActionResult DeletePost(int? id)
        {
            var brand = unitOfWork.Brand.Get(u => u.Brandid == id);
            if (brand == null )
            {
                return NotFound();
            }
            unitOfWork.Brand.Remove(brand);
            unitOfWork.Save();
            TempData["success"] = "Brand deleted successfully";
            return RedirectToAction("Index");

        }
    }
}
