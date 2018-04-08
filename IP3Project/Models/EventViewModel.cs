using System;
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

    public class DeleteEventViewModel : PutViewModel
    {

        public string TimelineEventId { get; set; }

        public DeleteEventViewModel(string id)
        {
            TimelineEventId = id;
        }

    }


    public class TimelineEventLink : PutViewModel
    {
        public string TimelineId { get; set; }
        public string EventId { get; set; }

        public TimelineEventLink()
        {

        }

        public TimelineEventLink(string timelineEventId,string eventId)
        {
            TimelineId = timelineEventId;
            EventId = eventId;
        }




    }


    public class CreateEventView : PutViewModel
    {
        public string TimelineEventId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string EventDateTime { get; set; }
        public string Location { get; set; }
        public string TimelineId { get; set; }

        public CreateEventView(String Id)
        {
            TimelineId = Id;
        }

        public CreateEventView()
        {
        }
    }

    public class Event
    {

        public string Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string EventDateTime { get; set; }
        public string Location { get; set; }
        public List<Attachment> Attachments { get; set; }

        public Event()
        {
            Attachments = new List<Attachment>();
        }

        public DateTime GetDateTime(string dateTimeString)
        {
            DateTime dateTime = new DateTime((long.Parse(dateTimeString)));

            return dateTime;
        }


    }

    public class Attachment
    {

        public string Id { get; set; }
        public string Title { get; set; }
        public string TimelineEventId { get; set; }
        
    }

    public class AttachmentList
    {
        public List<Attachment> Attachments { get; set; }

        public AttachmentList()
        {
            Attachments = new List<Attachment>();
        }
    }


}
