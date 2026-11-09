using System.ComponentModel.DataAnnotations;

namespace POE_RaceDay_MVC.Models
{
    public class CaptureResultViewModel
    {
        [Required]
        public int EnrollmentID { get; set; }

        [Required]
        public TimeSpan FinishTime { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        public int Position { get; set; }
    }
}
