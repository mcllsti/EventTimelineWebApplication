using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IP3Project.Models


{

    public class Timelinez
    {
        public List<TimelineEventViewModel> Timelines { get; set; }

        public Timelinez()
        {
            Timelines = new List<TimelineEventViewModel>();
        }

    }


    public class TimelineEventViewModel
    {
        public string Title { get; set; }

        public string CreationTimeStamp { get; set; }
        public string Id { get; set; }
        public List<TimelineEventsViewModel> TimelineEvents { get; set; }

        public TimelineEventViewModel()
        {
            TimelineEvents= new List<TimelineEventsViewModel>();
        }

    }


    public class TimelineEventsViewModel
    {

        public string Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string tDateTime { get; set; }
        public string Location { get; set; }
        public List<tempAttachment> Attachments { get; set; }

        public TimelineEventsViewModel()
        {
            Attachments = new List<tempAttachment>();
        }

        //LINKEDEVENTS?
        //ATTACHEMENTS?

    }

    public class tempAttachment
    {

        public string Id { get; set; }
        public string Title { get; set; }
        public string TimelineEventId { get; set; }
        
    }


}
