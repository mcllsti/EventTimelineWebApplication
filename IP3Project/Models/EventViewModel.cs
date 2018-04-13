using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IP3Project.Models

    //View Models that concern the interactions and displaying of Events
{
    /// <summary>
    /// A viewmodel containing a list of Events to be displayed
    /// </summary>
    public class EventListViewModel
    {
        public List<Event> Events { get; set; }

        public EventListViewModel()
        {
            Events = new List<Event>();
        }

    }

    /// <summary>
    /// View model for deleteing an Event from a Timeline
    /// </summary>
    public class DeleteEventViewModel : PutViewModel
    {

        public string TimelineEventId { get; set; }

        public DeleteEventViewModel(string id)
        {
            TimelineEventId = id;
        }

    }

    /// <summary>
    /// View model that has the main purpose of creating an event. Validation is included
    /// </summary>
    public class CreateEventViewModel : PutViewModel
    {
        public string TimelineEventId { get; set; }
        [Required]
        [StringLength(25, ErrorMessage = "Title too long. Must be under 30 characters")]
        [RegularExpression(@"^\S.+\S$", ErrorMessage = "Title not valid!")]
        public string Title { get; set; }
        public string Description { get; set; }
        [Required]
        [Display(Name = "Event Date and Time")]
        public string EventDateTime { get; set; }
        public string Location { get; set; }
        public string TimelineId { get; set; }

        public CreateEventViewModel(String Id)
        {
            TimelineId = Id;
        }

        public CreateEventViewModel()
        {
        }
    }

    /// <summary>
    /// View model that deals with the editing of descriptions and its form
    /// </summary>
    public class EditDescriptionViewModel : PutViewModel
    {
        public string TimelineEventId { get; set; }
        [Required]
        public string Description { get; set; }

        public EditDescriptionViewModel(string Id, string description)
        {
            TimelineEventId = Id;
            Description = description;
        }

        public EditDescriptionViewModel()
        {

        }


    }

    /// <summary>
    /// Edit Date viewmodel deals with the editing of the dates as well as parseing functions
    /// </summary>
    public class EditDateViewModel : PutViewModel
    {
        public string TimelineEventId { get; set; }
        [Required]
        [Display(Name = "Date/Time")]
        public string EventDateTime { get; set; }

        public EditDateViewModel(string Id, string Date)
        {
            TimelineEventId = Id;
            EventDateTime = Date;
        }

        public EditDateViewModel()
        {

        }

        /// <summary>
        /// Used to get a DateTime from the EventDateTime string
        /// </summary>
        /// <returns>DateTime version of date time string</returns>
        public DateTime GetDateTime() 
        {
            DateTime dateTime = new DateTime((long.Parse(EventDateTime)));
            return dateTime;
        }


    }

    /// <summary>
    /// View model that deals with the editing and form handling of Locations
    /// </summary>
    public class EditLocationViewModel : PutViewModel
    {
        public string TimelineEventId { get; set; }
        [Required]
        public string Location { get; set; }

        public EditLocationViewModel(string Id, string location)
        {
            TimelineEventId = Id;
            Location = location;
        }

        public EditLocationViewModel()
        {

        }



    }

    /// <summary>
    /// Edit Title is a view model that concersn the editing and validation of a Event Title
    /// </summary>
    public class EditTitleViewModel : PutViewModel
    {
        public string TimelineEventId { get; set; }
        [Required]
        [RegularExpression(@"^\S.+\S$", ErrorMessage = "Title not valid!")]
        [StringLength(25, ErrorMessage = "Title too long. Must be under 30 characters")]
        public string Title { get; set; }

        public EditTitleViewModel(string Id, string title)
        {
            TimelineEventId = Id;
            Title = title;
        }

        public EditTitleViewModel()
        {

        }



    }


}
