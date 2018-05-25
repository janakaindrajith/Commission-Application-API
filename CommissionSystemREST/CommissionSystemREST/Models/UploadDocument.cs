namespace ComissionWebAPI.Models
{
    public class UploadDoc
    {
        public int Id { get; set; }
        public string AgtCode { get; set; }
        public int DocTypeId { get; set; }
        public string DocTypeDesc { get; set; }
        public string DocUrl { get; set; }
        public string DocDescription { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedDate { get; set; }
        public string EffectiveEndDate { get; set; }
    }
}
