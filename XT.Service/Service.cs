using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using XT.Model;
using XT.Repository;

namespace XT.BusinessService
{
    public class Service<U, V> : IService<U, V> where U : class, IEntity<V>
    {
        private readonly IUow _uow;
        public Service(IUow uow)
        {
            _uow = uow;
        }
        
        protected IRepository<U, V> GetRepository()
        {
            return _uow.Repository<U, V>();
        }

        protected IRepository<T, Int32> GetRepository<T>() where T : class, IEntity<Int32>
        {
            return _uow.Repository<T>();
        }

        #region Properties
        protected int Lang_Id { get { return _uow.Lang_Id; } }
        protected int Center_Id { get { return _uow.Center_Id; } }
        public virtual string NameForFinding
        {
            get { return string.Empty; }
        }

        public virtual string NameForParent
        {
            get { return string.Empty; }
        }
        #endregion

        /////////////////////////////////////////////////////////////////////////////////
        /////////////////////////////////////////////////////////////////////////////////
        /////////////////////////////////////////////////////////////////////////////////
        //Add, Update, Delete

        public virtual U Add(U u)
        {
            u.Created_Date = DateTime.Now;
            return GetRepository().Add(u);
        }

        public virtual void AddAll(U[] us)
        {
            GetRepository().AddAll(us);
        }

        public virtual U Update(U u)
        {
            return GetRepository().Update(u);
        }

        public virtual void Delete(V id)
        {
            var found = FindById(id);
            if (found != null)
                GetRepository().Delete(found);
            else
                throw new NullReferenceException();
        }

        public virtual void DeleteById(V id)
        {
            Delete(id);
        }

        public virtual void Delete(U u)
        {
            Delete(u.Obj_Id);
        }

        public virtual void DeleteForever(U u)
        {
            GetRepository().DeleteForever(u);
        }

        public virtual void Active(V id)
        {
            var found = FindById(id);
            if (found != null)
                GetRepository().Active(found);
            else
                throw new NullReferenceException();
        }

        public void Active(U u)
        {
            GetRepository().Active(u);
        }

        public void DeleteAll(V[] ids)
        {
            if (ids != null && ids.Length > 0)
            {
                GetRepository().DeleteAll(ids);
            }
        }

        public void DeleteAll(U[] us)
        {
            if (us != null && us.Length > 0)
            {
                GetRepository().DeleteAll(us);
            }
        }

        /////////////////////////////////////////////////////////////////////////////////
        /////////////////////////////////////////////////////////////////////////////////
        /////////////////////////////////////////////////////////////////////////////////
        //Find and Check

        private bool CanTranslate()
        {
            return false;
            //return Lang_Id != Convert.ToInt16(LanguageEnum.vi) && ModelBase<Int32>.GetProperty_Translations(typeof(U)) != null;
        }

        public U FindById(V id)
        {
            var target = GetRepository().Find(id);
            //if (CanTranslate())
            //{
            //    var localizer = (U)target.Clone();
            //    localizer.Localize(Lang_Id);
            //    return localizer;
            //}
            return target;
        }


        public U FindByCriteria(Expression<Func<U, bool>> exp)//with localize
        {
            //return GetRepository().FindByCriteria(exp);
            var target = GetRepository().FindByCriteria(exp);
            //if (CanTranslate())
            //{
            //    var t_target = target.Clone();
            //    t_target.Localize(Lang_Id);
            //    return (U)t_target;
            //}

            return target;
        }

        public U FindValidByCriteria(Expression<Func<U, bool>> exp)
        {
            return FindAllValidByCriteria(exp).FirstOrDefault();
        }

        public IEnumerable<U> FilterByName(IEnumerable<U> source, string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return source;

            name = name.Trim().ToLower().Convert_Chuoi_Khong_Dau();
            var list = new List<U>();
            var HasName = false;

            foreach (var item in source)
            {
                var item_name_prop = item.GetType().GetProperty(NameForFinding);
                if (item_name_prop != null)
                {
                    HasName = true;
                    var item_name_value = item_name_prop.GetValue(item).ToString();
                    if (item_name_value.ToLower().Convert_Chuoi_Khong_Dau().Contains(name))
                    {
                        list.Add(item);
                    }
                }
                else
                {
                    break;
                }
            }

            return HasName ? list : source;
        }

        public IEnumerable<U> FilterByParent(IEnumerable<U> source, int parentId = 0)
        {
            if (parentId == 0)
                return source;

            var list = new List<U>();
            var HasName = false;

            foreach (var item in source)
            {
                var item_name_prop = item.GetType().GetProperty(NameForParent);
                if (item_name_prop != null)
                {
                    HasName = true;
                    var item_name_value = item_name_prop.GetValue(item).ToString();
                    if (item_name_value.Equals(parentId.ToString()))
                    {
                        list.Add(item);
                    }
                }
                else
                {
                    break;
                }
            }

            return HasName ? list : source;
        }

        public virtual IEnumerable<U> FindByName(string name, int parentId = 0)
        {
            return FilterByName(FilterByParent(FindAllValid(), parentId), name);
        }

        public virtual bool CheckExistIdentity(string identity)
        {
            return false;
        }

        public virtual bool CheckExistIdentity(U current)
        {
            return false;
        }

        /////////////////////////////////////////////////////////////////////////////////
        /////////////////////////////////////////////////////////////////////////////////
        /////////////////////////////////////////////////////////////////////////////////
        //FindAll

        public IEnumerable<U> FindAll()
        {
            //var result = GetRepository().FindAll();
            //if (CanTranslate())
            //{
            //    foreach (var item in result)
            //    {
            //        item.Localize(Lang_Id);
            //    }
            //}
            //return result;

            return Center_Id != 0 ?
                GetRepository().FindAll().Where(s => s.IsCenter(Center_Id)) :
                GetRepository().FindAll();
        }

        /// <summary>
        /// Based on FindAll()
        /// </summary>
        /// <param name="exp"></param>
        /// <returns></returns>
        public IEnumerable<U> FindAllByCriteria(Expression<Func<U, bool>> exp)
        {
            var result = FindAll().Where(exp.Compile());//GetRepository().FindAllByCriteria(exp);
            //if (CanTranslate())
            //{
            //    foreach (var item in result)
            //    {
            //        item.Localize(Lang_Id);
            //    }
            //}
            return result;
        }

        /////////////////////////////////////////////////////////////////////////////////
        /////////////////////////////////////////////////////////////////////////////////
        /////////////////////////////////////////////////////////////////////////////////
        //FindAllBy

        /// <summary>
        /// Based on FindAll()
        /// </summary>
        /// <returns></returns>
        public IEnumerable<U> FindAllValid()
        {
            //return GetRepository().FindAllValid();
            return FindAll().Valid();
        }

        public IEnumerable<U> FindAllValidByCriteria(Expression<Func<U, bool>> exp)
        {
            //return GetRepository().FindAllValidByCriteria(exp);
            //return FindAllByCriteria(exp).Valid();
            return FindAll().Where(exp.Compile()).Valid();
        }

        public virtual IEnumerable<U> FindAllForFilter()
        {
            return FindAllValid();
        }

        public IQueryable<U> FindAllQuery()
        {
            return GetRepository().FindAllQuery();
        }

        public void SubmitChanges()
        {
            GetRepository().SubmitChanges();
        }

    }
}
