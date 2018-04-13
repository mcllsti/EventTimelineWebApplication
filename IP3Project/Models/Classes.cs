using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IP3Project.Models
{
    //Master classes that attribute to a system

    /// <summary>
    /// Timeline is a representation of a overall Timeline
    /// It contains a list of events that are part of that perticular Timeline
    /// </summary>
    public class Timeline
    {
        public string Id { get; set; }
        public string Title { get; set; }
        [Display(Name = "Date/Time")]
        public string CreationTimeStamp { get; set; }

        public List<Event> TimelineEvents { get; set; }

        public Timeline()
        {
            TimelineEvents = new List<Event>();
        }

        public Timeline(string id)
        {
            Id = id;
            TimelineEvents = new List<Event>();
        }

        /// <summary>
        /// Parses a string to a new DateTime
        /// </summary>
        /// <returns>DateTime from the CreationTimeStamp</returns>
        public DateTime GetDateTime()
        {
            DateTime dateTime = new DateTime((long.Parse(CreationTimeStamp)));
            return dateTime;
        }


    }

    /// <summary>
    /// Event is a representation of events that make up a single Timeline
    /// Each event may have attachments that are part of that event
    /// </summary>
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

        /// <summary>
        /// Deals with the parsing of EventDateTime
        /// </summary>
        /// <param name="dateTimeString">DateTimeString to parse</param>
        /// <returns>DateTime of parsed string</returns>
        public DateTime GetDateTime(string dateTimeString)
        {
            DateTime dateTime = new DateTime((long.Parse(dateTimeString) ));

            return dateTime;
        }


    }

    /// <summary>
    /// Attachment is a representation of attachment files that are part of an Event
    /// </summary>
    public class Attachment : PutViewModel
    {

        public string AttachmentId { get; set; }
        public string Title { get; set; }
        public string TimelineEventId { get; set; }

        public Attachment()
        {

        }

        public Attachment(string tempId, string title, string EventId)
        {
            AttachmentId = tempId;
            Title = title;
            TimelineEventId = EventId;
        }

    }

    /// <summary>
    /// Deals with the linking between an Event and a Timeline
    /// </summary>
    public class TimelineEventLink : PutViewModel
    {
        public string TimelineId { get; set; }
        public string EventId { get; set; }

        public TimelineEventLink()
        {

        }

        public TimelineEventLink(string timelineEventId, string eventId)
        {
            TimelineId = timelineEventId;
            EventId = eventId;
        }

    }

}
