using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using XT.Model;

namespace XT.Repository
{
    /// <summary>
    /// Interface for Repository Base, use for almost DAL actions
    /// If you need to implement other actions, extend a new interface to implement this
    /// Example: IDirectoryRepository : IRepository
    /// </summary>
    public interface IRepository
    {

    }

    public interface IRepository<U, V> : IRepository where U : class, IEntity<V>
    {
        U Add(U u);
        void AddAll(U[] us);
        U Update(U u);
        U Update(V id, Func<U, bool> pred);
        void Delete(V id);
        void Delete(U u);
        void DeleteAll(V[] ids);
        void DeleteAll(U[] us);
        void DeleteForever(U u);
        /// <summary>
        /// Set Status = Visible
        /// </summary>
        /// <param name="u"></param>
        void Active(U u);

        U Find(V id);
        U FindByCriteria(Expression<Func<U, bool>> exp);

        IEnumerable<U> FindAll();
        IEnumerable<U> FindAllByCriteria(Expression<Func<U, bool>> exp);

        IQueryable<U> FindAllQuery();

        void SubmitChanges();
    }
}
