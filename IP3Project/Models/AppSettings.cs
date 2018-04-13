using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IP3Project.Models
{
    /// <summary>
    /// Class used for Dependency injection of appsettings.JSON
    /// </summary>
    public class AppSettings
    {
        public string BaseUrl { get; set; }
        public string TenantId { get; set; }
        public string AuthToken { get; set; }
    }
}
