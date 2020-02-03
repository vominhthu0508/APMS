using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XT.Model;

namespace XT.BusinessService
{
    public interface IClassService : IService<Class, Int32>
    {
        Class FindByClassName(string class_id);
    }
}
