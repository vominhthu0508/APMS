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
    //U là 1 class và là con của IEntity<V>
    //V là kiểu dữ liệu khóa
    public class Repository<U, V> : IRepository<U, V> where U : class, IEntity<V>
    {
        private bool disposed = false;
        protected IDataContext Context { get; set; }

        public Repository(IDataContext context)
        {
            Context = context;
        }

        public virtual U Add(U u)
        {
            this.Context.Add(u);
            this.Context.SaveChanges();
            return u;
        }

        public virtual void AddAll(U[] us)
        {
            foreach (var u in us)
            {
                this.Context.Add(u);
            }

            this.Context.SaveChanges();
        }

        public virtual U Update(U u)
        {
            return Update(u.Obj_Id, (updating) =>
            {
                updating.Copy(u);
                return true;
            });
        }

        public virtual U Update(V id, Func<U, bool> pred)
        {
            if (pred == null)
            {
                throw new ArgumentNullException();
            }

            U saving = Find(id);

            if (pred(saving))
            {
                try
                {
                    this.Context.SaveChanges();
                }
                catch (Exception ex)
                {
                    //return null;
                    throw ex;
                }
            }
            else
            {
                throw new System.Data.DataException();
            }

            return saving;
        }

        public virtual void Delete(U u)
        {
            this.Context.Delete(u);
            this.Context.SaveChanges();
        }

        public virtual void DeleteForever(U u)
        {
            Delete(u);
        }

        public virtual void Delete(V id)
        {
            var u = Find(id);
            if (u != null)
            {
                Delete(u);
            }
        }

        public virtual void DeleteAll(V[] ids)
        {
            IEnumerable<U> deleting = FindAllByCriteria(c => ids.Contains(c.Obj_Id));

            foreach (var u in deleting)
            {
                this.Context.Delete(u);
            }

            this.Context.SaveChanges();
        }

        public virtual void DeleteAll(U[] us)
        {
            foreach (var u in us)
            {
                this.Context.Delete(u);
            }

            this.Context.SaveChanges();
        }

        public virtual void Active(U u)
        {

        }

        /////////////////////////////////////////////////////////////////////////////////
        /////////////////////////////////////////////////////////////////////////////////
        /////////////////////////////////////////////////////////////////////////////////
        //FindById

        public virtual U Find(V id)
        {
            return this.Context.Find<U, V>(id);
        }

        public virtual U FindByCriteria(Expression<Func<U, bool>> exp)
        {
            return this.Context.FindAll<U>().Where(exp).FirstOrDefault();
        }

        /////////////////////////////////////////////////////////////////////////////////
        /////////////////////////////////////////////////////////////////////////////////
        /////////////////////////////////////////////////////////////////////////////////
        //FindAll

        public IEnumerable<U> FindAll()
        {
            return this.Context.FindAll<U>();
        }

        public IEnumerable<U> FindAllByCriteria(Expression<Func<U, bool>> exp)
        {
            return FindAll().Where(exp.Compile());//.AsEnumerable();
        }

        public IQueryable<U> FindAllQuery()
        {
            return this.Context.FindAll<U>();
        }

        /////////////////////////////////////////////////////////////////////////////////
        /////////////////////////////////////////////////////////////////////////////////
        /////////////////////////////////////////////////////////////////////////////////
        //SubmitChanges

        public void SubmitChanges()
        {
            this.Context.SaveChanges();
        }

        /////////////////////////////////////////////////////////////////////////////////
        /////////////////////////////////////////////////////////////////////////////////
        /////////////////////////////////////////////////////////////////////////////////
        //Dispose
        //public void Dispose()
        //{
        //    Dispose(true);
        //    GC.SuppressFinalize(this);
        //}

        //// Protected implementation of Dispose pattern. 
        //protected virtual void Dispose(bool disposing)
        //{
        //    if (disposed)
        //        return;

        //    if (disposing)
        //    {
        //        // Free any other managed objects here. 
        //        Context.Dispose();
        //    }

        //    // Free any unmanaged objects here. 
        //    //
        //    disposed = true;
        //}
    }
}
