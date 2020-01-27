using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Taste.DataAccess.Data.Repository.IRepository;
using Test.Models;

namespace Taste.DataAccess.Data.Repository
{
    public class OrderDetailsRepository : Repository<OrderDetails> , IOrderDetailsRepository
    {
        private readonly ApplicationDbContext _db;

        /// <summary>
        /// intializing db
        /// </summary>
        /// <param name="db"></param>
        public OrderDetailsRepository(ApplicationDbContext db) :base(db)
        {
            _db = db;
        }

        /// <summary>
        /// Updating orderDetails
        /// </summary>
        /// <param name="orderDetails"></param>
        public void Update(OrderDetails orderDetails)
        {
            var orderDetailsFromDb = _db.OrderDetails.FirstOrDefault(m => m.Id == orderDetails.Id);
            _db.OrderDetails.Update(orderDetailsFromDb);

            _db.SaveChanges();
        }
    }
}
