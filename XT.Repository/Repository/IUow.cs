using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XT.Model;

namespace XT.Repository
{
    public interface IUow
    {
        IRepository<T, Int32> Repository<T>() where T : class, IEntity<Int32>;
        IRepository<U, V> Repository<U, V>() where U : class, IEntity<V>;
        int Lang_Id { get; set; }
        int Center_Id { get; set; }

        void Commit();
    }
}