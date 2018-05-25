namespace ComissionWebAPI.Models
{
    public class CommissionRateChart
    {
        public int Id { get; set; }
        public int CategoryId { get; set; }
        public int AgtTypeId { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public int TermLowerLimit { get; set; }
        public int TermUpperLimit { get; set; }
        public int YearLowerLimit { get; set; }
        public int YearUpperLimit { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedDate { get; set; }
        public string EffectiveEndDate { get; set; }
        public int ActiveStatus { get; set; }
        public double Rate { get; set; }
        public string Sql { get; set; }
        public string Product_Category { get; set; }
        public string Agent_Type { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }

    }
}
