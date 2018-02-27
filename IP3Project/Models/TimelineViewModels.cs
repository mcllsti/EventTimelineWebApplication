using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IP3Project.Models
{

    public class TimelineList
    {
        public List<TimelineViewModel> timelines { get; set; }

        public TimelineList()
        {
            timelines = new List<TimelineViewModel>();
        }

    }

    public class putviewmodel
    {
        public string AuthToken { get; set; }
        public string TenantId { get; set; }

    }

    public class DeleteViewModel : putviewmodel
    {
        public string TimelineId { get; set; }




    }


    public class TimelineViewModel
    {
        public string Title { get; set; }
        public DateTime CreationTimeStamp { get; set; }
        public string Id { get; set; }

        public TimelineViewModel(string title, string date, string id)
        {
            Title = title;
            CreationTimeStamp = new DateTime(long.Parse(date));
            Id = id;
        }

    }
}
