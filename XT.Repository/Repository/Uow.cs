using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using XT.Model;

namespace XT.Repository
{
    public class Uow : IUow
    {
        private IDataContext Context { get; set; }
        private Dictionary<string, object> repositories;
        private bool disposed = false;
        public int Lang_Id
        {
            get;
            set;
        }
        public int Center_Id
        {
            get;
            set;
        }
        public Uow(IDataContext context, int lang_id, int center_id)
        {
            // TODO: Complete member initialization
            Context = context;
            Lang_Id = lang_id;
            Center_Id = center_id;

            if (repositories == null)
            {
                repositories = new Dictionary<string, object>();
            }

            //Entity => Repository<Entity, V>
            //RAM nhiều thì add, không thì xuống dưới!!!
            //foreach (var t in typeof(IEntity).Assembly.GetExportedTypes())
            //{
            //    if (t.IsClass && !t.IsAbstract && t.GetInterfaces().Contains(typeof(IEntity)))
            //    {
            //        //t là class Model
            //        repositories.Add(t.Name, GetInstance(t));
            //    }
            //}
        }

        public void Commit()
        {
            Context.SaveChanges();
        }

        private object GetInstance(Type t)
        {
            var modelName = t.Name;
            var repositoryName = Assembly.GetExecutingAssembly().GetName().Name + "." + modelName + "Repository";
            var repository = Type.GetType(repositoryName);
            object repositoryInstance = null;
            if (repository != null)
            {
                repositoryInstance = Activator.CreateInstance(repository, Context);
            }
            else
            {
                if (t.GetInterfaces().Contains(typeof(IStoredEntity)))
                {
                    repositoryInstance = Activator.CreateInstance(
                                typeof(StoredRepository<,>).MakeGenericType(t, typeof(int)), Context);
                }
                else
                {
                    repositoryInstance = Activator.CreateInstance(
                            typeof(Repository<,>).MakeGenericType(t, typeof(int)), Context);
                }
            }

            return repositoryInstance;
        }

        public IRepository<T, Int32> Repository<T>() where T : class, IEntity<Int32>
        {
            return Repository<T, Int32>();
        }

        public IRepository<U, V> Repository<U, V>() where U : class, IEntity<V>
        {
            var t = typeof(U);
            if (!repositories.ContainsKey(t.Name))
            {
                repositories.Add(t.Name, GetInstance(t));
            }
            return (IRepository<U, V>)repositories[t.Name];

            //return (IRepository<U, V>)repositories[typeof(U).Name];
        }

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
        //        if (repositories != null && repositories.Count > 0)
        //        {
        //            foreach (var repo in repositories)
        //            {
        //                if (repo.Value != null)
        //                    ((IRepository)repo.Value).Dispose();
        //            }
        //        }
        //    }

        //    // Free any unmanaged objects here. 
        //    //
        //    disposed = true;
        //}
    }
}
