using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XT.Model;
using XT.Repository;

namespace XT.BusinessService
{
    public class CompanyService : Service<Company, Int32>, ICompanyService
    {
        public CompanyService(IUow uow)
            : base(uow)
        {
        }

        //used for FindByName in Manage
        public override string NameForFinding
        {
            get
            {
                return "Company_Name";
            }
        }
    }
}
