using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Text;
using Test.Models;

namespace Taste.DataAccess.Data.Repository.IRepository
{
    public interface IFoodTypeRepository :IRepository<FoodType>
    {
        IEnumerable<SelectListItem> GetCategoryListForDropDown();

        void Update(FoodType foodType);
    }
}
