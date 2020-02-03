using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace XT.Model
{
    public enum EntityStatus
    {
        Visible = 1,
        Invisible = 0,
        UnDefined = 0,
        UnPublic = -1,
    }

    //mọi đối tượng sẽ được đưa về cùng kiểu chung dễ quản lý
    public interface IEntity
    {
        object Obj_Id { get; }
        DateTime Created_Date { get; set; }
        bool IsValid();//tất cả đều cần xác định IsValid
        bool IsCenter(int Center_Id);
        bool HasChildren();
        void Copy(IEntity u);
        void CopyModel(IEntity u);
        IEntity ToModel();
        IEntity ToModel(IEntity u);
        IEntity Clone();
        void Localize(int langId);
    }

    public interface IStoredEntity : IEntity
    {
        int Status { get; set; }
    }

    public interface IEntity<T> : IEntity
    {
        new T Obj_Id { get; }
    }

    public interface IStoredEntity<T> : IStoredEntity, IEntity<T>
    {
        //new T Id { get; }
        //Expression<Func<T, bool>> IsValidExpression { get; }
    }

    public interface IRegisterEntity : IStoredEntity
    {
        string Name { get; set; }
        string Email { get; set; }
        string Avatar { get; set; }
    }

    public interface IRegisterEntity<T> : IRegisterEntity, IEntity<T>
    {
    }

    public interface ISearchableEntity : IStoredEntity
    {
        //int SId { get; set; }
        string Name { get; set; }
        string Title { get; set; }
        string Description { get; set; }
        string SubDescription { get; set; }
        string Image { get; set; }
        double Rating { get; set; }
    }

    public interface ISearchableEntity<T> : ISearchableEntity, IEntity<T>
    {
    }

    ////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////
    //IDataContext

    public interface IDataContext
    {
        U Add<U>(U u) where U : class;
        void Delete<U>(U u) where U : class;
        U Find<U, V>(V id) where U : class, IEntity<V>;
        IQueryable<U> FindAll<U>() where U : class;

        void SaveChanges();
    }

    ////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////
    //PagedData

    public class PagedData<T> where T : class
    {
        public IEnumerable<T> Data { get; set; }
        public int TotalItemCount { get; set; }
        public int NumberOfPages { get; set; }
        public int CurrentPage { get; set; }
    }
}
