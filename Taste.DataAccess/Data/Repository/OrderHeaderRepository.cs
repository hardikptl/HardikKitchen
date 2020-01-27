using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Taste.DataAccess.Data.Repository.IRepository;
using Test.Models;

namespace Taste.DataAccess.Data.Repository
{
    public class OrderHeaderRepository : Repository<OrderHeader> , IOrderHeaderRepository
    {
        private readonly ApplicationDbContext _db;

        /// <summary>
        /// intializing db
        /// </summary>
        /// <param name="db"></param>
        public OrderHeaderRepository(ApplicationDbContext db) :base(db)
        {
            _db = db;
        }

        /// <summary>
        /// Updating OrderHeader
        /// </summary>
        /// <param name="orderHeader"></param>
        public void Update(OrderHeader orderHeader)
        {
            var orderHeaderFromDb = _db.OrderHeader.FirstOrDefault(m => m.Id == orderHeader.Id);
            _db.OrderHeader.Update(orderHeaderFromDb);

            _db.SaveChanges();
        }
    }
}
