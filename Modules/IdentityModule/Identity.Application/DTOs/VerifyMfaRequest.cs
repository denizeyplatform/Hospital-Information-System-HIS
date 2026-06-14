using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Identity.Application.DTOs
{
    public class VerifyMfaRequest
    {
        public string UserId { get; set; } = string.Empty;

        public string Code { get; set; } = string.Empty;
    }
}
