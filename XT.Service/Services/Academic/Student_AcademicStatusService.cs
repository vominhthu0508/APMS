using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XT.Model;
using XT.Repository;

namespace XT.BusinessService
{
    public class Student_AcademicStatusService : Service<Student_AcademicStatus, Int32>, IStudent_AcademicStatusService
    {
        public Student_AcademicStatusService(IUow uow)
            : base(uow)
        {
        }

        public override string NameForParent
        {
            get
            {
                return "Student_Id";
            }
        }
    }
}
