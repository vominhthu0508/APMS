using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using XT.Model;

namespace XT.BusinessService
{
    public interface IService
    {

    }

    public interface IService<U, V> : IService where U : class, IEntity<V>
    {
        U Add(U u);
        void AddAll(U[] us);
        U Update(U u);
        //U UpdateByFields(V id , Func<U,Boolean> fields);
        //U Update(U u);
        void Delete(V id);
        void DeleteById(V id);//for anti-ambigous with Delete
        void Delete(U u);
        void DeleteForever(U u);
        void Active(V id);
        void Active(U u);

        void DeleteAll(V[] ids);
        void DeleteAll(U[] us);

        U FindById(V id);
        U FindByCriteria(Expression<Func<U, bool>> exp);
        U FindValidByCriteria(Expression<Func<U, bool>> exp);

        bool CheckExistIdentity(string identity);//hàm override cho class con kế thừa
        bool CheckExistIdentity(U current);//hàm override cho class con kế thừa

        IEnumerable<U> FindAll();
        IEnumerable<U> FindAllValid();
        IEnumerable<U> FindAllForFilter();
        //IEnumerable<U> FilterByName(IEnumerable<U> source, string name);
        IEnumerable<U> FindByName(string name, int parentId = 0);

        IEnumerable<U> FindAllByCriteria(Expression<Func<U, bool>> exp);
        IEnumerable<U> FindAllValidByCriteria(Expression<Func<U, bool>> exp);

        IQueryable<U> FindAllQuery();

        void SubmitChanges();
    }
}
