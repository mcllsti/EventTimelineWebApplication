using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IP3Project.Models


{

    public class AllTimelines
    {
        public List<TimelineEventViewModel> Timelines { get; set; }

        public AllTimelines()
        {
            Timelines = new List<TimelineEventViewModel>();
        }

    }


    public class TimelineEventViewModel
    {
        public string Title { get; set; }

        public string CreationTimeStamp { get; set; }
        public string Id { get; set; }
        public List<Event> TimelineEvents { get; set; }

        public TimelineEventViewModel()
        {
            TimelineEvents= new List<Event>();
        }

    }


    public class Event
    {

        public string Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string DateTime { get; set; }
        public string Location { get; set; }
        public List<Attachment> Attachments { get; set; }

        public Event()
        {
            Attachments = new List<Attachment>();
        }


    }

    public class Attachment
    {

        public string Id { get; set; }
        public string Title { get; set; }
        public string TimelineEventId { get; set; }
        
    }


}
