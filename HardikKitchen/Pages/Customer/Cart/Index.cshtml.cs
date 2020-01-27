using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Taste.DataAccess.Data.Repository.IRepository;
using Test.Models;
using Test.Models.ViewModel;

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
    }
}