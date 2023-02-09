using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Online_Book_Store_Application.Models.ViewModels;
using Online_Book_Store_Application.Repository.IRepository;

namespace Online_Book_Store_Application_Web.Areas.Admin.Controllers;

[Area("Admin")]
public class BookController : Controller
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IWebHostEnvironment _webHostEnvironment;
    public BookController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
    {
        _unitOfWork = unitOfWork;
        _webHostEnvironment = webHostEnvironment;
    }
    public IActionResult Index()
    {
        
        return View();
    }
    //GET
    public IActionResult Upsert(int? id)
    {
        BookVM bookVM = new()
        {
            Book = new(),
            CategoryList = _unitOfWork.Category.GetAll().Select(
            x => new SelectListItem
            {
                Text = x.Name,
                Value = x.Id.ToString()
            }),
            BookAuthorList = _unitOfWork.BookAuthor.GetAll().Select(
            x => new SelectListItem
            {
                Text = x.Name,
                Value = x.Id.ToString()
            })
        };
        
        if (id == null || id == 0)
        {
            //create Book
            return View(bookVM);
        }
        else
        {
            //update Book
            bookVM.Book = _unitOfWork.Book.Get(x=>x.Id == id);
            return View(bookVM);
        }
    }
    [HttpPost]
    public async Task<IActionResult> Upsert(BookVM entity,IFormFile file)
    {
        if (ModelState.IsValid)
        {
            var wwwRootPath = _webHostEnvironment.WebRootPath;
            if(file != null)
            {
                string fileName = Guid.NewGuid().ToString();
                var uploads = Path.Combine(wwwRootPath, @"images\books");
                var extension = Path.GetExtension(file.FileName);
                if(entity.Book.ImageUrl!= null)
                {
                    var oldImagePath = Path.Combine(wwwRootPath, entity.Book.ImageUrl.TrimStart('\\'));
                    if(System.IO.File.Exists(oldImagePath))
                    {
                        System.IO.File.Delete(oldImagePath);
                    }
                }
                using(var fileStream = new FileStream(Path.Combine(uploads,fileName+extension),FileMode.Create))
                {
                    file.CopyTo(fileStream);
                }
                entity.Book.ImageUrl = @"\images\books\" + fileName + extension;
            }
            if(entity.Book.Id == 0)
            {
                await _unitOfWork.Book.Add(entity.Book);
                _unitOfWork.Save();
                TempData["success"] = "Book Created successfully";
            }
            else
            {
                _unitOfWork.Book.Update(entity.Book);
                _unitOfWork.Save();
                TempData["success"] = "Book Updated successfully";
            }
            
           
            return RedirectToAction("Index");
        }
        return View(entity);
    }

    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null || id == 0)
        {
            return NotFound();
        }
        var data =  _unitOfWork.BookAuthor.Get(x => x.Id == id);
        if (data == null)
        {
            return NotFound();
        }
        return View(data);
    }
    

    #region API Calls

    [HttpGet]
    public IActionResult GetAll()
    {
        var bookList = _unitOfWork.Book.GetAll(includeProperties: "Category,BookAuthor");
        return Json(new { data = bookList});
    }

    [HttpDelete]
    public IActionResult DeletePost(int? id)
    {
        var data = _unitOfWork.Book.Get(x => x.Id == id);
        if (data == null)
        {
            return Json(new {success =false, message = "Error while deleting"});
        }

        var oldImagePath = Path.Combine(_webHostEnvironment.WebRootPath, data.ImageUrl.TrimStart('\\'));
        if (System.IO.File.Exists(oldImagePath))
        {
            System.IO.File.Delete(oldImagePath);
        }

        _unitOfWork.Book.Remove(data);
        _unitOfWork.Save();

        return Json(new { success = true, message = "Delete Successfully" });

    }
    #endregion
}
