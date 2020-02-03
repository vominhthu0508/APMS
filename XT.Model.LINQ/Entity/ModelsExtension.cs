using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

////////////////////////////////////////////////////////////////////////////////////
//Model for LINQ
//This file is for private extensions such as GetLocation by XML

namespace XT.Model
{
    public partial class Student
    {
        public EntitySet<Student_AcademicStatus> Student_AcademicStatuses
        {
            get
            {
                return this.Student_AcademicStatus;
            }
            set
            {
                this.Student_AcademicStatus.Assign(value);
            }
        }
    }

    public partial class Prize
    {
        public Class_Module_StudentExam Exam
        {
            get
            {
                return this.Class_Module_StudentExam;
            }
            set
            {
                this.Class_Module_StudentExam = value;
            }
        }
    }

    public partial class Class_Module
    {
        public Resource Resource_LT
        {
            get
            {
                return this.Resource1;
            }
            set
            {
                this.Resource1 = value;
            }
        }

        public Resource Resource_TH
        {
            get
            {
                return this.Resource2;
            }
            set
            {
                this.Resource2 = value;
            }
        }

        public Resource Resource_Exam
        {
            get
            {
                return this.Resource;
            }
            set
            {
                this.Resource = value;
            }
        }
    }

    public partial class Course
    {
        public Course Parent_Course
        {
            get
            {
                return this.Course1;
            }
            set
            {
                this.Course1 = value;
            }
        }
    }
}
