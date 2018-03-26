﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IP3Project.Models


{

    public class EventList
    {
        public List<Event> Events { get; set; }

        public EventList()
        {
            Events = new List<Event>();
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
