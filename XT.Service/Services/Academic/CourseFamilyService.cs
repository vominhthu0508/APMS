using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XT.Model;
using XT.Repository;

namespace XT.BusinessService
{
    public class CourseFamilyService : Service<CourseFamily, Int32>, ICourseFamilyService
    {
        public CourseFamilyService(IUow uow)
            : base(uow)
        {
        }

        //used for FindByName in Manage
        public override string NameForFinding
        {
            get
            {
                return "CourseFamily_Name";
            }
        }

        public override string NameForParent
        {
            get
            {
                return "Course_Id";
            }
        }

        public IEnumerable<CourseFamily> FindAllValid()
        {
            return Center_Id != 0 ?
                base.FindAllValid().Where(s => s.IsCenter(Center_Id)) :
                base.FindAllValid();
        }
    }
}
