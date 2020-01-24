using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Taste.DataAccess.Data.Repository.IRepository;
using Test.Models;

namespace Taste.DataAccess.Data.Repository
{
    public class MenuItemRepository : Repository<MenuItem>, IMenuItemRepository
    {
        private readonly ApplicationDbContext _db;
        public MenuItemRepository(ApplicationDbContext db) :base(db)
        {
            _db = db;
        }
        public void Update(MenuItem menuItem)
        {
            var MenuItemFromDb = _db.MenuItem.FirstOrDefault(m => m.Id == menuItem.Id);
                MenuItemFromDb.Name = menuItem.Name;
                MenuItemFromDb.Description = menuItem.Description;
                MenuItemFromDb.CategoryId = menuItem.CategoryId;
                MenuItemFromDb.FoodTypeId = menuItem.FoodTypeId;
                MenuItemFromDb.Price = menuItem.Price;
                if(menuItem.Image!= null)
                {
                    MenuItemFromDb.Image = menuItem.Image;
                }
                _db.SaveChanges();
                
            
        }
    }
}
