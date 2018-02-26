using System;
namespace IP3Project.Models
{
    public class Timeline
    {
        public string Title { get; set; }
        public string CreationTimeStamp { get; set; }
        public bool IsDeleted { get; set; }
        public string Id { get; set; }
        public string TenantId { get; set; }
    }
}
