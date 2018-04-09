using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
        [Required]
        public string Title { get; set; }
        public string Description { get; set; }
        [Required]
        [Display(Name = "Event Date and Time")]
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
        [Required]
        public string Title { get; set; }
        public string Description { get; set; }
        [Required]
        [Display(Name = "Date/Time")]
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

    public class CreateAttachment
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string TimelineEventId { get; set; }

        public CreateAttachment(string id, string title, string timelineEventId)
        {
            id = Id;
            title = Title;
            timelineEventId = TimelineEventId;
        }

    }

    public class AttachmentList
    {
        public List<Attachment> Attachments { get; set; }

        public AttachmentList()
        {
            Attachments = new List<Attachment>();
        }
    }

    public class EditDescription : PutViewModel
    {
        public string TimelineEventId { get; set; }
        [Required]
        public string Description { get; set; }

        public EditDescription(string Id, string description)
        {
            TimelineEventId = Id;
            Description = description;
        }

        public EditDescription()
        {

        }


    }

    public class EditDate : PutViewModel
    {
        public string TimelineEventId { get; set; }
        [Required]
        [Display(Name = "Date/Time")]
        public string EventDateTime { get; set; }

        public EditDate(string Id, string Date)
        {
            TimelineEventId = Id;
            EventDateTime = Date;
        }

        public EditDate()
        {

        }

        public DateTime GetDateTime()
        {
            DateTime dateTime = new DateTime((long.Parse(EventDateTime)));
            return dateTime;
        }


    }

    public class EditLocation : PutViewModel
    {
        public string TimelineEventId { get; set; }
        [Required]
        public string Location { get; set; }

        public EditLocation(string Id, string location)
        {
            TimelineEventId = Id;
            Location = location;
        }

        public EditLocation()
        {

        }



    }

    public class EditTitle : PutViewModel
    {
        public string TimelineEventId { get; set; }
        [Required]
        public string Title { get; set; }

        public EditTitle(string Id, string title)
        {
            TimelineEventId = Id;
            Title = title;
        }

        public EditTitle()
        {

        }



    }


}
