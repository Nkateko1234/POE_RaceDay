namespace POE_RaceDay.Models
{
    public class ResultViewModel
    {
        public int ResultID { get; set; }

        public string EventName { get; set; } = "";

        public DateTime EventDate { get; set; }

        public string CategoryName { get; set; } = "";

        public TimeSpan FinishTime { get; set; }

        public int Position { get; set; }

        public string ResultStatus { get; set; } = "";
    }
}
