namespace ComissionWebAPI.Models
{
    public class FSTAllowance
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public int MaximumYears { get; set; }
        public int MaximumAllowance { get; set; }
        public int PolicyCount { get; set; }
        public int FstMinimumAmt { get; set; }
        public int FstMaximumAmt { get; set; }
        public int Allowance { get; set; }
        public string Sql { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public int ActiveStatus { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedDate { get; set; }
        public string EffectiveEndDate { get; set; }
    }

}
