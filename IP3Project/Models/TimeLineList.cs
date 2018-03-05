using IP3Project.Classes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace IP3Project.Models
                    
{
    /// <summary>
    /// A Viewmodel that will contain a List of TimelineViewModels for output
    /// </summary>
    public class TimelineList
    {
        public List<TimelineViewModel> timelines { get; set; }

        public TimelineList()
        {
            timelines = new List<TimelineViewModel>();
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
    /// CreateViewModel will add timelines 
    /// </summary>
    public class CreateViewModel : PutViewModel
    {
        [Required]
        [StringLength(30, ErrorMessage = "Title too long. Must be under 30 characters")]
        [Display(Name = "Timeline Name")]
        public string Title { get; set; }

        public string TimelineId { get; set; }

    }


}
