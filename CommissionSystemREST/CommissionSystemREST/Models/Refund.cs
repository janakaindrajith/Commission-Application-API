using System;

namespace ComissionWebAPI.Models
{
    public class Refund
    {
        public Int64 RfdId { get; set; }
        public string RfdReceiptNo { get; set; }
        public string RfdRefundDate { get; set; }
        public double RfdType { get; set; }
        public double RfdAmt { get; set; }
        public double RfdPercentage { get; set; }
        public string RfdBy { get; set; }
        public string RfdReason { get; set; }
        public string RfdAgtCode { get; set; }
        public string RfdProcessInd { get; set; }
        public string RfdRvNo { get; set; }
        public string RfdPvNo { get; set; }
        public string RfdBalType { get; set; }
        public string RfdCreatedBy { get; set; }
        public string RfdCreatedDate { get; set; }
        public string RfdEffectiveEndDate { get; set; }
        public Int16 RfdStatus { get; set; }
        public string RfdProposalNo { get; set; }
        public string RfdPolicyNo { get; set; }
        public double RfdCancellationFee { get; set; }
        public double RfdRecoveryFee { get; set; }
        public string RfdRecStatus { get; set; }
        public string RfdRecNarration { get; set; }
        public string RfdRecUpdatedBy { get; set; }
        public string RfdRecUpdatedDate { get; set; }
        public string RfdPaymentMTD { get; set; }
    }
}
