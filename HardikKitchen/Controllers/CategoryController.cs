using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Taste.DataAccess.Data.Repository.IRepository;

namespace HardikKitchen.Controllers
{
    /// <summary>
    /// this is Category controller to handle get and delete requste of 
    /// category model.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public CategoryController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        /// GET Requste For Category
        /// </summary>
        /// <returns>json</returns>
        [HttpGet]
        public IActionResult Get()
        {
            return Json(new { data = _unitOfWork.Category.GetAll() });

        }

        /// <summary>
        /// Delete Requste for Category
        /// </summary>
        /// <param name="id"></param>
        /// <returns>stutas of message</returns>
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var ObjFromDb = _unitOfWork.Category.GetFirstOrDefault(u => u.Id == id);
            if (ObjFromDb == null)
            {
                return Json(new { success = false, Message = "Error While Deleting" });
            }
            _unitOfWork.Category.Remove(ObjFromDb);
            _unitOfWork.Save();
            return Json(new { success = true, message = "Delete Successful" });
        }

    }
}