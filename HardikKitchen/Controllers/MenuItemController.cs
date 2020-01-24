using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Taste.DataAccess.Data.Repository.IRepository;

namespace HardikKitchen.Controllers
{
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

        [HttpGet]
        public IActionResult Get()
        {
            return Json(new { data = _unitOfWork.MenuItem.GetAll(null,null,"Category,FoodType") });

        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var ObjFromDb = _unitOfWork.MenuItem.GetFirstOrDefault(u => u.Id == id);
            if (ObjFromDb == null)
            {
                return Json(new { success = false, Message = "Error While Deleting" });
            }
           
            _unitOfWork.MenuItem.Remove(ObjFromDb);
            _unitOfWork.Save();
            return Json(new { success = true, message = "Delete Successful" });
        }
    }
}