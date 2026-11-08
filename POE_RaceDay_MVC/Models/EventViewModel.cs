using System.ComponentModel.DataAnnotations;

namespace POE_RaceDay_MVC.Models
{
    public class EventViewModel
    {
        public int EventID { get; set; }

        public int OrganiserID { get; set; }

        [Required]
        public string EventType { get; set; } = "";

        [Required]
        public string EventName { get; set; } = "";

        [Required]
        public DateTime EventDate { get; set; }

        [Required]
        public string EventLocation { get; set; } = "";

        [Required]
        public string EventDescription { get; set; } = "";
    }
}
