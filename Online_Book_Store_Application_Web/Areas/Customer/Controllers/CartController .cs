using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Differencing;
using Microsoft.Extensions.Options;
using Online_Book_Store_Application.Models;
using Online_Book_Store_Application.Models.Models;
using Online_Book_Store_Application.Models.ViewModels;
using Online_Book_Store_Application.Repository.IRepository;
using Online_Book_Store_Application.Utility;
using Stripe.Checkout;
using System.Diagnostics;
using System.Security.Claims;

namespace Online_Book_Store_Application_Web.Areas.Customer.Controllers
{
    [Area("Customer")]
    [Authorize]
    public class CartController : Controller
    {
        private readonly ILogger<CartController> _logger;
        [BindProperty]
        public ShoppingCartVM ShoppingCartVM { get; set; }
        public int OrderTotal { get; set; }
        private readonly IUnitOfWork _unitOfWork;
        public CartController(ILogger<CartController> logger,IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            var claimIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimIdentity.FindFirst(ClaimTypes.NameIdentifier);

            ShoppingCartVM = new ShoppingCartVM()
            {
                ListCart = _unitOfWork.ShoppingCart.GetAll(u=>u.ApplicationUserId == claim.Value,includeProperties: "Book"),
                OrderHeader = new()
            };
            foreach(var cart in ShoppingCartVM.ListCart)
            {
                ShoppingCartVM.OrderHeader.OrderTotal += (cart.Count * cart.Book.PriceAfterDiscount);
				
            }
            return View(ShoppingCartVM);
        }

		public IActionResult Summary()
		{
            var claimIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimIdentity.FindFirst(ClaimTypes.NameIdentifier);

            ShoppingCartVM = new ShoppingCartVM()
            {
                ListCart = _unitOfWork.ShoppingCart.GetAll(u => u.ApplicationUserId == claim.Value, includeProperties: "Book"),
				OrderHeader = new()
			};

            ShoppingCartVM.OrderHeader.ApplicationUser = _unitOfWork.ApplicationUser.Get(u => u.Id == claim.Value);

            ShoppingCartVM.OrderHeader.Name = ShoppingCartVM.OrderHeader.ApplicationUser.Name;
			ShoppingCartVM.OrderHeader.PhoneNumber = ShoppingCartVM.OrderHeader.ApplicationUser.PhoneNumber;
			ShoppingCartVM.OrderHeader.City = ShoppingCartVM.OrderHeader.ApplicationUser.Address;
			ShoppingCartVM.OrderHeader.State = ShoppingCartVM.OrderHeader.ApplicationUser.Address;
			ShoppingCartVM.OrderHeader.PostalCode = ShoppingCartVM.OrderHeader.ApplicationUser.Address;

			foreach (var cart in ShoppingCartVM.ListCart)
            {
                ShoppingCartVM.OrderHeader.OrderTotal += (cart.Count * cart.Book.PriceAfterDiscount);

            }
            return View(ShoppingCartVM);
		}
        [HttpPost]
        [ActionName("Summary")]
        [ValidateAntiForgeryToken]
		public IActionResult SummaryPost()
		{
			var claimIdentity = (ClaimsIdentity)User.Identity;
			var claim = claimIdentity.FindFirst(ClaimTypes.NameIdentifier);

			ShoppingCartVM.ListCart = _unitOfWork.ShoppingCart.GetAll(u => u.ApplicationUserId == claim.Value, includeProperties: "Book");

            ShoppingCartVM.OrderHeader.PaymentStatus = StaticDetails.PaymentStatusPending;
			ShoppingCartVM.OrderHeader.OrderStatus = StaticDetails.StatusPending;
            ShoppingCartVM.OrderHeader.OrderDate = DateTime.Now;
            ShoppingCartVM.OrderHeader.ApplicationUserId = claim.Value;

			foreach (var cart in ShoppingCartVM.ListCart)
			{
				ShoppingCartVM.OrderHeader.OrderTotal += (cart.Count * cart.Book.PriceAfterDiscount);

			}
            _unitOfWork.OrderHeader.Add(ShoppingCartVM.OrderHeader);
            _unitOfWork.Save();

			foreach (var cart in ShoppingCartVM.ListCart)
			{
                OrderDetail orderDetail = new()
                {
                    BookId= cart.BookId,
                    OrderId = ShoppingCartVM.OrderHeader.Id,
                    Price = cart.Book.PriceAfterDiscount,
                    Count = cart.Count
				};
                _unitOfWork.OrderDetail.Add(orderDetail);
				_unitOfWork.Save();
			}
            //stripe seetings
            var domain = "https://localhost:7066/";
			var options = new SessionCreateOptions
			{
                PaymentMethodTypes = new List<string>
                {
                    "card",
                },
				LineItems = new List<SessionLineItemOptions>(),
		        
				Mode = "payment",
				SuccessUrl = domain+$"customer/cart/OrderConfirmation?id={ShoppingCartVM.OrderHeader.Id}",
				CancelUrl = domain + $"customer/cart/index",
			};
            foreach(var item in ShoppingCartVM.ListCart)
            {
                var sessionLineItem = new SessionLineItemOptions
                {
                    PriceData = new SessionLineItemPriceDataOptions
                    {
                        UnitAmount = (long)(item.Book.PriceAfterDiscount * 100),//2000,
                        Currency = "bdt",
                        ProductData = new SessionLineItemPriceDataProductDataOptions
                        {
                            Name = item.Book.Title
                        },
                    },
                    Quantity = item.Count,
                };	
                options.LineItems.Add(sessionLineItem); 
            }

			var service = new SessionService();
			Session session = service.Create(options);
            _unitOfWork.OrderHeader.UpdateStripePaymentId(ShoppingCartVM.OrderHeader.Id, session.Id, session.PaymentIntentId);
			_unitOfWork.Save();
			Response.Headers.Add("Location", session.Url);
			return new StatusCodeResult(303);


			//_unitOfWork.ShoppingCart.RemoveRange(ShoppingCartVM.ListCart);
			//_unitOfWork.Save();
			//return RedirectToAction("Index","Home");
		}
		//Visa	4242424242424242	Any 3 digits Any future date
  //      Visa(debit)    4000056655665556	Any 3 digits Any future date

