using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Linq;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace XT.Model
{
    public abstract class ModelBase<T> : IEntity<T>
    {
        public ModelBase()
        {
            Created_Date = DateTime.Now;
        }

        public virtual T Obj_Id { get { throw new NotImplementedException(); } }

        object IEntity.Obj_Id { get { throw new NotImplementedException(); } }

        public virtual DateTime Created_Date { get; set; }

        public virtual bool IsValid()
        {
            return true;
        }

        public virtual bool IsCenter(int Center_Id)
        {
            return true;
        }

        public virtual bool IsAvailable()
        {
            return true;
        }

        public virtual bool HasChildren()
        {
            return false;
        }

        public virtual bool IsValidCopyProperties(PropertyInfo prop)
        {
            return prop.Name != "Id" && prop.Name != "Obj_Id" && prop.Name != "Created_Date";
        }

        public virtual void Copy(IEntity u)
        {
            var this_namespace = typeof(IEntity).Namespace;
            foreach (var prop in this.GetType().GetProperties())
            {
                if (prop.PropertyType.Namespace != this_namespace
                    && IsValidCopyProperties(prop)//prop.Name != "Id" && prop.Name != "Obj_Id"
                    && prop.CanWrite
                    && !prop.PropertyType.IsArray//array
                    && (!prop.PropertyType.IsGenericType ||
                        prop.PropertyType.IsGenericType
                        && prop.PropertyType.GetGenericTypeDefinition() != typeof(EntitySet<>)//LINQ
                        && prop.PropertyType.GetGenericTypeDefinition() != typeof(EntityRef<>)//LINQ
                        && prop.PropertyType.GetGenericTypeDefinition() != typeof(ICollection<>)//EF
                        && prop.PropertyType.GetGenericTypeDefinition() != typeof(IEnumerable<>)//EF
                        )//foreign entity
                    )
                {
                    prop.SetValue(this, u.GetType().GetProperty(prop.Name).GetValue(u));
                }
            }
        }

        /// <summary>
        /// Copy các data properties từ entity u vào this (ngoại trừ non-data properties như Id, Status, Created_Date)
        /// </summary>
        /// <param name="u"></param>
        public void CopyModel(IEntity u)
        {
            Copy(u);
        }

        public static System.Reflection.PropertyInfo GetProperty_Translations(Type t)
        {
            return t.GetProperty(t.Name + "_Translations");
        }

        public void Localize(int langId)
        {
            IEnumerable<Object> castObj;
            var thisT = this.GetType();
            castObj = (IEnumerable<Object>)GetProperty_Translations(thisT).GetValue(this);
            foreach (var sub in castObj)
            {
                var subT = sub.GetType();
                int subLangId = Convert.ToInt32(subT.GetProperty("Lang_Id").GetValue(sub));
                if (subLangId == langId)
                {
                    foreach (var subProp in subT.GetProperties())
                    {
                        foreach (var mainProp in thisT.GetProperties())
                        {
                            if ("T" + mainProp.Name == subProp.Name)
                            {
                                mainProp.SetValue(this, subProp.GetValue(sub));
                            }
                        }
                    }
                }
            }
        }
        public IEntity Clone()
        {
            return this.MemberwiseClone() as IEntity<T>;
        }

        public virtual IEntity ToModel()
        {
            return this;
        }

        public virtual IEntity ToModel(IEntity _model)
        {
            _model.CopyModel(this);
            return _model;
        }
    }

    public abstract class StoredModelBase<T> : ModelBase<T>, IStoredEntity<T>
    {
        public StoredModelBase()
            :base()
        {
        }

        public virtual int Status { get; set; }

        public virtual bool IsValid()
        {
            //return true;
            return this.Status == (int)EntityStatus.Visible;
            //return IsValidExpression.Compile().Invoke(this);
        }

        public override bool IsValidCopyProperties(PropertyInfo prop)
        {
            return base.IsValidCopyProperties(prop) && prop.Name != "Status";
        }
    }

    public abstract class RegisterModelBase<T> : StoredModelBase<T>, IRegisterEntity<T>
    {
        public RegisterModelBase()
            : base()
        {
        }

        public virtual string Name { get; set; }
        public virtual string Email { get; set; }
        public virtual string Avatar { get; set; }
    }

    public abstract class SearchableModelBase<T> : StoredModelBase<T>, ISearchableEntity<T>
    {
        public SearchableModelBase()
            : base()
        {
        }

        //public virtual int SId { get; set; }
        object IEntity.Obj_Id { get { return this.Obj_Id; } }
        public virtual string Name { get; set; }
        public virtual string Title { get; set; }
        public virtual string Description { get; set; }
        public virtual string SubDescription { get; set; }
        public virtual string Image { get; set; }
        public virtual double Rating { get; set; }
    }
}
