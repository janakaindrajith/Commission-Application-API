namespace ComissionWebAPI.Models
{
    public class CommissionORDRateChart
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public int AgtTypeId { get; set; }
        public int ComLevelId { get; set; }
        public int ComYearId { get; set; }
        public int Rate { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public string Sql { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedDate { get; set; }
        public string EffectiveEndDate { get; set; }
        public int ActiveStatus { get; set; }
    }
}
