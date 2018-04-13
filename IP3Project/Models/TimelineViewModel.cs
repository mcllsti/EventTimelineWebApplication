using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IP3Project.Models
{
    //View Models that concern the interaction and displaying of Timelines


    /// <summary>
    /// A Viewmodel that will contain a List of TimelineViewModels for output
    /// </summary>
    public class TimelineListViewModel
    {
        public List<Timeline> Timelines { get; set; }

        public TimelineListViewModel()
        {
            Timelines = new List<Timeline>();
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
    /// CreateEditViewModel will be used to add Timelines and edit them
    /// </summary>
    public class CreateEditViewModel : PutViewModel
    {
        public CreateEditViewModel()
        {
        }

        public CreateEditViewModel(string Id,string title)
        {
            TimelineId = Id;
            Title = title;
        }

        public CreateEditViewModel(string Id)
        {
            TimelineId = Id;
        }


        [Required]
        [StringLength(50, ErrorMessage = "Title too long. Must be under 50 characters")]
        [RegularExpression(@"^\S.+\S$", ErrorMessage = "Title not valid!")]
        [Display(Name = "Timeline Title")]
        public string Title { get; set; }

        public string TimelineId { get; set; }

    }


}
