using Data.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repository
{
    public class Repository<Entity> : IRepository<Entity> where Entity : class
    {
        private DBbrella db;
        private DbSet<Entity> ds;

        public Repository(DBbrella _db)
        {
            db = _db;
            ds = db.Set<Entity>();
        }


        public virtual void Add(Entity entity)
        {
            ds.Add(entity);
        }


        public virtual List<Entity> Get(Expression<Func<Entity, bool>> where = null,
            Func<IQueryable<Entity>, IOrderedQueryable<Entity>> orderby = null, string Include = "")
        {
            IQueryable<Entity> query = ds;

            if (where != null)
            {
                query = query.Where(where);
            }

            if (orderby != null)
            {
                query = orderby(query);
            }

            if (Include != null)
            {
                foreach (var item in Include.Split(','))
                {
                    query = query.Include(item);
                }
            }

            return query.ToList();
        }


        public virtual void Update(Entity entity)
        {
            ds.Attach(entity);
            db.Entry(entity).State = EntityState.Modified;
        }


        public virtual void Remove(Entity entity)
        {
            if (db.Entry(entity).State == EntityState.Detached)
            {
                ds.Attach(entity);
            }

            ds.Remove(entity);
        }


        public virtual void Remove(object id)
        {
            Remove(Find(id));
        }


        public virtual Entity Find(object id)
        {
            return ds.Find(id);
        }


        public virtual void SaveChange()
        {
            db.SaveChanges();
        }
    }
}
