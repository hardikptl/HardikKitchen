using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Taste.DataAccess.Data.Repository.IRepository;
using Test.Models;
using Test.Models.ViewModel;
using Test.Utility;

namespace HardikKitchen.Pages.Customer.Cart
{
    public class IndexModel : PageModel
    {
        private readonly IUnitOfWork _unitOfWork;

        public IndexModel(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public OrderDetailsCartVM OrderDetailsCartVM { get; set; }
        /// <summary>
        /// getiing all orderheader
        /// </summary>
        public void OnGet()
        {
            //intializing order 
            OrderDetailsCartVM = new OrderDetailsCartVM()
            {
                OrderHeader = new Test.Models.OrderHeader()
            };
             
            //set orderTotal = 0
            OrderDetailsCartVM.OrderHeader.OrderTotal = 0;

            //ckecking loggedin user value
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            //getting all item from cart as per loggedin user 
            IEnumerable<ShoppingCart> cart = _unitOfWork.ShoppingCart.GetAll(c => c.ApplicationUserId == claim.Value);


            if (cart != null)
            {
                //display all item detail in list 
                OrderDetailsCartVM.listCart = cart.ToList();
            }

            foreach (var cartList in OrderDetailsCartVM.listCart)
            {
                cartList.MenuItem = _unitOfWork.MenuItem.GetFirstOrDefault(m => m.Id == cartList.MenuItemId);
                OrderDetailsCartVM.OrderHeader.OrderTotal += (cartList.MenuItem.Price * cartList.Count);
            }
        }

        /// <summary>
        /// this onPostPlus for plus button post method
        /// </summary>
        /// <param name="cartId"></param>
        /// <returns></returns>
        public IActionResult OnPostPlus(int cartId)
        {
            //getting item by id and adding one value in count of item
            var cart = _unitOfWork.ShoppingCart.GetFirstOrDefault(c => c.Id == cartId);
            _unitOfWork.ShoppingCart.IncrementCount(cart, 1);
            _unitOfWork.Save();
            return RedirectToPage("/Customer/Cart/Index");
        }

        /// <summary>
        /// this is ONPOSTMINUS for minus button post method
        /// </summary>
        /// <param name="cartId"></param>
        /// <returns></returns>
        public IActionResult OnPostMinus(int cartId)
        {
            var cart = _unitOfWork.ShoppingCart.GetFirstOrDefault(c => c.Id == cartId);
            
            if (cart.Count == 1)
            {
                //this is for item quantity is one
                _unitOfWork.ShoppingCart.Remove(cart);
                _unitOfWork.Save();

                var cnt = _unitOfWork.ShoppingCart.
                GetAll(u => u.ApplicationUserId == cart.ApplicationUserId).ToList().Count;
                HttpContext.Session.SetInt32(SD.ShoppingCart, cnt);
            }
            else
            {
                //or decrement count by one
                _unitOfWork.ShoppingCart.DecrementCount(cart, 1);
                _unitOfWork.Save();

            }


            return RedirectToPage("/Customer/Cart/Index");
        }

        /// <summary>
        /// this is ONPOSTRemove for Remove button post method        
        /// <param name="cartId"></param>
        /// <returns></returns>
        public IActionResult OnPostRemove(int cartId)
        {
            var cart = _unitOfWork.ShoppingCart.GetFirstOrDefault(c => c.Id == cartId);
            _unitOfWork.ShoppingCart.Remove(cart);
            _unitOfWork.Save();

            var cnt = _unitOfWork.ShoppingCart.
                               GetAll(u => u.ApplicationUserId == cart.ApplicationUserId).ToList().Count;
            HttpContext.Session.SetInt32(SD.ShoppingCart, cnt);
            return RedirectToPage("/Customer/Cart/Index");
        }
    }
}