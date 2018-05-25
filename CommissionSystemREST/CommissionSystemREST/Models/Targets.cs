namespace ComissionWebAPI.Models
{
    public class targets
    {
        public int Id { get; set; }
        public int LevelId { get; set; }
        public int TypeId { get; set; }
        public int Year { get; set; }
        public int YearlyTarget { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedDate { get; set; }
        public string EffectiveEndDate { get; set; }
        public int ActiveStatus { get; set; }
    }

}
