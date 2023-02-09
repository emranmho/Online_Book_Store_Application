using Microsoft.AspNetCore.Mvc;
using Online_Book_Store_Application.Models.Models;
using Online_Book_Store_Application.Repository.IRepository;

namespace Online_Book_Store_Application_Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class BookAuthorController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public BookAuthorController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<IActionResult> Index()
        {
            IEnumerable<BookAuthor> bookAuthors = _unitOfWork.BookAuthor.GetAll();
            return View(bookAuthors);
        }
        public async Task<IActionResult> Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(BookAuthor bookAuthor)
        {
            if ( _unitOfWork.BookAuthor.Get(x => x.AuthorSerialId == bookAuthor.AuthorSerialId) != null)
            {
                ModelState.AddModelError("serialnumber", "Serial Number Must be Unique");
            }
            if (ModelState.IsValid)
            {
                await _unitOfWork.BookAuthor.Add(bookAuthor);
                _unitOfWork.Save();
                TempData["success"] = "Author create successfully";
                return RedirectToAction("Index");
            }
            return View(bookAuthor);
        }

        public async Task<IActionResult> Edit(int? id)
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
        [HttpPost]
        public async Task<IActionResult> Edit(BookAuthor bookAuthor)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.BookAuthor.Update(bookAuthor);
                _unitOfWork.Save();
                TempData["success"] = "Author update successfully";
                return RedirectToAction("Index");
            }
            return View(bookAuthor);
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
        [HttpPost]
        public async Task<IActionResult> DeletePost(int? id)
        {
            var data =  _unitOfWork.BookAuthor.Get(x => x.Id == id);
            if (data == null)
            {
                return NotFound();
            }

            _unitOfWork.BookAuthor.Remove(data);
            _unitOfWork.Save();
            TempData["success"] = "Author Delete successfully";
            return RedirectToAction("Index");

        }
    }
}
