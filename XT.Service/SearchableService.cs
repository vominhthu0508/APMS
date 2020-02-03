using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XT.Model;
using XT.Repository;

namespace XT.BusinessService
{
    public abstract class SearchableService<U, V> : Service<U, V>, ISearchableService<U, V> where U : class, ISearchableEntity<V>
    {
        public SearchableService(IUow uow)
            : base(uow)
        {
        }

        public IEnumerable<ISearchableEntity> FindAllSearch()
        {
            return FindAll();
        }

        /// <summary>
        /// Find All Valid By Search
        /// </summary>
        /// <param name="term"></param>
        /// <returns></returns>
        public IEnumerable<ISearchableEntity> FindAllSearch(string term)
        {
            var new_term = term.Trim().ToLower().Convert_Chuoi_Khong_Dau();
            var blank = " ".ToCharArray();
            var term_chars = new_term.Split(blank);
            var term_length = term_chars.Length;

            var results = new List<U>();
            foreach (var c in FindAllValid())
            {
                var description = c.Description.ToLower().Convert_Chuoi_Khong_Dau();
                var overloading = description.Split(blank).Intersect(term_chars, StringComparer.OrdinalIgnoreCase).Count();
                if (overloading > 0 && overloading >= term_length)
                {
                    results.Add(c);
                }
                else
                {
                    var name = c.Name.ToLower().Convert_Chuoi_Khong_Dau();

                    if (name.WordContains(new_term))
                    {
                        results.Add(c);
                    }
                    else if (name.Remove_Khoang_Trang().Contains(new_term.Remove_Khoang_Trang()))
                    {
                        results.Add(c);
                    }
                }
            }

            return results;


            //var fetchTag = FindAllValidByCriteria(c => c.Name.ToLower().Convert_Chuoi_Khong_Dau().WordContains(new_term));
            //if (fetchTag.Count() == 0)
            //{
            //    //fetchTag = FindAllValidByCriteria(c => c.Name.ToLower().Convert_Chuoi_Khong_Dau().WordPartialContains(new_term));
            //    fetchTag = FindAllValidByCriteria(c => c.Name.ToLower().Convert_Chuoi_Khong_Dau().Remove_Khoang_Trang().Contains(new_term.Remove_Khoang_Trang()));
            //    if (fetchTag.Count() == 0)
            //    {
            //        var blank = " ".ToCharArray();
            //        var term_chars = new_term.Split(blank);
            //        fetchTag = FindAllValidByCriteria(c => c.Name.ToLower().Convert_Chuoi_Khong_Dau().Split(blank).Intersect(term_chars, StringComparer.OrdinalIgnoreCase).Count() > 0);
            //        if (fetchTag.Count() == 0)
            //        {
            //            fetchTag = FindAllValidByCriteria(c => c.Description.ToLower().Convert_Chuoi_Khong_Dau().WordContains(new_term));
            //            if (fetchTag.Count() == 0)
            //            {
            //                fetchTag = FindAllValidByCriteria(c => c.Description.ToLower().Convert_Chuoi_Khong_Dau().WordPartialContains(new_term));
            //            }
            //        }
            //    }
            //    //fetchTag = FindAllValidByCriteria(c => c.Description.ToLower().Convert_Chuoi_Khong_Dau().WordContains(new_term));
            //    //if (fetchTag.Count() == 0)
            //    //{
            //    //    fetchTag = FindAllValidByCriteria(c => c.Name.ToLower().Convert_Chuoi_Khong_Dau().WordPartialContains(new_term));
            //    //    if (fetchTag.Count() == 0)
            //    //    {
            //    //        fetchTag = FindAllValidByCriteria(c => c.Description.ToLower().Convert_Chuoi_Khong_Dau().WordPartialContains(new_term));
            //    //    }
            //    //}
            //}

            //return fetchTag;
        }
    }
}
