using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

////////////////////////////////////////////////////////////////////////////////////
//Model for LINQ
//- Add IsValid to each model class: No need
//- New MyDataContext : Done
//- namespace for MyDataContext designer: XT.Model.LINQ -> XT.Model
//- replace 'public int Status' to 'public override int Status'
//- replace 'public System.DateTime Created_Date' to 'public override System.DateTime Created_Date'

namespace XT.Model
{
    #region IDataContext
    public partial class MyDataContext : IDataContext
    {
        protected object RefreshObject;

        public U Add<U>(U u) where U : class
        {
            this.GetTable<U>().InsertOnSubmit(u);
            RefreshObject = u;
            return u;
        }

        public void Delete<U>(U u) where U : class
        {
            this.GetTable<U>().DeleteOnSubmit(u);
        }

        public U Find<U, V>(V id) where U : class, IEntity<V>
        {
            // get the row from the database using the meta-model
            MetaType meta = this.Mapping.GetTable(typeof(U)).RowType;
            if (meta.IdentityMembers.Count != 1) throw new InvalidOperationException(
                "Composite identity not supported");
            string idName = meta.IdentityMembers[0].Member.Name;

            var param = Expression.Parameter(typeof(U), "row");
            var lambda = Expression.Lambda<Func<U, bool>>(
                Expression.Equal(
                    Expression.PropertyOrField(param, idName),
                    Expression.Constant(id, typeof(V))), param);

            return this.GetTable<U>().FirstOrDefault(lambda);
        }

        public IQueryable<U> FindAll<U>() where U : class
        {
            return this.GetTable<U>().AsQueryable<U>();
        }

        public void SaveChanges()
        {
            base.SubmitChanges();
            if (RefreshObject != null)
            {
                base.Refresh(RefreshMode.OverwriteCurrentValues, RefreshObject);
            }
        }
    }
    #endregion
}
