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
    public interface IStoredRepository<U, V> : IRepository<U, V> where U : class, IStoredEntity<V>
    {
    }
}
