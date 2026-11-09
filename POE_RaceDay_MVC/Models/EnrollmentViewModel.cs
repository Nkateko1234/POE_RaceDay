namespace POE_RaceDay_MVC.Models
{
    public class EnrollmentViewModel
    {
        public int EnrollmentID { get; set; }

        public string ParticipantName { get; set; } = "";
        public string Surname { get; set; } = "";
        public string Email { get; set; } = "";

        public string EventName { get; set; } = "";
        public DateTime EventDate { get; set; }
        public string EventLocation { get; set; } = "";

        public string CategoryName { get; set; } = "";
        public string Distance { get; set; } = "";

        public decimal EntryFee { get; set; }

        public DateTime EnrollmentDate { get; set; }
        public string EnrollmentStatus { get; set; } = "";
    }
}
