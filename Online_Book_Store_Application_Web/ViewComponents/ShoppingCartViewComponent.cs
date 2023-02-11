using Microsoft.AspNetCore.Mvc;
using Online_Book_Store_Application.Repository.IRepository;
using Online_Book_Store_Application.Utility;
using System.Security.Claims;

namespace Online_Book_Store_Application_Web.ViewComponents
{
    public class ShoppingCartViewComponent : ViewComponent
    {
        private readonly IUnitOfWork _unitOfWork;
        public ShoppingCartViewComponent(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var claimIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimIdentity.FindFirst(ClaimTypes.NameIdentifier);

            if(claim != null)
            {
                if(HttpContext.Session.GetInt32(StaticDetails.SessionCart) != null)
                {
                    return View(HttpContext.Session.GetInt32(StaticDetails.SessionCart));
                }
                else
                {
                    HttpContext.Session.SetInt32(StaticDetails.SessionCart,
                        _unitOfWork.ShoppingCart.GetAll(u=>u.ApplicationUserId == claim.Value).ToList().Count);
                    return View(HttpContext.Session.GetInt32(StaticDetails.SessionCart));
                }
            }
            else
            {
                HttpContext.Session.Clear();
                return View(0);
            }
        }
    }
}
