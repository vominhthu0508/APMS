using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Linq;
using System.Text;
using System.Threading.Tasks;
using XT.Model;
using System.Linq.Expressions;

namespace XT.Repository
{
    public class StoredRepository<U, V> : Repository<U, V>, IStoredRepository<U, V> where U : class, IStoredEntity<V>
    {
        public StoredRepository(IDataContext context)
            :base(context)
        {
        }

        public override U Add(U u)
        {
            if (u.Status == (int)EntityStatus.UnDefined)
            {
                u.Status = (int)EntityStatus.Visible;
            }
            return base.Add(u);
        }

        public override void Delete(U u)
        {
            u.Status = (int)EntityStatus.Invisible;
            base.Update(u);
        }

        public override void DeleteForever(U u)
        {
            base.Delete(u);
        }

        public override void Active(U u)
        {
            u.Status = (int)EntityStatus.Visible;
            base.Update(u);
        }
    }
}
