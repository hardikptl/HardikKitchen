using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Stripe;
using Taste.DataAccess.Data.Repository.IRepository;
using Test.Models;
using Test.Models.ViewModel;
using Test.Utility;

namespace HardikKitchen.Pages.Admin.Order
{
    public class ManageOrderModel : PageModel
    {
        private readonly IUnitOfWork _unitOfWork;

        public ManageOrderModel(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [BindProperty]
        public List<OrderDetailsVM> orderDetailsVM { get; set; }


        public void OnGet()
        {
            orderDetailsVM = new List<OrderDetailsVM>();

            List<OrderHeader> OrderHeaderList = _unitOfWork.OrderHeader
                .GetAll(o => o.Status == SD.StatusSubmitted || o.Status == SD.StatusInProcess)
                .OrderByDescending(u => u.PickUpTime).ToList();


            foreach (OrderHeader item in OrderHeaderList)
            {
                OrderDetailsVM individual = new OrderDetailsVM
                {
                    OrderHeader = item,
                    OrderDetails = _unitOfWork.OrderDetails.GetAll(o => o.OrderId == item.Id).ToList()
                };
                orderDetailsVM.Add(individual);
            }
        }

        //post for Order Prepare
        public IActionResult OnPostOrderPrepare(int orderId)
        {
            OrderHeader orderHeader = _unitOfWork.OrderHeader.GetFirstOrDefault(o => o.Id == orderId);
            orderHeader.Status = SD.StatusInProcess;
            _unitOfWork.Save();
            return RedirectToPage("ManageOrder");
        }
        //post for Order Ready
        public IActionResult OnPostOrderReady(int orderId)
        {
            OrderHeader orderHeader = _unitOfWork.OrderHeader.GetFirstOrDefault(o => o.Id == orderId);
            orderHeader.Status = SD.StatusReady;
            _unitOfWork.Save();
            return RedirectToPage("ManageOrder");
        }
        //post for Order cancel
        public IActionResult OnPostOrderCancel(int orderId)
        {
            OrderHeader orderHeader = _unitOfWork.OrderHeader.GetFirstOrDefault(o => o.Id == orderId);
            orderHeader.Status = SD.StatusCancelled;
            _unitOfWork.Save();
            return RedirectToPage("ManageOrder");
        }


        //post for Order Refund
        public IActionResult OnPostOrderRefund(int orderId)
        {
            OrderHeader orderHeader = _unitOfWork.OrderHeader.GetFirstOrDefault(o => o.Id == orderId);
            //refund amoount
            var options = new RefundCreateOptions
            {
                Amount = Convert.ToInt32(orderHeader.OrderTotal * 100),
                Reason = RefundReasons.RequestedByCustomer, //on customer requste we can put other options as well
                ChargeId = orderHeader.TransactionId //trcking transaction id for refund
            };
            var service = new RefundService();
            Refund refund = service.Create(options);
            orderHeader.Status = SD.StatusRefunded;

            _unitOfWork.Save();
            return RedirectToPage("ManageOrder");
        }
    }
}