using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Taste.DataAccess.Data.Repository.IRepository;

namespace HardikKitchen.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FoodTypeController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public FoodTypeController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Json(new { data = _unitOfWork.FoodType.GetAll() });

        }

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