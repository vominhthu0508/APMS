using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XT.Model;
using XT.Repository;

namespace XT.BusinessService
{
    public class StudentService : Service<Student, Int32>, IStudentService
    {
        public StudentService(IUow uow)
            : base(uow)
        {
        }

        //used for FindByName in Manage
        public override string NameForFinding
        {
            get
            {
                return "Student_FullName";
            }
        }

        public IEnumerable<Student> FindAllValid()
        {
            return Center_Id != 0 ?
                base.FindAllValid().Where(s => s.IsCenter(Center_Id)) :
                base.FindAllValid();
        }

        public Student UpdateClass(Student student, int Class_Id)
        {
            if (student.Class_Id != Class_Id)
            {
                student.Student_ClassHistories.Add(new Student_ClassHistory { 
                    Class_Id = Class_Id,
                    Created_Date = DateTime.Now,
                    Status = (int)EntityStatus.Visible,
                    StartDate = DateTime.Now
                });
                student.Class_Id = Class_Id;

                return Update(student);
            }

            return null;
        }
    }
}
