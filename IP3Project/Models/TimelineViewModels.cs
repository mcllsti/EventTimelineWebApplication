using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
    /// Viewmodel that will be used to display Timelines from the API
    /// </summary>
    public class TimelineViewModel
    {
        public string Title { get; set; }
        public DateTime CreationTimeStamp { get; set; }
        public string Id { get; set; }

        public TimelineViewModel(string title, string date, string id)
        {
            Title = title;
            CreationTimeStamp = new DateTime(long.Parse(date));
            Id = id;
        }

    }
}
