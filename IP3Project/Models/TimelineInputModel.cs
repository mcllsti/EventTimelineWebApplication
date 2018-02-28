using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IP3Project.Models
{
    public class TimelineInputModel
    {

        [Required]
        [StringLength(30, ErrorMessage ="Title too long. Must be under 30 characters")]
        [Display(Name ="Timeline Name")]
        public string TimlineName { get; set; }

    }
}
