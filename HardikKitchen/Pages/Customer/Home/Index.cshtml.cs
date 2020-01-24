using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Taste.DataAccess.Data.Repository.IRepository;
using Test.Models;

namespace HardikKitchen.Pages.Customer.Home
{
    public class IndexModel : PageModel
    {
        private readonly IUnitOfWork _unitOfWork;

        /// <summary>
        /// intialize IUnitOfwork
        /// </summary>
        /// <param name="unitOfWork"></param>
        public IndexModel(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        /// Getting list of Properties using list model
        /// </summary>
        public IEnumerable<MenuItem> MenuItemList { get; set; }
        public IEnumerable<Category> CategoryList { get; set; }


        /// <summary>
        /// display menuitem and category as displayorder using
        /// order by property that define in Repository model and Category Model
        /// </summary>
        public void OnGet()
        {
            MenuItemList = _unitOfWork.MenuItem.GetAll(null, null, "Category,FoodType");
            CategoryList = _unitOfWork.Category.GetAll(null, q => q.OrderBy(c => c.DisplayOrder), null);
        }
    }
}