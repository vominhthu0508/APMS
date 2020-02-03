using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XT.Model;
using XT.Repository;

namespace XT.BusinessService
{
    public class ModuleService : Service<Module, Int32>, IModuleService
    {
        public ModuleService(IUow uow)
            : base(uow)
        {
        }

        //used for FindByName in Manage
        public override string NameForFinding
        {
            get
            {
                return "Module_Name";
            }
        }

        public IEnumerable<Module> FindAllValid()
        {
            return Center_Id != 0 ?
                base.FindAllValid().Where(s => s.IsCenter(Center_Id)) :
                base.FindAllValid();
        }

        public override bool CheckExistIdentity(Module current)
        {
            var code = current.Module_Code.ToLower().Trim();
            return FindAllValid().Any(m => 
                m.Id != current.Id && 
                m.CourseFamily_Id == current.CourseFamily_Id && m.Module_Code.ToLower().Equals(code));//sửa
        }
    }
}
