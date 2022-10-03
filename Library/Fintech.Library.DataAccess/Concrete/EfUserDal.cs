using Fintech.Library.DataAccess.Context;
using Fintech.Library.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fintech.Library.DataAccess.Concrete
{
    public class EfUserDal : EfEntityRepositoryBase<ProjectDbContext, User>, IUserDal
    {


   
    }
}
