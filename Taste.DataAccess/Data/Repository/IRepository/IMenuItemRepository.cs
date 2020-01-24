using System;
using System.Collections.Generic;
using System.Text;
using Test.Models;

namespace Taste.DataAccess.Data.Repository.IRepository
{
    public interface IMenuItemRepository : IRepository<MenuItem>
    {
        void Update(MenuItem MenuItem);

    }
}
