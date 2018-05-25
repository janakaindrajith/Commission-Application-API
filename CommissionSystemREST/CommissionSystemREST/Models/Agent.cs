using System;

namespace ComissionWebAPI.Models
{
    public class Agent
    {
        public string AGT_ID { get; set; }
        public string AGT_CODE { get; set; }
        public Int32 AGT_CODE_ID { get; set; }
        public Int32 AGT_TYPE_ID { get; set; }
        public Int32 AGT_MDRT_STATUS { get; set; }
        public Int32 AGT_MDRT_YEAR { get; set; }
        public string AGT_SYSTEM_ID { get; set; }
        public string AGT_SUB_CODE { get; set; }
        public Int32 AGT_LINE_OF_BUSINESS { get; set; }
        public string AGT_CHANNEL { get; set; }
        public Int32 AGT_LEVEL { get; set; }
        public Int32 AGT_LANGUAGE { get; set; }
        public string AGT_SUPER_CODE { get; set; }
        public string AGT_TITLE { get; set; }
        public string AGT_FIRST_NAME { get; set; }
        public string AGT_LAST_NAME { get; set; }
        public string AGT_ADD1 { get; set; }
        public string AGT_ADD2 { get; set; }
        public string AGT_ADD3 { get; set; }
        public string AGT_NIC_NO { get; set; }
        public string AGT_DOB { get; set; }
        public string AGT_MOBILE { get; set; }
        public string AGT_TEL_NO { get; set; }
        public string AGT_FAX_NO { get; set; }
        public string AGT_EMAIL { get; set; }


        public string AGT_BRANCH_CODE { get; set; }
        public Int32 AGT_BANK_ID { get; set; }
        public Int32 AGT_BANK_BRANCH_ID { get; set; }
        public string AGT_BANK_ACC_NO { get; set; }
        public int AGT_ID_ISSUED { get; set; }
        public string AGT_ID_ISSUED_DATE { get; set; }
        public string AGT_APPOINT_DATE { get; set; }
        public Int32 AGT_SLII_EXAM { get; set; }
        public string AGT_SLII_EXAM_DATE { get; set; }


        public string AGT_AGMT_DATE_RECEIVED  { get; set; }
        public string AGT_AGMT_DATE_ISSUED { get; set; }
        public string AGT_APP_DATE_RECEIVED { get; set; }
        public string AGT_APP_DATE_ISSUED { get; set; }


        public string AGT_TRNS_BRANCH_CODE { get; set; }
        public string AGT_TRNS_BRANCH_DATE { get; set; }
        public string AGT_STOP_COMM_DATE { get; set; }
        public string AGT_STOP_COMM_REASON { get; set; }
        public string AGT_RELEASE_COMM_DATE { get; set; }
        public string AGT_RELEASE_COMM_REASON { get; set; }
        public string AGT_CUSTOMER_COMPLAIN { get; set; }
        public string AGT_TERMINATE_NOTICE_DATE { get; set; }
        public string AGT_TERMINATE_DATE { get; set; }
        public string AGT_TERMINATE_REASON { get; set; }
        public string AGT_BLACK_LISTED_DATE { get; set; }
        public string AGT_DUES_TO_COMPANY { get; set; }
        public string AGT_REJOINED_DATE { get; set; }
        public string AGT_REMARKS { get; set; }


        public Int16 AGT_BUSINESS_TYPE { get; set; }
        public string AGT_LEVEL_CODE { get; set; }
        public string AGT_LEADER_CODE { get; set; }
        public Int32 AGT_STATUS { get; set; }
        public Int32 AGT_TERMINATE_STATUS { get; set; }
        public Int32 AGT_STOP_COMM_STATUS { get; set; }
        public Int32 AGT_ISS_STATUS { get; set; }
        public double AGT_ISS_AMOUNT { get; set; }
        public string AGT_ISS_GIVEN_DATE { get; set; }
        public string AGT_ISS_CLOSE_DATE { get; set; }
        public string AGT_RETAINER_STATUS { get; set; }
        public double AGT_RETAINER_AMOUNT { get; set; }
        public string AGT_RETAINER_GIVEN_DATE { get; set; }
        public string AGT_RETAINER_CLOSE_DATE { get; set; }
        public string AGT_FULL_NAME { get; set; }
        public string AGT_FULL_ADDRESS { get; set; }

        public string AGT_LEADER_AGENT_CODE_V { get; set; }
        public string AGT_LEADER_LEADER_CODE_V { get; set; }
        public string AGT_LEADER_AGENT_CODE_H { get; set; }
        public string AGT_LEADER_LEADER_CODE_H { get; set; }
        public string AGT_CREATED_BY { get; set; }
        public Int32 AGT_DESIGNATION_ID { get; set; }
        public Int32 AGT_HIERARCHY_TYPE_ID { get; set; }
        public Int32 AGT_CHANGE_REASON_ID { get; set; }
        public string AGT_EFFECTIVE_DATE { get; set; }
        public string AGT_CHANGE_REASON { get; set; }
        public string AGT_IS_AGENT_ATTACHED_CHANGED { get; set; }

        public string AGT_LEADER_GIVEN_DATE { get; set; }

    }
}
