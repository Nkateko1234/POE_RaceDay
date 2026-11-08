namespace POE_RaceDay.Models
{
    public class EventModel
    {
        public int EventID { get; set; }

        public int OrganiserID { get; set; }

        public string EventType { get; set; } = "";

        public string EventName { get; set; } = "";

        public DateTime EventDate { get; set; }

        public string EventLocation { get; set; } = "";

        public string EventDescription { get; set; } = "";
    }
}
