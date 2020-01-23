using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Taste.DataAccess.Data.Repository.IRepository;
using Test.Models;

namespace HardikKitchen
{
    public class UpsertModel : PageModel
    {

        private readonly IUnitOfWork _unitOfWork;

        public UpsertModel(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public Category CategoryObj { get; set; }

        public IActionResult OnGet(int? id)
        {
            CategoryObj = new Test.Models.Category();
            if (id != null)
            {
                CategoryObj = _unitOfWork.Category.GetFirstOrDefault(u => u.Id == id);
                if (CategoryObj == null)
                {
                    return NotFound();
                }
            }
            return Page();
        }
    }
}

