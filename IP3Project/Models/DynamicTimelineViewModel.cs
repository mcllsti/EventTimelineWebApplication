using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IP3Project.Models
{
    public class DynamicTimelineViewModel
    {
        [Display(Name = "Title")]
        public string Title { get; set; }
        [Display(Name = "Date/Time")]
        public string DateCreated { get; set; }
        public string Id { get; set; }
        public List<Event> AllEvents { get; set; }

        public DynamicTimelineViewModel(string id)
        {
            Id = id;
        }

    }
}
