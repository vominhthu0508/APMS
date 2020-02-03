using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XT.Model;
using XT.Repository;

namespace XT.BusinessService
{
    public class Student_FeePlanService : Service<Student_FeePlan, Int32>, IStudent_FeePlanService
    {
        public Student_FeePlanService(IUow uow)
            : base(uow)
        {
        }

        
    }
}
