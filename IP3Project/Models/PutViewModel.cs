using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IP3Project.Models
{
    /// <summary>
    /// Abstract class that all ViewModels for PUT methods will inherit from
    /// </summary>
    public abstract class PutViewModel
    {
        public string AuthToken { get; set; }
        public string TenantId { get; set; }

    }
}
