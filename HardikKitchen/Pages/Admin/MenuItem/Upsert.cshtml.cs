using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Taste.DataAccess.Data.Repository.IRepository;
using Test.Models;
using Test.Models.ViewModel;


namespace HardikKitchen.Pages.Admin.MenuItem
{   /// <summary>
    /// this class handle edit and delete 
    /// requste from the menu item list page
    /// </summary>
    public class UpsertModel : PageModel
    {

        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webHostEnvironment;

        /// <summary>
        /// constructor to intialize below parameter.
        /// </summary>
        /// <param name="unitOfWork"></param>
        /// <param name="webHostEnvironment"></param>
        public UpsertModel(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;
        }
        //binding menu item view model for 3 property from 3 tables
        [BindProperty]
        public MenuItemVM MenuItemObj { get; set; }


        /// <summary>
        /// this is for edit requste from menu item list
        /// </summary>
        /// <param name="id"></param>
        /// <returns>list of Item</returns>
        public IActionResult OnGet(int? id)
        {   //this obj come from menuitem view model so we can bind multiple 
            //model proteries 
            MenuItemObj = new MenuItemVM
            {
                CategoryList = _unitOfWork.Category.GetCategoryListForDropDown(),
                FoodTypeList = _unitOfWork.FoodType.GetFoodTypeListForDropDown()
            };
            if (id != null)
            {
                MenuItemObj.MenuItem = _unitOfWork.MenuItem.GetFirstOrDefault(u => u.Id == id);
                if (MenuItemObj.MenuItem == null)
                {
                    return NotFound();
                }
            }
            return Page();
        }


        /// <summary>
        /// this handle on post requste from page.
        /// </summary>
        /// <returns></returns>
        public IActionResult OnPost()
        {   //this store path up to www.root folder
            string webRootPath = _webHostEnvironment.WebRootPath;
            var files = HttpContext.Request.Form.Files;

            if (!ModelState.IsValid)
            {
                return Page();
            }
            if (MenuItemObj.MenuItem.Id == 0)
            {

                string fileName = Guid.NewGuid().ToString(); //creting new guid
                var uploads = Path.Combine(webRootPath, @"images\menuItems"); //upload given path
                var extension = Path.GetExtension(files[0].FileName); //getting extension of file

                using (var fileStream = new FileStream(Path.Combine(uploads, fileName + extension), FileMode.Create))
                {
                    files[0].CopyTo(fileStream);
                }
                MenuItemObj.MenuItem.Image = @"\image\menuItems\" + fileName + extension;
                _unitOfWork.MenuItem.Add(MenuItemObj.MenuItem);

            }
            else
            {
                //Edit Menu Item
                var objFromDb = _unitOfWork.MenuItem.Get(MenuItemObj.MenuItem.Id);
                if (files.Count == 0)
                {
                    string fileName = Guid.NewGuid().ToString(); //creting new guid
                    var uploads = Path.Combine(webRootPath, @"images\menuItems"); //upload given path
                    var extension = Path.GetExtension(files[0].FileName); //getting extension of file

                    var imagePath = Path.Combine(webRootPath, objFromDb.Image.TrimStart('\\'));

                    //deleting exsting image file if there one
                    if (System.IO.File.Exists(imagePath))
                    {
                        System.IO.File.Delete(imagePath);
                    }


                    using (var fileStream = new FileStream(Path.Combine(uploads, fileName + extension), FileMode.Create))
                    {
                        files[0].CopyTo(fileStream);
                    }
                    MenuItemObj.MenuItem.Image = @"\image\menuItems\" + fileName + extension;
                    _unitOfWork.MenuItem.Add(MenuItemObj.MenuItem);
                }
                else
                {
                    MenuItemObj.MenuItem.Image = objFromDb.Image;
                }
              
                _unitOfWork.MenuItem.Update(MenuItemObj.MenuItem);
            }

            _unitOfWork.Save();
            return RedirectToPage("./Index");
        }
    }
}

