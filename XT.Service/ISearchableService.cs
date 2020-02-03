using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XT.Model;

namespace XT.BusinessService
{
    public interface ISearchableService
    {
        /// <summary>
        /// Find All Valid By Search
        /// </summary>
        /// <param name="term"></param>
        /// <returns></returns>
        IEnumerable<ISearchableEntity> FindAllSearch();

        /// <summary>
        /// Find All Valid By Search
        /// </summary>
        /// <param name="term"></param>
        /// <returns></returns>
        IEnumerable<ISearchableEntity> FindAllSearch(string term);
    }

    public interface ISearchableService<U, V> : ISearchableService, IService<U, V> where U : class, ISearchableEntity<V>
    {
    }
}
