using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XT.Model;

namespace XT.BusinessService
{
    public interface ICourseFamilyService : IService<CourseFamily, Int32>
    {
        IEnumerable<CourseFamily> FindByName(string name, int parentId = 0);
    }
}
