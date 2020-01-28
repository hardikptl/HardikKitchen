using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Taste.DataAccess.Data.Repository.IRepository;
using Test.Models;
using Test.Models.ViewModel;
using Test.Utility;

namespace HardikKitchen.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : Controller
    {

        private readonly IUnitOfWork _unitOfWork;

        public OrderController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        //GETTNG STATUS AS PARAMETER AND IT COULD BE NULL
        //ON BASE OF STATUS WILL DISPALY DATA IN ORDERPICKUP
        [HttpGet]
        public IActionResult Get(string status = null)
        {
            List<OrderDetailsVM> orderListVM = new List<OrderDetailsVM>();

            IEnumerable<OrderHeader> OrderHeaderList;

            if (User.IsInRole(SD.CustomerRole))
            {
                //retrieve all orders for that customer
                var claimsIdentity = (ClaimsIdentity)User.Identity;
                var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
                OrderHeaderList = _unitOfWork.OrderHeader.GetAll(u => u.UserId == claim.Value, null, "ApplicationUser");
            }
            else
            {
                OrderHeaderList = _unitOfWork.OrderHeader.GetAll(null, null, "ApplicationUser");
            }
            //IF STATUS CNACELLED
            if (status == "cancelled")
            {
                OrderHeaderList = OrderHeaderList.Where(o => o.Status == SD.StatusCancelled || o.Status == SD.StatusRefunded || o.Status == SD.PaymentStatusRejected);
            }
            else
            {
                //IF STATUS COMPLETED
                if (status == "completed")
                {
                    OrderHeaderList = OrderHeaderList.Where(o => o.Status == SD.StatusCompleted);
                }
                //IF STATUS PENDING
                else
                {
                    OrderHeaderList = OrderHeaderList.Where(o => o.Status == SD.StatusReady || o.Status == SD.StatusInProcess || o.Status == SD.StatusSubmitted || o.Status == SD.PaymentStatusPending);
                }
            }

            foreach (OrderHeader item in OrderHeaderList)
            {
                OrderDetailsVM individual = new OrderDetailsVM
                {
                    OrderHeader = item,
                    OrderDetails = _unitOfWork.OrderDetails.GetAll(o => o.OrderId == item.Id).ToList()
                };
                orderListVM.Add(individual);
            }

            return Json(new { data = orderListVM });
        }
    }
}