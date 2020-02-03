using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XT.Model
{
    //Phần dành chung cho tất cả các loại Data Model
    #region StoredModelBase
    public partial class Role_Type : StoredModelBase<Int32>
    {
        public override int Obj_Id { get { return this.Id; } }
    }
    public partial class User_Type : StoredModelBase<Int32>
    {
        public override int Obj_Id { get { return this.Id; } }
    }
    public partial class Account : StoredModelBase<Int32>
    {
        public override int Obj_Id { get { return this.Id; } }
        public override bool IsCenter(int Center_Id)
        {
            return this.User_Profile != null && this.User_Profile.IsCenter(Center_Id);
        }
    }
    public partial class BookOrder : StoredModelBase<Int32>
    {
        public override int Obj_Id { get { return this.Id; } }
        public override bool IsCenter(int Center_Id)
        {
            return this.Company_Id == Center_Id;
        }
    }
    #region Class
    public partial class Class : StoredModelBase<Int32>
    {
        public override int Obj_Id { get { return this.Id; } }
        public override bool IsCenter(int Center_Id)
        {
            return this.Company_Id == Center_Id;
        }
    }
    public partial class Class_Module : StoredModelBase<Int32>
    {
        public override int Obj_Id { get { return this.Id; } }
        public override bool IsCenter(int Center_Id)
        {
            return this.Class != null && this.Class.IsCenter(Center_Id);
        }
    }
    public partial class Class_Module_Day : StoredModelBase<Int32>
    {
        public override int Obj_Id { get { return this.Id; } }
        public override bool IsCenter(int Center_Id)
        {
            return this.Class_Module != null && this.Class_Module.IsCenter(Center_Id);
        }
    }
    public partial class Class_Module_Day_Student : StoredModelBase<Int32>
    {
        public override int Obj_Id { get { return this.Id; } }
        public override bool IsCenter(int Center_Id)
        {
            return this.Class_Module_Day != null && this.Class_Module_Day.IsCenter(Center_Id);
        }
    }
    public partial class Class_Module_StudentExam : StoredModelBase<Int32>
    {
        public override int Obj_Id { get { return this.Id; } }
        public override bool IsCenter(int Center_Id)
        {
            return this.Class_Module != null && this.Class_Module.IsCenter(Center_Id);
        }
        public override bool IsValid()
        {
            return base.IsValid() && this.Student.IsValid();
        }
    }
    #endregion
    public partial class Company : StoredModelBase<Int32>
    {
        public override int Obj_Id { get { return this.Id; } }
    }
    public partial class Company_Type : StoredModelBase<Int32>
    {
        public override int Obj_Id { get { return this.Id; } }
        public override bool IsCenter(int Center_Id)
        {
            return this.Companies.Any(c => c.Id == Center_Id);
        }
    }
    public partial class Course : StoredModelBase<Int32>
    {
        public override int Obj_Id { get { return this.Id; } }
        public override bool IsCenter(int Center_Id)
        {
            return this.Company_Type != null && this.Company_Type.IsCenter(Center_Id);
        }
    }
    public partial class CourseFamily : StoredModelBase<Int32>
    {
        public override int Obj_Id { get { return this.Id; } }
        public override bool IsCenter(int Center_Id)
        {
            return this.Course != null && this.Course.IsCenter(Center_Id);
        }
    }
    public partial class Faculty : StoredModelBase<Int32>
    {
        public override int Obj_Id { get { return this.Id; } }
        public override bool IsCenter(int Center_Id)
        {
            return this.Company_Id == Center_Id;
        }
    }
    public partial class FeePlan : StoredModelBase<Int32>
    {
        public override int Obj_Id { get { return this.Id; } }
        public override bool IsCenter(int Center_Id)
        {
            return this.Company_Type != null && this.Company_Type.IsCenter(Center_Id);
        }
    }
    public partial class FeePlan_Detail : StoredModelBase<Int32>
    {
        public override int Obj_Id { get { return this.Id; } }
        public override bool IsCenter(int Center_Id)
        {
            return this.FeePlan != null && this.FeePlan.IsCenter(Center_Id);
        }
    }
    public partial class Module : StoredModelBase<Int32>
    {
        public override int Obj_Id { get { return this.Id; } }
        public override bool IsCenter(int Center_Id)
        {
            return this.CourseFamily != null && this.CourseFamily.IsCenter(Center_Id);
        }
    }
    public partial class Prize : StoredModelBase<Int32>
    {
        public override int Obj_Id { get { return this.Id; } }
        public override bool IsCenter(int Center_Id)
        {
            return this.Student != null && this.Student.IsCenter(Center_Id);
        }
    }
    public partial class Resource : StoredModelBase<Int32>
    {
        public override int Obj_Id { get { return this.Id; } }
        public override bool IsCenter(int Center_Id)
        {
            return this.Company_Id == Center_Id;
        }
    }
    #region Student
    public partial class Student : StoredModelBase<Int32>
    {
        public override int Obj_Id { get { return this.Id; } }
        public override bool IsCenter(int Center_Id)
        {
            return this.Class != null && this.Class.IsCenter(Center_Id);
        }
    }
    public partial class Student_AcademicStatus : StoredModelBase<Int32>
    {
        public override int Obj_Id { get { return this.Id; } }
        public override bool IsCenter(int Center_Id)
        {
            return this.Student != null && this.Student.IsCenter(Center_Id);
        }
    }
    public partial class Student_ClassHistory : StoredModelBase<Int32>
    {
        public override int Obj_Id { get { return this.Id; } }
        public override bool IsCenter(int Center_Id)
        {
            return this.Student != null && this.Student.IsCenter(Center_Id);
        }
    }
    public partial class Student_FeePlan : StoredModelBase<Int32>
    {
        public override int Obj_Id { get { return this.Id; } }
        public override bool IsCenter(int Center_Id)
        {
            return this.Student != null && this.Student.IsCenter(Center_Id);
        }
    }
    public partial class Student_FeePlan_Installment : StoredModelBase<Int32>
    {
        public override int Obj_Id { get { return this.Id; } }
        public override bool IsCenter(int Center_Id)
        {
            return this.Student_FeePlan != null && this.Student_FeePlan.IsCenter(Center_Id);
        }
    }
    #endregion
    #endregion

    #region ModelBase
    public partial class BookOrder_Detail : ModelBase<Int32>
    {
        public override int Obj_Id { get { return this.Id; } }
        public override bool IsCenter(int Center_Id)
        {
            return this.Student != null && this.Student.IsCenter(Center_Id);
        }
    }
    public partial class Faculty_Module : ModelBase<Int32>
    {
        public override int Obj_Id { get { return this.Id; } }
        public override bool IsCenter(int Center_Id)
        {
            return this.Faculty != null && this.Faculty.IsCenter(Center_Id);
        }
    }
    public partial class User_Company : ModelBase<Int32>
    {
        public override int Obj_Id { get { return this.Id; } }
    }
    #endregion

    #region RegisterEntity
    public partial class User_Profile : RegisterModelBase<Int32>
    {
        public override int Obj_Id { get { return this.Id; } }
        public override bool IsCenter(int Center_Id)
        {
            return this.Companies_List.Any(c => c.Id == Center_Id);
        }

        public override string Name
        {
            get
            {
                return this.User_Profile_Name;
            }
        }

        public override string Email
        {
            get
            {
                return this.User_Profile_Email;
            }
        }

        public override string Avatar
        {
            get
            {
                return this.User_Profile_Avatar;
            }
        }
    }
    #endregion
}
