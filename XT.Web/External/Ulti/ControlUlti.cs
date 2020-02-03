using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Linq.Expressions;
using XT.Model;

namespace XT.Web.External
{
    public class ControlUlti
    {
        #region SelectList
        public static SelectList GetSelectListForEnums(Type type)
        {
            return new SelectList(MyDictionary.GetValues(type), "Id", "Name");
        }

        public static SelectList GetSelectListForEntities(IEnumerable<IEntity> items, string name)
        {
            return new SelectList(items, "Id", name);
        }

        public static string DropDownListFor(IEnumerable<MyDictionary> items, string name, int value)
        {
            var res = "<select class='form-control select2' name='" + name + "'>";
            foreach (var item in items)
            {
                res += "<option " + (value == item.Id ? "selected=selected" : "") + " value=" + item.Id + ">" + item.Name + "</option>";
            }
            res += "</select>";

            return res;
        }

        //public static string DropDownListFor<T>(Type type, Expression<Func<T>> expr)
        //{
        //    var body = ((MemberExpression)expr.Body);
        //    var name = body.Member.Name;
        //    var value = ((System.Reflection.FieldInfo)body.Member).GetValue(((ConstantExpression)body.Expression).Value);

        //    return DropDownListFor(MyDictionary.GetValues(type), name, (int)value);
        //}

        public static string DropDownListFor<T>(Type type, T item, Func<object, bool> filter = null) where T : class
        {
            return DropDownListFor(MyDictionary.GetValues(type, filter), item);
        }

        public static string DropDownListFor<T>(IEnumerable<MyDictionary> items, T item) where T : class
        {
            var name = typeof(T).GetProperties()[0].Name;
            var value = item.GetType().GetProperty(name).GetValue(item);

            return DropDownListFor(items, name, (int)value);
        }

        public static string DropDownListForAll<T>(Type type, T item, Func<object, bool> filter = null, string name = "All") where T : class
        {
            var all = new List<MyDictionary>();
            all.Add(new MyDictionary { Id = 0, Name = "--" + name + "--" });
            all.AddRange(MyDictionary.GetValues(type, filter));
            return DropDownListFor(all, item);
        }
        #endregion
    }
}