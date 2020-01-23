using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using Taste.DataAccess.Data.Repository.IRepository;

namespace Taste.DataAccess.Data.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        protected readonly DbContext Context;
        internal DbSet<T> dbset;

        public Repository(DbContext context)
        {
            Context = context;
            this.dbset = context.Set<T>();
        }

        public void Add(T entity)
        {
            dbset.Add(entity);
            
        }

        public T Get(int Id)
        {
            return dbset.Find(Id);
        }

        public IEnumerable<T> GetAll(Expression<Func<T, bool>> filter = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, string includeProperties = null)
        {
            IQueryable<T> query = dbset;
            //checking filter parameter
            if (filter != null)
            {
                query = query.Where(filter);

            }
            //checking IncludeProperties Parameter
            //include property comma separated 
            if(includeProperties != null)
            {
                foreach(var includeProperty in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProperty);
                }
            }
            //checking orderBy Parameter
            if(orderBy != null)
            {
                return orderBy(query).ToList();
            }

            //return all records
            return query.ToList();
        }

        public T GetFirstOrDefault(Expression<Func<T, bool>> filter = null, string includeProperties = null)
        {
            IQueryable<T> query = dbset;
            //checking filter parameter
            if (filter != null)
            {
                query = query.Where(filter);

            }
            //checking IncludeProperties Parameter
            //include property comma separated 
            if (includeProperties != null)
            {
                foreach (var includeProperty in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProperty);
                }
            }
            //returning first or default record.
            return query.FirstOrDefault();
        }

        public void Remove(int id)
        {
            T entityToRemove = dbset.Find(id);
            Remove(entityToRemove);
        }

        public void Remove(T entity)
        {
            dbset.Remove(entity);
        }
    }
}
