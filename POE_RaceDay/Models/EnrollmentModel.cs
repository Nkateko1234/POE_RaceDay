namespace POE_RaceDay.Models
{
    public class EnrollmentModel
    {
        public int EnrollmentID { get; set; }

        // Participant details
        public string ParticipantName { get; set; } = "";
        public string Surname { get; set; } = "";
        public string Email { get; set; } = "";

        // Event details
        public string EventName { get; set; } = "";
        public DateTime EventDate { get; set; }
        public string EventLocation { get; set; } = "";

        // Category details
        public string CategoryName { get; set; } = "";
        public string Distance { get; set; } = "";
        public decimal EntryFee { get; set; }

        // Enrolment details
        public DateTime EnrollmentDate { get; set; }
        public string EnrollmentStatus { get; set; } = "";
    }
}
