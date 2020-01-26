using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Test.Models
{

    /// <summary>
    /// this class hnadles application user properties
    /// </summary>
    public class ApplicationUser : IdentityUser
    {
        //displaying full name from full name propertity
        [Display(Name ="Full Name")]
        public string FirstName { get; set; }
        public string LastName { get; set; }


        //notmapped will not added to database when craete migration
        [NotMapped]
        public string FullName { get { return FirstName + " " + LastName; } }
    }
}
