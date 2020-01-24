using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Test.Models
{

    /// <summary>
    /// FoodType Model with Properties
    /// </summary>
    public class FoodType
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Display(Name=" Food Type Name")]
        public string Name { get; set; }
    }
}
