using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XT.Model;
using XT.Repository;

namespace XT.BusinessService
{
    public class FacultyService : Service<Faculty, Int32>, IFacultyService
    {
        public FacultyService(IUow uow)
            : base(uow)
        {
        }

        //used for FindByName in Manage
        public override string NameForFinding
        {
            get
            {
                return "FC_Name";
            }
        }

        public IEnumerable<Faculty> FindAllValid()
        {
            return Center_Id != 0 ?
                base.FindAllValid().Where(s => s.IsCenter(Center_Id)) :
                base.FindAllValid();
        }
    }

    public class Faculty_ModuleService : Service<Faculty_Module, Int32>, IFaculty_ModuleService
    {
        public Faculty_ModuleService(IUow uow)
            : base(uow)
        {
        }

        //used for FindByName in Manage
        //public override string NameForFinding
        //{
        //    get
        //    {
        //        return "FC_Name";
        //    }
        //}
    }
}
