using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace IP3Project.Models
{
    public class AttachmentViewModel
    {
        public string TimelineEventId { get; set; }
        public string AttachmentId { get; set; }
        public string Title { get; set; }

        public AttachmentViewModel(string eventId)
        {
            TimelineEventId = eventId;
        }

    }

    public class AttachmentInputModel
    {
        [Required]
        [StringLength(30, ErrorMessage = "Title too long. Must be under 30 characters")]
        [Display(Name = "Attachment Title")]
        public string Title { get; set; }

    }

    public class AttachmentList
    {
        public List<AttachmentViewModel> Attachments { get; set; }

        public AttachmentList()
        {
            Attachments = new List<AttachmentViewModel>();
        }
    }

    public class CreateEditAttachmentViewModel : PutViewModel
    {
        public CreateEditAttachmentViewModel()
        {
        }

        public CreateEditAttachmentViewModel(string Id)
        {
            AttachmentId = Id;
        }

        [Required]
        [StringLength(30, ErrorMessage = "Title too long. Must be under 30 characters")]
        [Display(Name = "Attachment Title")]
        public string Title { get; set; }

        public string AttachmentId { get; set; }

    }
}
