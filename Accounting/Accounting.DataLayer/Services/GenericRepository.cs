using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Linq.Expressions;

namespace Accounting.DataLayer.Services
{
    public class GenericRepository<TEnity> where TEnity : class
    {
        private Accounting_DBEntities _db;
        private DbSet<TEnity> _dbSet;

        public GenericRepository(Accounting_DBEntities db)
        {
            _db = db;
            _dbSet = _db.Set<TEnity>();
        }

        public virtual IEnumerable<TEnity> Get(Expression<Func<TEnity, bool>> where = null)
        {
            IQueryable<TEnity> query = _dbSet;

            if (where != null)
            {
                query = query.Where(where);
            }
            return query.ToList();
        }

        public virtual TEnity GetById(object Id)
        {
            return _dbSet.Find(Id);
        }

        public virtual void Insert(TEnity entity)
        {
            _dbSet.Attach(entity);
            _dbSet.Add(entity);
        }



        public virtual void Update(TEnity entity)
        {
            _dbSet.Attach(entity);
            _db.Entry(entity).State = EntityState.Modified;
        }

        public virtual void Delete(TEnity entity)
        {
            if (_db.Entry(entity).State == EntityState.Detached)
            {
                _dbSet.Attach(entity);
            }
            _dbSet.Remove(entity);
        }

        public virtual void Delete(object Id)
        {
            Delete(GetById(Id));
        }


    }
    public class test
    {
        // GenericRepository<Customers> customer = new GenericRepository<Customers>(db);
    }
}