		public IActionResult OrderConfirmation(int id)
        {
            OrderHeader orderHeader = _unitOfWork.OrderHeader.Get(u => u.Id == id);

			var service = new SessionService();
			Session session = service.Get(orderHeader.SessionId);

			//check the stripe status
            if(session.PaymentStatus.ToLower() == "paid")
            {
                _unitOfWork.OrderHeader.UpdateStatus(id, StaticDetails.StatusApproved, StaticDetails.PaymentStatusApproved);
				_unitOfWork.Save();
			}
            List<ShoppingCart> shoppingCarts = _unitOfWork.ShoppingCart.GetAll(u=>u.ApplicationUserId == orderHeader.ApplicationUserId).ToList();
            _unitOfWork.ShoppingCart.RemoveRange(shoppingCarts);
            _unitOfWork.Save();
            return View(id);
        }

		public IActionResult Plus(int cartId)
        {
            var cart = _unitOfWork.ShoppingCart.Get(u=>u.Id == cartId);
            _unitOfWork.ShoppingCart.IncrementUpdate(cart, 1);
            _unitOfWork.Save();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Minus(int cartId)
        {
			var cart = _unitOfWork.ShoppingCart.Get(u => u.Id == cartId);
            if (cart.Count <= 1)
            {
                _unitOfWork.ShoppingCart.Remove(cart);
            }
            else
            {
                _unitOfWork.ShoppingCart.DecrementUpdate(cart, 1);
            }
			_unitOfWork.Save();
			return RedirectToAction(nameof(Index));
		}

        public IActionResult Delete(int cartId)
        {
			var cart = _unitOfWork.ShoppingCart.Get(u => u.Id == cartId);
			_unitOfWork.ShoppingCart.Remove(cart);
			_unitOfWork.Save();
			return RedirectToAction(nameof(Index));
		}

    }       
}