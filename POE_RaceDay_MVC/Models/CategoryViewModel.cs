namespace POE_RaceDay_MVC.Models
{
    public class CategoryViewModel
    {
        public int CategoryID { get; set; }

        public int EventID { get; set; }

        public string CategoryName { get; set; } = "";

        public string Distance { get; set; } = "";

        public decimal EntryFee { get; set; }
    }
}
