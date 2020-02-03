using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XT.Model;
using XT.Repository;

namespace XT.BusinessService
{
    public class BookOrderService : Service<BookOrder, Int32>, IBookOrderService
    {
        public BookOrderService(IUow uow)
            : base(uow)
        {
        }

        
    }
}
