using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IP3Project.Models
{
    //View Models that concern the interaction and displaying of Attachments

    /// <summary>
    /// View model that will be used to display create attachment form and the posting of the form to controller
    /// </summary>
    public class CreateAttachmentViewModel
    {
        [Required]
        public IFormFile File { get; set; } //FILE variable
        public string TimelineEventId { get; set; }

        public CreateAttachmentViewModel(IFormFile file, string timelineEventId)
        {
            File = file;
            timelineEventId = TimelineEventId;
        }

        public CreateAttachmentViewModel()
        {

        }

        public CreateAttachmentViewModel(string timelineEventId)
        {
            timelineEventId = TimelineEventId;
        }

    }

    /// <summary>
    /// Class used only to temporaily get serilized data from API before turning into attachment
    /// </summary>
    public class AttachmentViewModel
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string TimelineEventId { get; set; }
    }

    /// <summary>
    /// Class used to delete attachments
    /// </summary>
    public class DeleteAttachmentViewModel : PutViewModel
    {
        public string AttachmentId { get; set; }

        public DeleteAttachmentViewModel()
        {

        }

        public DeleteAttachmentViewModel(string Id)
        {
            AttachmentId = Id;
        }
    }
}
