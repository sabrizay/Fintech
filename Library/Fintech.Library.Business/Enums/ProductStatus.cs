using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fintech.Library.Business.Enums
{
    public enum NoSqlProductStatus : int
    {
        InReview = 1,
        Rejecting = 2,
        Rejected = 3,
        Confirming = 4,
        Confirmed = 5
    }

    public enum ProductStatus : int
    {
        Confirmed = 1,
        UnConfirmed = 2
    }
}
