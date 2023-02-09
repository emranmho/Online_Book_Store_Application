using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Online_Book_Store_Application.Models.Models;
using Online_Book_Store_Application.Repository.IRepository;

namespace Online_Book_Store_Application_Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoryController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public CategoryController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IActionResult> IndexAsync()
        {
            IEnumerable<Category> category = _unitOfWork.Category.GetAll();
            return View(category);
        }

        public async Task<IActionResult> Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(Category category)
        {
            if ( _unitOfWork.Category.Get(x => x.SerialNumber == category.SerialNumber) != null)
            {
                ModelState.AddModelError("serialnumber", "Serial Number Must be Unique");
            }
            if (ModelState.IsValid)
            {
                await _unitOfWork.Category.Add(category);
                _unitOfWork.Save();
                TempData["success"] = "Category create successfully";
                return RedirectToAction("Index");
            }
            return View(category);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var data =  _unitOfWork.Category.Get(x => x.Id == id);
            if (data == null)
            {
                return NotFound();
            }
            return View(data);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(Category category)
        {
            //if (await _context.Categories.FirstOrDefaultAsync(x => x.SerialNumber == category.SerialNumber) != null)
            //{
            //    ModelState.AddModelError("serialnumber", "Serial Number Must be Unique");
            //}
            if (ModelState.IsValid)
            {
                _unitOfWork.Category.Update(category);
                _unitOfWork.Save();
                TempData["success"] = "Category update successfully";
                return RedirectToAction("Index");
            }
            return View(category);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var data =  _unitOfWork.Category.Get(x => x.Id == id);
            if (data == null)
            {
                return NotFound();
            }
            return View(data);
        }
        [HttpPost]
        public async Task<IActionResult> DeletePost(int? id)
        {
            var data =  _unitOfWork.Category.Get(x => x.Id == id);
            if (data == null)
            {
                return NotFound();
            }

            _unitOfWork.Category.Remove(data);
            _unitOfWork.Save();
            TempData["success"] = "Category Delete successfully";
            return RedirectToAction("Index");

        }
    }
}
