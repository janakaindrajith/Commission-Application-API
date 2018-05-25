namespace ComissionWebAPI.Models
{
    public class property
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Type { get; set; }
        public string Description { get; set; }
        public int ActiveStatus { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedDate { get; set; }
        public string EffectiveEndDate { get; set; }
    }
}
