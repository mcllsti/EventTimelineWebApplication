using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IP3Project.Models
{
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
