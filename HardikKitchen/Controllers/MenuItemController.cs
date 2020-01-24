using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Taste.DataAccess.Data.Repository.IRepository;

namespace HardikKitchen.Controllers
{

    /// <summary>
    ///  this is MenuItem controller to handle get and delete requste of 
    /// MenuItem model.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class MenuItemController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public MenuItemController(IUnitOfWork unitOfWork,IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;
        }

        /// <summary>
        /// GET Requste For MenuItem
        /// </summary>
        /// <returns>json</returns>
        [HttpGet]
        public IActionResult Get()
        {
            return Json(new { data = _unitOfWork.MenuItem.GetAll(null,null,"Category,FoodType") });

        }


        /// <summary>
        /// Delete Requste for MenuItem
        /// </summary>
        /// <param name="id"></param>
        /// <returns>stutas of message</returns>
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
                var ObjFromDb = _unitOfWork.MenuItem.GetFirstOrDefault(u => u.Id == id);
                if (ObjFromDb == null)
                {
                    return Json(new { success = false, Message = "Error While Deleting" });
                }
                var imagePath = Path.Combine(_webHostEnvironment.WebRootPath, ObjFromDb.Image.TrimStart('\\'));
                if (System.IO.File.Exists(imagePath))
                {
                    System.IO.File.Delete(imagePath);
                }
                _unitOfWork.MenuItem.Remove(ObjFromDb);
                _unitOfWork.Save();

            }
            catch(Exception)
            {
                return Json(new { success = false, Message = "Error While Deleting" });
            }
            return Json(new { success = true, message = "Delete Successful" });
        }
    }
}