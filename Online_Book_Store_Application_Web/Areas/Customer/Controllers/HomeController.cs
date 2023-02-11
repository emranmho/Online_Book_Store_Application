using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Online_Book_Store_Application.Models;
using Online_Book_Store_Application.Models.Models;
using Online_Book_Store_Application.Repository.IRepository;
using Online_Book_Store_Application.Utility;
using System.Diagnostics;
using System.Security.Claims;

namespace Online_Book_Store_Application_Web.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private readonly IUnitOfWork _unitOfWork;
        public HomeController(ILogger<HomeController> logger,IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            IEnumerable<Book> booksList= _unitOfWork.Book.GetAll(includeProperties: "Category,BookAuthor");
            return View(booksList);
        }

        public IActionResult Details(int bookId)
        {
            ShoppingCart cart = new()
            {
                Count = 0,
                BookId = bookId,
                Book = _unitOfWork.Book.Get(x => x.Id == bookId, includeProperties: "Category,BookAuthor")
            };
                
            return View(cart);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public IActionResult Details(ShoppingCart shoppingCart)
        {
            var claimIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimIdentity.FindFirst(ClaimTypes.NameIdentifier);
            shoppingCart.ApplicationUserId = claim.Value;

            ShoppingCart cart = _unitOfWork.ShoppingCart.Get(
                x => x.ApplicationUserId == claim.Value && x.BookId == shoppingCart.BookId);
              
            if(cart == null)
            {
                _unitOfWork.ShoppingCart.Add(shoppingCart);
                _unitOfWork.Save();

                HttpContext.Session.SetInt32(StaticDetails.SessionCart, 
                    _unitOfWork.ShoppingCart.GetAll(u=>u.ApplicationUserId == claim.Value).ToList().Count);
            }
            else
            {
                _unitOfWork.ShoppingCart.IncrementUpdate(cart, shoppingCart.Count);
                _unitOfWork.Save();

            }

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}