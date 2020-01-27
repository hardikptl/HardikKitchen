using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Test.Models
{   
    /// <summary>
    /// Shopping cart model with Properties
    /// </summary>
    public class ShoppingCart
    {
        //intilizing count property for quantity when user add 
        //item to shopping cart
        public ShoppingCart()
        {
            Count = 1;
        }
        public int Id { get; set; }

        
        public int MenuItemId { get; set; }

        //notmapped attirubutre to not add this property to database 
        //foreignkey set
        [NotMapped]
        [ForeignKey("MenuItemId")]
        public virtual MenuItem MenuItem { get; set; }

       
        public string ApplicationUserId { get; set; }

        //notmapped attirubutre to not add this property to database 
        //foreignkey set
        [NotMapped]
        [ForeignKey("ApplicationUserId")]
        public virtual ApplicationUser ApplicationUser { get; set; }

        //validating count range
        [Range(1,100 , ErrorMessage ="Please select count between 1 to 100")]
        public int Count { get; set; }
    }
}
