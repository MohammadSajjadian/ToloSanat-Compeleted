using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repository
{
    public interface IRepository<Entity>
    {
        void Add(Entity entity);

        List<Entity> Get(Expression<Func<Entity, bool>> where = null, Func<IQueryable<Entity>, IOrderedQueryable<Entity>> orderby = null, string Include = "");

        void Update(Entity entity);

        void Remove(Entity entity);

        void Remove(object id);

        Entity Find(object id);

        void SaveChange();
    }
}
