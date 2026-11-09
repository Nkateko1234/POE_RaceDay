namespace POE_RaceDay.Models
{
    public class CategoryModel
    {
        public int CategoryID { get; set; }

        public int EventID { get; set; }

        public string CategoryName { get; set; } = "";

        public string Distance { get; set; } = "";

        public decimal EntryFee { get; set; }
    }
}
