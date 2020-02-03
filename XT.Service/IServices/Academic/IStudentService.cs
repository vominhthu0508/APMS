using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XT.Model;

namespace XT.BusinessService
{
    public interface IStudentService : IService<Student, Int32>
    {
        Student UpdateClass(Student student, int Class_Id);
    }
}
