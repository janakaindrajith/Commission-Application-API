namespace CommissionSystemREST.Models
{
    public class Language
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedDate { get; set; }
        public string EffectiveEndDate { get; set; }
        public int SeqNo { get; set; }
    }
}