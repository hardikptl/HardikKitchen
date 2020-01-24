using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Text;

namespace Test.Models.ViewModels
{

    /// <summary>
    /// Viewmodel for Menuitem 
    /// </summary>
    public class MenuItemVM
    {
        // this is for accessing 3 model at same place
        public MenuItem MenuItem { get; set; }
        public IEnumerable<SelectListItem> CategoryList { get; set; }
        public IEnumerable<SelectListItem> FoodTypeList { get; set; }
    }
}