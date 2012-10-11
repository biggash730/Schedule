using System;
using System.ComponentModel.DataAnnotations;


namespace Schedule.Models
{
    public class Once
    {
        [Required]
        [Display(Name = "Schedule Date")]
        public DateTime ScheduleDate { get; set; }

        [Required]
        [Display(Name = "Hour")]
        public int ScheduleTimeHour { get; set; }

        [Required]
        [Display(Name = "Minute")]
        public int ScheduleTimeMinute { get; set; }
    }
}