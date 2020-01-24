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
    /// this is FoodType controller to handle get and delete requste of 
    /// FoodType model.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class FoodTypeController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public FoodTypeController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        /// GET Requste For FoodType
        /// </summary>
        /// <returns>json</returns>
        [HttpGet]
        public IActionResult Get()
        {
            return Json(new { data = _unitOfWork.FoodType.GetAll() });

        }


        /// <summary>
        /// Delete Requste for FoodType
        /// </summary>
        /// <param name="id"></param>
        /// <returns>stutas of message</returns>
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var ObjFromDb = _unitOfWork.FoodType.GetFirstOrDefault(u => u.Id == id);
            if (ObjFromDb == null)
            {
                return Json(new { success = false, Message = "Error While Deleting" });
            }
            _unitOfWork.FoodType.Remove(ObjFromDb);
            _unitOfWork.Save();
            return Json(new { success = true, message = "Delete Successful" });
        }
    }
}