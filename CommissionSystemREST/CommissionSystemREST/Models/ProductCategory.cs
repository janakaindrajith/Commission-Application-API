﻿namespace ComissionWebAPI.Models
{
    public class productcategory
    {
        public int Id { get; set; }
        public string Type { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public string BussinessType { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedDate { get; set; }
        public string EffectiveEndDate { get; set; }
        public int ActiveStatus { get; set; }
    }
}
