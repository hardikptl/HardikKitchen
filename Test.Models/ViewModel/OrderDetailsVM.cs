using System;
using System.Collections.Generic;
using System.Text;

namespace Test.Models.ViewModel
{

    /// <summary>
    /// this viewmodel for order details
    /// </summary>
    public class OrderDetailsVM
    {


        public OrderHeader OrderHeader { get; set; }
        public List<OrderDetails> OrderDetails { get; set; }
    }

}
