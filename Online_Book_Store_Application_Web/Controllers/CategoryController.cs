using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Online_Book_Store_Application.Repository.Repository.IRepository;
using Online_Book_Store_Application.Models;

namespace Online_Book_Store_Application_Web.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ICategoryRepository _categoryRepo;
        public CategoryController(ICategoryRepository categoryRepo)
        {
           _categoryRepo= categoryRepo;
        }

        public async Task<IActionResult> IndexAsync()
        {
            IEnumerable<Category> category = await _categoryRepo.GetAll();
            return View(category);
        }

        public async Task<IActionResult> Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(Category category)
        {
            if(await _categoryRepo.Get(x=>x.SerialNumber == category.SerialNumber) !=null)
            {
                ModelState.AddModelError("serialnumber", "Serial Number Must be Unique");
            }
            if(ModelState.IsValid)
            {
                await _categoryRepo.Add(category);
                _categoryRepo.Save();
                TempData["success"] = "Category create successfully";
                return RedirectToAction("Index");
            }
            return View(category);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if(id == null || id == 0)
            {
                return NotFound();
            }
            var data = await _categoryRepo.Get(x=>x.Id ==id);
            if (data==null)
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
                _categoryRepo.Update(category);
                _categoryRepo.Save();
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
            var data = await _categoryRepo.Get(x=>x.Id==id);
            if (data == null)
            {
                return NotFound();
            }
            return View(data);
        }
        [HttpPost]
        public async Task<IActionResult> DeletePost(int? id)
        {
            var data = await _categoryRepo.Get(x => x.Id == id);
            if (data == null)
            {
                return NotFound();
            }

            _categoryRepo.Remove(data);
            _categoryRepo.Save();
            TempData["success"] = "Category Delete successfully";
            return RedirectToAction("Index");
            
        }
    }
}
