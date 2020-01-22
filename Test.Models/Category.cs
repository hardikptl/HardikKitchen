using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Test.Models
{
    public class Category
    {
        //if property name ID byfault  it consider as primary key but you can specify 
        //primary key by [key] attribute
        [Key]
        public int Id { get; set; }

        //using annotation [require] class for validation [display] for error message for property.
        [Required]
        [Display(Name ="Category Name")]
        public string Name { get; set; }

        [Required]
        [Display(Name ="Display Order")]
        public int DisplayOrder { get; set; }
    }
}
