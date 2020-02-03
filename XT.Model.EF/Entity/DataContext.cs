using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XT.Model
{
    #region IDataContext
    public partial class MyDataContext : IDataContext
    {
        public U Add<U>(U u) where U : class
        {
            return this.Set<U>().Add(u);
        }

        public void Delete<U>(U u) where U : class
        {
            this.Set<U>().Remove(u);
        }

        public U Find<U, V>(V id) where U : class, IEntity<V>
        {
            return this.Set<U>().Find(id);
        }

        public IQueryable<U> FindAll<U>() where U : class
        {
            return this.Set<U>().AsQueryable<U>();
        }

        public new void SaveChanges()
        {
            base.SaveChanges();
        }
    }
    #endregion
}
