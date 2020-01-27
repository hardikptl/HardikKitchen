using System;
using System.Collections.Generic;
using System.Text;
using Test.Models;

namespace Taste.DataAccess.Data.Repository.IRepository
{
    public interface IOrderDetailsRepository : IRepository<OrderDetails>
    {
        void Update(OrderDetails orderDetails);
    }
}
