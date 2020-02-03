using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XT.Model;
using XT.Repository;

namespace XT.BusinessService
{
    public class ClassService : Service<Class, Int32>, IClassService
    {
        public ClassService(IUow uow)
            : base(uow)
        {
        }

        //used for FindByName in Manage
        public override string NameForFinding
        {
            get
            {
                return "Class_Name";
            }
        }

        public Class FindByClassName(string class_id)
        {
            //return FindByCriteria(c => c.Class_Name.ToLower() == class_id.ToLower());
            class_id = class_id.ToLower();
            return FindAllValid().FirstOrDefault(c => c.Class_Name.ToLower() == class_id);
        }

        public IEnumerable<Class> FindAll()
        {
            return Center_Id != 0 ?
                base.FindAll().Where(s => s.IsCenter(Center_Id)) :
                base.FindAll();
        }
    }
}
