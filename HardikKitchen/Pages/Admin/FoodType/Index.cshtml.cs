using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Taste.DataAccess.Data.Repository.IRepository;
using Test.Models;
using Test.Utility;

namespace HardikKitchen.Pages.Admin.FoodType
{
    //only manager can access admin side pages 
    [Authorize(Roles = SD.ManagerRole)]

    public class IndexModel : PageModel
    {
        public void OnGet()
        {

        }
    }
}