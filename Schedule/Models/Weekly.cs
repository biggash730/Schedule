using System.ComponentModel.DataAnnotations;

namespace Schedule.Models
{
    public class Weekly
    {
        [Required]
        [Display(Name = "Hour")]
        public int ScheduleTimeHour { get; set; }

        [Required]
        [Display(Name = "Minute")]
        public int ScheduleTimeMinute { get; set; }

        [Required]
        [Display(Name = "Sunday")]
        public int Sunday { get; set; }

        [Required]
        [Display(Name = "Monday")]
        public int Monday { get; set; }

        [Required]
        [Display(Name = "Tuesday")]
        public int Tuesday { get; set; }

        [Required]
        [Display(Name = "Wednesday")]
        public int Wednesday { get; set; }

        [Required]
        [Display(Name = "Thursday")]
        public int Thursday { get; set; }

        [Required]
        [Display(Name = "Friday")]
        public int Friday { get; set; }

        [Required]
        [Display(Name = "Saturday")]
        public int Saturday { get; set; }
    }
}