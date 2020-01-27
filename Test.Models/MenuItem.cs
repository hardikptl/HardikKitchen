using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Test.Models
{   
    
    /// <summary>
    /// MenuItem Model With All Properties 
    /// </summary>
    public class MenuItem
    {
        public int Id { get; set; }
        [Required]
        [Display(Name ="Food Name")]
        public string Name { get; set; }

        public string Description { get; set; }

        public String Image { get; set; }


        //using range to validate price(min value, max value , error message) at server side
        [Range(1,int.MaxValue,ErrorMessage ="Price sholud be greater than 1")]
        public double Price { get; set; }

        [Required]
        [Display(Name="Category Type")]
        public int CategoryId { get; set; }

        //this is get id from above CategoryId varible and set forreignkey relation 
        [ForeignKey("CategoryId")]
        public virtual Category Category { get; set; }
        [Required]
        [Display(Name = "Food Type")]
        public int FoodTypeId{ get; set; }

        //this is get id from above FoodTypeID varible and set forreignkey relation 
        [ForeignKey("FoodTypeId")]
        public virtual FoodType FoodType { get; set; }


    }
}
