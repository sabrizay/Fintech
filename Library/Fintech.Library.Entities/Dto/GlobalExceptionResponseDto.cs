using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fintech.Library.Entities.Dto
{
    public class GlobalExceptionResponseDto : IDto
    {
        private string _message;
        public GlobalExceptionResponseDto(string message)
        {
            _message = message;
        }
    }

}
