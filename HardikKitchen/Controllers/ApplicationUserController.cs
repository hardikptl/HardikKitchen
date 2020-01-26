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
    public class ApplicationUserController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public ApplicationUserController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        /// GET Requste For ApplicationUser
        /// <returns>json</returns>
        [HttpGet]
        public IActionResult Get()
        {
            return Json(new { data = _unitOfWork.ApplicationUser.GetAll() });

        }

        /// <summary>
        /// LockUnlock Requste forApplicationUser
        /// </summary>
        /// <param name="id"></param>
        /// <returns>stutas of message</returns>
        [HttpPost]
        public IActionResult LockUnlock([FromBody] string id)
        {
            var ObjFromDb = _unitOfWork.ApplicationUser.GetFirstOrDefault(u => u.Id == id);
            if (ObjFromDb == null)
            {
                return Json(new { success = false, Message = "Error While Locking/Unlocking" });
            }
            if(ObjFromDb.LockoutEnd!=null && ObjFromDb.LockoutEnd > DateTime.Now)
            {
                //user alredy locked
                ObjFromDb.LockoutEnd = DateTime.Now;
            }
            else
            {
                //locking user for 100 years
                ObjFromDb.LockoutEnd = DateTime.Now.AddYears(100);
            }
            
            _unitOfWork.Save();
            return Json(new { success = true, message = "Operation Successful" });
        }

    }
}