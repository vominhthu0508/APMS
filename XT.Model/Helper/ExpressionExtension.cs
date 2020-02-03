using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace XT.Model
{
    public static class ExpressionExtension
    {
        public static Expression<Func<T, bool>> True<T>() { return f => true; }
        public static Expression<Func<T, bool>> False<T>() { return f => false; }

        public static Expression<Func<T, bool>> Or<T>(this Expression<Func<T, bool>> expr1, Expression<Func<T, bool>> expr2)
        {
            var invokedExpr = Expression.Invoke(expr2, expr1.Parameters.Cast<Expression>());
            return Expression.Lambda<Func<T, bool>>(Expression.Or(expr1.Body, invokedExpr), expr1.Parameters);
        }

        public static Expression<Func<T, bool>> And<T>(this Expression<Func<T, bool>> expr1, Expression<Func<T, bool>> expr2)
        {
            var invokedExpr = Expression.Invoke(expr2, expr1.Parameters.Cast<Expression>());
            return Expression.Lambda<Func<T, bool>>(Expression.And(expr1.Body, invokedExpr), expr1.Parameters);
        }

        /// <summary>
        /// pageIndex = 1 [start from page #1]
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public static IEnumerable<T> Paging<T>(this IEnumerable<T> list, int pageIndex, int pageSize)
        {
            return list.Skip((pageIndex - 1) * pageSize).Take(pageSize);
        }

        public static IEnumerable<T> Valid<T>(this IEnumerable<T> list) where T : class, IEntity
        {
            return list.Where(u => u.IsValid());
        }

        public static EntitySet<T> ToEntitySet<T>(this IEnumerable<T> source) where T : class
        {
            var es = new EntitySet<T>();
            es.AddRange(source);
            return es;
        }

        public static IEnumerable<T> GetValues<T>()
        {
            return Enum.GetValues(typeof(T)).Cast<T>();
        }

        public static bool IsNumber(this string s)
        {
            int n;
            return int.TryParse(s, out n);
        }

        public static IEnumerable<T> AddRange<T>(this ICollection<T> list, ICollection<T> others)
        {
            foreach (var item in others)
            {
                list.Add(item);
            }

            return list;
        }

        public static IEnumerable<T> WhereIf<T>(this IEnumerable<T> source, bool condition, Func<T, bool> predicate)
        {
            if (condition)
            {
                return source.Where(predicate);
            }
            return source;
        }

        public static PagedData<T> ToPagedData<T>(this IEnumerable<T> source, int pageIndex, int pageSize) where T : class
        {
            var pagedData = new PagedData<T>();
            pagedData.TotalItemCount = source.Count();
            pagedData.NumberOfPages = Convert.ToInt32(Math.Ceiling((double)pagedData.TotalItemCount / pageSize));
            pagedData.CurrentPage = pageIndex;
            pagedData.Data = source.Skip(pageSize * (pageIndex - 1)).Take(pageSize);

            return pagedData;
        }

        public static List<T> ToList<T>(this IEnumerable<IEntity> list) where T : class
        {
            if (list != null)
            {
                List<T> convertedList = new List<T>();
                foreach (var item in list)
                {
                    convertedList.Add(item as T);
                }

                return convertedList;
            }

            return null;
        }

        public static T MaxBy<T, U>(this IEnumerable<T> source, Func<T, U> selector) where U : IComparable<U>
        {
            if (source == null) throw new ArgumentNullException("source");
            bool first = true;
            T maxObj = default(T);
            U maxKey = default(U);
            foreach (var item in source)
            {
                if (first)
                {
                    maxObj = item;
                    maxKey = selector(maxObj);
                    first = false;
                }
                else
                {
                    U currentKey = selector(item);
                    if (currentKey.CompareTo(maxKey) > 0)
                    {
                        maxKey = currentKey;
                        maxObj = item;
                    }
                }
            }
            //if (first) return null;
            return maxObj;
        }

        public static IEnumerable<TSource> DistinctBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        {
            HashSet<TKey> seenKeys = new HashSet<TKey>();
            foreach (TSource element in source)
            {
                if (seenKeys.Add(keySelector(element)))
                {
                    yield return element;
                }
            }

            //return source.GroupBy(keySelector, comparer).Select(g => g.First());
        }

        //public static bool In<T>(this T source, params T[] list)
        //{
        //    return (list as IList<T>).Contains(source);
        //}
    }
}
