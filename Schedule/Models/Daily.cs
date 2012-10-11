using System;
using System.ComponentModel.DataAnnotations;

namespace Schedule.Models
{
    public class Daily
    {
        [Required]
        [Display(Name = "Hour")]
        public int ScheduleTimeHour { get; set; }

        [Required]
        [Display(Name = "Minute")]
        public int ScheduleTimeMinute { get; set; }
    }
}