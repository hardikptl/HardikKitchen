using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Taste.DataAccess.Data.Repository.IRepository;
using Test.Models;
using Test.Utility;

namespace HardikKitchen.Pages.Customer.Home
{   
    [Authorize]
    public class DetailsModel : PageModel
    {
        private readonly IUnitOfWork _unitOfWork;
        public DetailsModel(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [BindProperty]
        public ShoppingCart ShoppingCartObj { get; set; }

        /// <summary>
        /// Getting Detials of Item by item id 
        /// </summary>
        /// <param name="id"></param>
        public void OnGet(int id)
        {
            ShoppingCartObj = new ShoppingCart()
            {
                //filter data by id and getting two propertiesusing include property define in repository class
                MenuItem = _unitOfWork.MenuItem.GetFirstOrDefault(includeProperties: "Category,FoodType", filter: c => c.Id == id),
                MenuItemId =id
            };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IActionResult OnPost()
        {
            if (ModelState.IsValid)
            {
                var claimsIdentity = (ClaimsIdentity)this.User.Identity;
                var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

                ShoppingCartObj.ApplicationUserId = claim.Value;

                ShoppingCart cartFromDb = _unitOfWork.ShoppingCart.GetFirstOrDefault(c => c.ApplicationUserId == ShoppingCartObj.ApplicationUserId &&
                                        c.MenuItemId == ShoppingCartObj.MenuItemId);

                //add directly if there is no item in cart
                if (cartFromDb == null)
                {
                    _unitOfWork.ShoppingCart.Add(ShoppingCartObj);
                }
                //if same item exsit in cart if cx add that same count then 
                //increment count to exsiting count of that same item
                else
                {

                    _unitOfWork.ShoppingCart.IncrementCount(cartFromDb, ShoppingCartObj.Count);
                }
                _unitOfWork.Save();

                //
                var count = _unitOfWork.ShoppingCart.GetAll(c => c.ApplicationUserId == ShoppingCartObj.ApplicationUserId).ToList().Count;
                HttpContext.Session.SetInt32(SD.ShoppingCart, count);
                return RedirectToPage("Index");

            }
            //retun back to page 
            else
            {
                ShoppingCartObj.MenuItem = _unitOfWork.MenuItem.GetFirstOrDefault(includeProperties: "Category,FoodType", filter: c => c.Id == ShoppingCartObj.MenuItemId);
                return Page();

            }
        }
    }
}