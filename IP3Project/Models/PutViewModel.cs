using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IP3Project.Models
{
    public abstract class PutViewModel
    {
        public string AuthToken { get; set; }
        public string TenantId { get; set; }

    }
}
