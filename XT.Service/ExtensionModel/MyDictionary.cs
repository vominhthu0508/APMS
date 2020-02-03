using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using System.Web.Script.Serialization;

namespace XT.Model
{
    public class MyDictionary
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
        public int Count { get; set; }

        /// <summary>
        /// To Json String
        /// </summary>
        /// <returns></returns>
        //public override string ToString()
        //{
        //    return new JavaScriptSerializer().Serialize(this);//To Json String
        //}

        //public static string ToStringList(IEnumerable<MyDictionary> list)
        //{
        //    return new JavaScriptSerializer().Serialize(list);//To Json String
        //}

        //public static MyDictionary Parse(string json)
        //{
        //    //return new JavaScriptSerializer().DeserializeObject(json) as MyDictionary;
        //    return new JavaScriptSerializer().Deserialize<MyDictionary>(json);//To Json String
        //}

        //public static IEnumerable<MyDictionary> ParseList(string json)
        //{
        //    //return new JavaScriptSerializer().DeserializeObject(json) as IEnumerable<MyDictionary>;
        //    return new JavaScriptSerializer().Deserialize<IEnumerable<MyDictionary>>(json);//To Json String
        //}

        public static List<MyDictionary> GetValues(Type type, Func<object, bool> filter = null)
        {
            var arr = Enum.GetValues(type);
            var dict = new List<MyDictionary>();
            foreach (var a in arr)
            {
                if (filter != null)
                {
                    if (filter.Invoke(a))
                    {
                        dict.Add(new MyDictionary { Id = (int)a, Name = a.ToString() });
                    }
                }
                else
                {
                    dict.Add(new MyDictionary { Id = (int)a, Name = a.ToString() });
                }
            }

            return dict;
        }

        public static List<MyDictionary> GetValuesWithAll(Type type, string type_name = "")
        {
            var name = type.Name.Replace("Enum", "");
            if (type_name != "")
                name = type_name;
            var dict = new List<MyDictionary>();
            dict.Add(new MyDictionary { Id = 0, Name = "--" + name + "--" });
            dict.AddRange(GetValues(type));

            return dict;
        }
    }
}
