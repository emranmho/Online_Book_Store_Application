using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Online_Book_Store_Application.Models.Models;
using Online_Book_Store_Application.Repository.IRepository;
using Online_Book_Store_Application.Utility;

namespace Online_Book_Store_Application_Web.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = StaticDetails.Role_Admin)]
public class CompanyController : Controller
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IWebHostEnvironment _webHostEnvironment;
    public CompanyController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
    {
        _unitOfWork = unitOfWork;
        _webHostEnvironment = webHostEnvironment;
    }
    public IActionResult Index()
    {
        //IEnumerable<Company> company = _unitOfWork.Company.GetAll();
        return View();
    }
    //GET
    public IActionResult Upsert(int? id)
    {
        Company company = new();
        if (id == null || id == 0)
        {
            //create Book
            return View(company);
        }
        else
        {
            //update Book
           company = _unitOfWork.Company.Get(x=>x.Id == id);
            return View(company);
        }
    }
    [HttpPost]
    public async Task<IActionResult> Upsert(Company entity)
    {
        if (ModelState.IsValid)
        {
            if(entity.Id == 0)
            {
                await _unitOfWork.Company.Add(entity);
                _unitOfWork.Save();
                TempData["success"] = "Company Added successfully";
            }
            else
            {
                _unitOfWork.Company.Update(entity);
                _unitOfWork.Save();
                TempData["success"] = "Company updated successfully";
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
        var companyList = _unitOfWork.Company.GetAll();
        return Json(new { data = companyList });
    }

    [HttpDelete]
    public IActionResult DeletePost(int? id)
    {
        var data = _unitOfWork.Company.Get(x => x.Id == id);
        if (data == null)
        {
            return Json(new {success =false, message = "Error while deleting"});
        }

        _unitOfWork.Company.Remove(data);
        _unitOfWork.Save();

        return Json(new { success = true, message = "Delete Successfully" });

    }
    #endregion
}
