using System;
using System.Collections.Generic;
using System.Text;

namespace Test.Models.ViewModel
{

    /// <summary>
    /// View Model For Cart details with all properties 
    /// of order class.
    /// </summary>
   public class OrderDetailsCartVM
    {
        public List<ShoppingCart> listCart { get; set; }
        public OrderHeader OrderHeader { get; set; }
    }
}
