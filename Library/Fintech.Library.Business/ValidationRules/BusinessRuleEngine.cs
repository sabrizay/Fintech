using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fintech.Library.Business.ValidationRules
{
    public static class BusinessRuleEngine
    {
        public static BaseResponse Validate(params BaseResponse[] rules)
        {
            foreach (var result in rules)
            {
                if (!result.Success)
                    return result;
            }
            return new BaseResponse { Success = true };
        }
    }
}
