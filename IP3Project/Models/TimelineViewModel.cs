﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IP3Project.Models
{


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

        public DateTime GetDateTime()
        {
            DateTime dateTime = new DateTime((long.Parse(CreationTimeStamp)));
            return dateTime;
        }


    }

    public class TimelineInputModel
    {

        [Required]
        [StringLength(30, ErrorMessage = "Title too long. Must be under 30 characters")]
        [Display(Name = "Timeline Name")]
        public string TimlineName { get; set; }

    }

    /// <summary>
    /// A Viewmodel that will contain a List of TimelineViewModels for output
    /// </summary>
    public class TimelineList
    {
        public List<Timeline> timelines { get; set; }

        public TimelineList()
        {
            timelines = new List<Timeline>();
        }

    }


    /// <summary>
    /// DeleteViewModel that will PUT a delete request in
    /// </summary>
    public class DeleteViewModel : PutViewModel
    {
        public string TimelineId { get; set; }

    }


    /// <summary>
    /// CreateEditViewModel will add timelines 
    /// </summary>
    public class CreateEditViewModel : PutViewModel
    {
        public CreateEditViewModel()
        {
        }

        public CreateEditViewModel(string Id)
        {
            TimelineId = Id;
        }


        [Required]
        [StringLength(30, ErrorMessage = "Title too long. Must be under 30 characters")]
        [Display(Name = "Timeline Title")]
        public string Title { get; set; }

        public string TimelineId { get; set; }

    }

    public class SystemViewModel
    {
        public string Title { get; set; }
        [Display(Name = "Date/Time")]
        public DateTime CreationTimeStamp { get; set; }
        public string Id { get; set; }

        public SystemViewModel(string title, string date, string id)
        {
            Title = title;
            CreationTimeStamp = new DateTime(long.Parse(date));
            Id = id;
        }

    }
}
