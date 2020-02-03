using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XT.Model;

namespace XT.BusinessService
{
    public interface IFacultyService : IService<Faculty, Int32>
    {
    }

    public interface IFaculty_ModuleService : IService<Faculty_Module, Int32>
    {
    }
}
