using ComissionWebAPI.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.OracleClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;


namespace ComissionWebAPI.Controllers
{
    public class RefundController : ApiController
    {

        static string ConnectionString = ConfigurationManager.ConnectionStrings["OracleConString"].ToString();[HttpGet]

        //Receipt wise can contain A,B,C etc... end of the receipt number
        [HttpPost]
        [ActionName("GetRealisationRequiredRefundsByRefundID")]
        public IEnumerable<Refund> GetRealisationRequiredRefundsByRefundID(Int64 RefundID)
        {

            //RFD_STATUS 
            //1 - UPLOADED
            //2 - APPROVED
            //3 - REJECTED


            List<Refund> refundList = new List<Refund>();
            DataTable dataTable = new DataTable();
            OracleDataReader dataReader = null;
            OracleConnection connection = new OracleConnection(ConnectionString);
            OracleCommand command;

            string sql = "SELECT " +
            "CASE WHEN t.RFD_ID IS NULL THEN 0  ELSE t.RFD_ID END, " +
            "CASE WHEN t.RFD_RECEIPT_NO IS NULL THEN ''  ELSE t.RFD_RECEIPT_NO END, " +
            "CASE WHEN t.RFD_REFUND_DATE IS NULL THEN to_date('01/01/1900', 'DD/MM/RRRR')  ELSE to_date(t.RFD_REFUND_DATE, 'DD/MM/RRRR') END, " +
            "CASE WHEN t.RFD_TYPE IS NULL THEN 0  ELSE t.RFD_TYPE END, " +
            "CASE WHEN t.RFD_AMT IS NULL THEN 0  ELSE t.RFD_AMT END, " +
            "CASE WHEN t.RFD_PERCENTAGE IS NULL THEN 0  ELSE t.RFD_PERCENTAGE END, " +
            "CASE WHEN t.RFD_BY IS NULL THEN ''  ELSE t.RFD_BY END, " +
            "CASE WHEN t.RFD_REASON IS NULL THEN ''  ELSE t.RFD_REASON END, " +
            "CASE WHEN t.RFD_AGT_CODE IS NULL THEN ''  ELSE t.RFD_AGT_CODE END, " +
            "CASE WHEN t.RFD_PROCESS_IND IS NULL THEN ''  ELSE t.RFD_PROCESS_IND END, " +
            "CASE WHEN t.RFD_RV_NO IS NULL THEN ''  ELSE t.RFD_RV_NO END, " +
            "CASE WHEN t.RFD_PV_NO IS NULL THEN ''  ELSE t.RFD_PV_NO END, " +
            "CASE WHEN t.RFD_BAL_TYPE IS NULL THEN ''  ELSE t.RFD_BAL_TYPE END, " +
            "CASE WHEN t.RFD_CREATED_BY IS NULL THEN ''  ELSE t.RFD_CREATED_BY END, " +
            "CASE WHEN t.RFD_CREATED_DATE IS NULL THEN to_date('01/01/1900', 'DD/MM/RRRR')  ELSE to_date(t.RFD_CREATED_DATE, 'DD/MM/RRRR') END, " +
            "CASE WHEN t.RFD_EFFECTIVE_END_DATE IS NULL THEN to_date('01/01/1900', 'DD/MM/RRRR')  ELSE to_date(t.RFD_EFFECTIVE_END_DATE, 'DD/MM/RRRR') END, " +
            "t.RFD_STATUS, " +
            "t.RFD_POLICY_NO, " +
            "t.RFD_PROPOSAL_NO, " +
            "CASE WHEN t.RFD_REC_STATUS = 1 THEN 'TO BE APPROVED' WHEN t.RFD_REC_STATUS = 2 THEN 'APPROVED' WHEN t.RFD_REC_STATUS = 3 THEN 'REJECTED' END AS RFD_REC_STATUS, " +
            "t.RFD_REC_NARRATION, " +
            "t.RFD_REC_UPDATED_BY, " +
            "t.RFD_REC_UPDATED_DATE " +
            "FROM HCI_TBL_MAY_REFUND  T WHERE t.rfd_type = 1 and t.RFD_ID = '" + RefundID + "'";// WHERE T.RFD_STATUS = 1 ";

            command = new OracleCommand(sql, connection);
            try
            {
                connection.Open();
                dataReader = command.ExecuteReader();
                dataTable.Load(dataReader);
                dataReader.Close();
                connection.Close();
                refundList = (from DataRow drow in dataTable.Rows
                              select new Refund()
                              {
                                  RfdId = Convert.ToInt32(drow[0]),
                                  RfdReceiptNo = drow[1].ToString(),
                                  RfdRefundDate = drow[2].ToString(),
                                  RfdType = Convert.ToInt32(drow[3]),
                                  RfdAmt = Convert.ToInt32(drow[4]),
                                  RfdPercentage = Convert.ToInt32(drow[5]),
                                  RfdBy = drow[6].ToString(),
                                  RfdReason = drow[7].ToString(),
                                  RfdAgtCode = drow[8].ToString(),
                                  RfdProcessInd = drow[9].ToString(),
                                  RfdRvNo = drow[10].ToString(),
                                  RfdPvNo = drow[11].ToString(),
                                  RfdBalType = drow[12].ToString(),
                                  RfdCreatedBy = drow[13].ToString(),
                                  RfdCreatedDate = drow[14].ToString(),
                                  RfdEffectiveEndDate = drow[15].ToString(),
                                  RfdStatus = Convert.ToInt16(drow[16]),
                                  RfdPolicyNo = drow[17].ToString(),
                                  RfdProposalNo = drow[18].ToString(),
                                  RfdRecStatus = drow[19].ToString(),
                                  RfdRecNarration = drow[20].ToString(),
                                  RfdRecUpdatedBy = drow[21].ToString(),
                                  RfdRecUpdatedDate = drow[22].ToString()
                              }).ToList();
            }
            catch (Exception)
            {
                if (dataReader != null || connection.State == ConnectionState.Open)
                {
                    dataReader.Close();
                    connection.Close();
                }
            }
            return refundList;
        }



        [HttpPost]
        [ActionName("GetRealisationRequiredRefundsByProposalNo")]
        public IEnumerable<Refund> GetRealisationRequiredRefundsByProposalNo(string ProposalNo)
        {

            //RFD_STATUS 
            //1 - UPLOADED
            //2 - APPROVED
            //3 - REJECTED


            List<Refund> refundList = new List<Refund>();
            DataTable dataTable = new DataTable();
            OracleDataReader dataReader = null;
            OracleConnection connection = new OracleConnection(ConnectionString);
            OracleCommand command;

            string sql = "SELECT " +
            "CASE WHEN t.RFD_ID IS NULL THEN 0  ELSE t.RFD_ID END, " +
            "CASE WHEN t.RFD_RECEIPT_NO IS NULL THEN ''  ELSE t.RFD_RECEIPT_NO END, " +
            "CASE WHEN t.RFD_REFUND_DATE IS NULL THEN to_date('01/01/1900', 'DD/MM/RRRR')  ELSE to_date(t.RFD_REFUND_DATE, 'DD/MM/RRRR') END, " +
            "CASE WHEN t.RFD_TYPE IS NULL THEN 0  ELSE t.RFD_TYPE END, " +
            "CASE WHEN t.RFD_AMT IS NULL THEN 0  ELSE t.RFD_AMT END, " +
            "CASE WHEN t.RFD_PERCENTAGE IS NULL THEN 0  ELSE t.RFD_PERCENTAGE END, " +
            "CASE WHEN t.RFD_BY IS NULL THEN ''  ELSE t.RFD_BY END, " +
            "CASE WHEN t.RFD_REASON IS NULL THEN ''  ELSE t.RFD_REASON END, " +
            "CASE WHEN t.RFD_AGT_CODE IS NULL THEN ''  ELSE t.RFD_AGT_CODE END, " +
            "CASE WHEN t.RFD_PROCESS_IND IS NULL THEN ''  ELSE t.RFD_PROCESS_IND END, " +
            "CASE WHEN t.RFD_RV_NO IS NULL THEN ''  ELSE t.RFD_RV_NO END, " +
            "CASE WHEN t.RFD_PV_NO IS NULL THEN ''  ELSE t.RFD_PV_NO END, " +
            "CASE WHEN t.RFD_BAL_TYPE IS NULL THEN ''  ELSE t.RFD_BAL_TYPE END, " +
            "CASE WHEN t.RFD_CREATED_BY IS NULL THEN ''  ELSE t.RFD_CREATED_BY END, " +
            "CASE WHEN t.RFD_CREATED_DATE IS NULL THEN to_date('01/01/1900', 'DD/MM/RRRR')  ELSE to_date(t.RFD_CREATED_DATE, 'DD/MM/RRRR') END, " +
            "CASE WHEN t.RFD_EFFECTIVE_END_DATE IS NULL THEN to_date('01/01/1900', 'DD/MM/RRRR')  ELSE to_date(t.RFD_EFFECTIVE_END_DATE, 'DD/MM/RRRR') END, " +
            "t.RFD_STATUS, " +
            "t.RFD_POLICY_NO, " +
            "t.RFD_PROPOSAL_NO, " +
            "CASE WHEN t.RFD_REC_STATUS = 1 THEN 'TO BE APPROVED' WHEN t.RFD_REC_STATUS = 2 THEN 'APPROVED' WHEN t.RFD_REC_STATUS = 3 THEN 'REJECTED' END AS RFD_REC_STATUS, " +
            "t.RFD_REC_NARRATION, " +
            "t.RFD_REC_UPDATED_BY, " +
            "t.RFD_REC_UPDATED_DATE " +
            "FROM HCI_TBL_REFUNDS t WHERE t.rfd_type = 1 and t.RFD_PROPOSAL_NO = '" + ProposalNo + "' AND T.RFD_STATUS = 5 ";// WHERE T.RFD_STATUS = 1 ";

            command = new OracleCommand(sql, connection);
            try
            {
                connection.Open();
                dataReader = command.ExecuteReader();
                dataTable.Load(dataReader);
                dataReader.Close();
                connection.Close();
                refundList = (from DataRow drow in dataTable.Rows
                              select new Refund()
                              {
                                  RfdId = Convert.ToInt32(drow[0]),
                                  RfdReceiptNo = drow[1].ToString(),
                                  RfdRefundDate = drow[2].ToString(),
                                  RfdType = Convert.ToInt32(drow[3]),
                                  RfdAmt = Convert.ToInt32(drow[4]),
                                  RfdPercentage = Convert.ToInt32(drow[5]),
                                  RfdBy = drow[6].ToString(),
                                  RfdReason = drow[7].ToString(),
                                  RfdAgtCode = drow[8].ToString(),
                                  RfdProcessInd = drow[9].ToString(),
                                  RfdRvNo = drow[10].ToString(),
                                  RfdPvNo = drow[11].ToString(),
                                  RfdBalType = drow[12].ToString(),
                                  RfdCreatedBy = drow[13].ToString(),
                                  RfdCreatedDate = drow[14].ToString(),
                                  RfdEffectiveEndDate = drow[15].ToString(),
                                  RfdStatus = Convert.ToInt16(drow[16]),
                                  RfdPolicyNo = drow[17].ToString(),
                                  RfdProposalNo = drow[18].ToString(),
                                  RfdRecStatus = drow[19].ToString(),
                                  RfdRecNarration = drow[20].ToString(),
                                  RfdRecUpdatedBy = drow[21].ToString(),
                                  RfdRecUpdatedDate = drow[22].ToString()
                              }).ToList();
            }
            catch (Exception)
            {
                if (dataReader != null || connection.State == ConnectionState.Open)
                {
                    dataReader.Close();
                    connection.Close();
                }
            }
            return refundList;
        }


        [HttpGet]
        [ActionName("GetRealisationRequiredRefunds")]
        public IEnumerable<Refund> GetRealisationRequiredRefunds()
        {

            //RFD_STATUS 
            //1 - UPLOADED
            //2 - APPROVED
            //3 - REJECTED


            List<Refund> refundList = new List<Refund>();
            DataTable dataTable = new DataTable();
            OracleDataReader dataReader = null;
            OracleConnection connection = new OracleConnection(ConnectionString);
            OracleCommand command;

            string sql = "SELECT " +
            "CASE WHEN t.RFD_ID IS NULL THEN 0  ELSE t.RFD_ID END, " +
            "CASE WHEN t.RFD_RECEIPT_NO IS NULL THEN ''  ELSE t.RFD_RECEIPT_NO END, " +
            "CASE WHEN t.RFD_REFUND_DATE IS NULL THEN to_date('01/01/1900', 'DD/MM/RRRR')  ELSE to_date(t.RFD_REFUND_DATE, 'DD/MM/RRRR') END, " +
            "CASE WHEN t.RFD_TYPE IS NULL THEN 0  ELSE t.RFD_TYPE END, " +
            "CASE WHEN t.RFD_AMT IS NULL THEN 0  ELSE t.RFD_AMT END, " +
            "CASE WHEN t.RFD_PERCENTAGE IS NULL THEN 0  ELSE t.RFD_PERCENTAGE END, " +
            "CASE WHEN t.RFD_BY IS NULL THEN ''  ELSE t.RFD_BY END, " +
            "CASE WHEN t.RFD_REASON IS NULL THEN ''  ELSE t.RFD_REASON END, " +
            "CASE WHEN t.RFD_AGT_CODE IS NULL THEN ''  ELSE t.RFD_AGT_CODE END, " +
            "CASE WHEN t.RFD_PROCESS_IND IS NULL THEN ''  ELSE t.RFD_PROCESS_IND END, " +
            "CASE WHEN t.RFD_RV_NO IS NULL THEN ''  ELSE t.RFD_RV_NO END, " +
            "CASE WHEN t.RFD_PV_NO IS NULL THEN ''  ELSE t.RFD_PV_NO END, " +
            "CASE WHEN t.RFD_BAL_TYPE IS NULL THEN ''  ELSE t.RFD_BAL_TYPE END, " +
            "CASE WHEN t.RFD_CREATED_BY IS NULL THEN ''  ELSE t.RFD_CREATED_BY END, " +
            "CASE WHEN t.RFD_CREATED_DATE IS NULL THEN to_date('01/01/1900', 'DD/MM/RRRR')  ELSE to_date(t.RFD_CREATED_DATE, 'DD/MM/RRRR') END, " +
            "CASE WHEN t.RFD_EFFECTIVE_END_DATE IS NULL THEN to_date('01/01/1900', 'DD/MM/RRRR')  ELSE to_date(t.RFD_EFFECTIVE_END_DATE, 'DD/MM/RRRR') END, " +
            "t.RFD_STATUS, " +
            "t.RFD_POLICY_NO, " +
            "t.RFD_PROPOSAL_NO " +
            "FROM HCI_TBL_REFUND_TEMP t WHERE t.rfd_type = 1 and T.RFD_STATUS = 5 AND T.RFD_EFFECTIVE_END_DATE IS NULL ";
            //t.rfd_rec_status = 1";// WHERE T.RFD_STATUS = 1 ";

            command = new OracleCommand(sql, connection);
            try
            {
                connection.Open();
                dataReader = command.ExecuteReader();
                dataTable.Load(dataReader);
                dataReader.Close();
                connection.Close();
                refundList = (from DataRow drow in dataTable.Rows
                              select new Refund()
                              {
                                  RfdId = Convert.ToInt32(drow[0]),
                                  RfdReceiptNo = drow[1].ToString(),
                                  RfdRefundDate = drow[2].ToString(),
                                  RfdType = Convert.ToInt32(drow[3]),
                                  RfdAmt = Convert.ToInt32(drow[4]),
                                  RfdPercentage = Convert.ToInt32(drow[5]),
                                  RfdBy = drow[6].ToString(),
                                  RfdReason = drow[7].ToString(),
                                  RfdAgtCode = drow[8].ToString(),
                                  RfdProcessInd = drow[9].ToString(),
                                  RfdRvNo = drow[10].ToString(),
                                  RfdPvNo = drow[11].ToString(),
                                  RfdBalType = drow[12].ToString(),
                                  RfdCreatedBy = drow[13].ToString(),
                                  RfdCreatedDate = drow[14].ToString(),
                                  RfdEffectiveEndDate = drow[15].ToString(),
                                  RfdStatus = Convert.ToInt16(drow[16]),
                                  RfdPolicyNo = drow[17].ToString(),
                                  RfdProposalNo = drow[18].ToString()
                              }).ToList();
            }
            catch (Exception ex)
            {
                if (dataReader != null || connection.State == ConnectionState.Open)
                {
                    dataReader.Close();
                    connection.Close();
                }
            }
            return refundList;
        }



        [HttpGet]
        [ActionName("GetRealisationRequiredRefundsForReconciliation")]
        public IEnumerable<Refund> GetRealisationRequiredRefundsForReconciliation(Int64 RefundID)
        {

            //RFD_STATUS 
            //1 - UPLOADED
            //2 - APPROVED
            //3 - REJECTED


            List<Refund> refundList = new List<Refund>();
            DataTable dataTable = new DataTable();
            OracleDataReader dataReader = null;
            OracleConnection connection = new OracleConnection(ConnectionString);
            OracleCommand command;


            string sql = "SELECT  CASE WHEN t.RFD_ID IS NULL THEN 0  ELSE t.RFD_ID END, " +
                         "lpad(t.rfd_receipt_no, 13) as RFD_RECEIPT_NO,  " +
                         "CASE WHEN t.RFD_REFUND_DATE IS NULL THEN to_date('01/01/1900', 'DD/MM/RRRR')  ELSE to_date(t.RFD_REFUND_DATE, 'DD/MM/RRRR') END,  " +
                         "'1' as RFD_TYPE, " +
                         "sum(t.rfd_amt) as RFD_AMT,CASE WHEN t.RFD_PERCENTAGE IS NULL THEN 0  ELSE t.RFD_PERCENTAGE END , " +
                         "t.RFD_BY ," +
                         "t.RFD_REASON ,t.RFD_AGT_CODE , " +
                         "t.RFD_PROCESS_IND ,t.RFD_RV_NO,t.RFD_PV_NO , 'XXX' AS RFD_BAL_TYPE , t.RFD_CREATED_BY , CASE WHEN t.RFD_CREATED_DATE IS NULL THEN to_date('01/01/1900', 'DD/MM/RRRR')  ELSE to_date(t.RFD_CREATED_DATE, 'DD/MM/RRRR') END , " +
                         "CASE WHEN t.RFD_EFFECTIVE_END_DATE IS NULL THEN to_date('01/01/1900', 'DD/MM/RRRR')  ELSE to_date(t.RFD_EFFECTIVE_END_DATE, 'DD/MM/RRRR') END, t.RFD_STATUS , t.RFD_POLICY_NO , t.RFD_PROPOSAL_NO,PID.PID_PAYMENT_MTD " +
                         "FROM Hci_Tbl_May_Refund t ,  HCI_TBL_MAY_PID_ACC_01 PID " +
                         "WHERE T.RFD_EFFECTIVE_END_DATE IS NULL AND t.rfd_type = 1 and T.RFD_STATUS = 5 AND T.RFD_PAYMENT_TYPE = 'CHEQUE' AND t.RFD_ID = '" + RefundID + "'" +
                         "AND PID.PID_RECEIPT_NO = lpad(t.rfd_receipt_no, 13) " +
                         "GROUP BY t.rfd_id,lpad(t.rfd_receipt_no, 13),CASE WHEN t.RFD_REFUND_DATE IS NULL THEN to_date('01/01/1900', 'DD/MM/RRRR')  ELSE to_date(t.RFD_REFUND_DATE, 'DD/MM/RRRR') END,RFD_TYPE,CASE WHEN t.RFD_PERCENTAGE IS NULL THEN 0  ELSE t.RFD_PERCENTAGE END, " +
                         "T.RFD_BY,T.RFD_REASON ,  t.RFD_AGT_CODE,T.RFD_PROCESS_IND, " +
                         "T.RFD_CREATED_BY, " +
                         "CASE WHEN t.RFD_CREATED_DATE IS NULL THEN to_date('01/01/1900', 'DD/MM/RRRR')  ELSE to_date(t.RFD_CREATED_DATE, 'DD/MM/RRRR') END, T.RFD_EFFECTIVE_END_DATE , T.RFD_STATUS,T.RFD_POLICY_NO,T.RFD_PROPOSAL_NO " +
                         ",T.RFD_CREATED_BY,CASE WHEN t.RFD_CREATED_DATE IS NULL THEN to_date('01/01/1900', 'DD/MM/RRRR')  ELSE to_date(t.RFD_CREATED_DATE, 'DD/MM/RRRR') END, CASE WHEN t.RFD_EFFECTIVE_END_DATE IS NULL THEN to_date('01/01/1900', 'DD/MM/RRRR')  ELSE to_date(t.RFD_EFFECTIVE_END_DATE, 'DD/MM/RRRR') END, T.RFD_STATUS,T.RFD_POLICY_NO,T.RFD_PROPOSAL_NO ,t.RFD_RV_NO,t.RFD_PV_NO ,PID.PID_PAYMENT_MTD ";
            //t.rfd_rec_status = 1";// WHERE T.RFD_STATUS = 1 ";

            command = new OracleCommand(sql, connection);
            try
            {
                connection.Open();
                dataReader = command.ExecuteReader();
                dataTable.Load(dataReader);
                dataReader.Close();
                connection.Close();
                refundList = (from DataRow drow in dataTable.Rows
                              select new Refund()
                              {
                                  RfdId = Convert.ToInt32(drow[0]),
                                  RfdReceiptNo = drow[1].ToString(),
                                  RfdRefundDate = drow[2].ToString(),
                                  RfdType = Convert.ToInt32(drow[3]),
                                  RfdAmt = Convert.ToInt32(drow[4]),
                                  RfdPercentage = Convert.ToInt32(drow[5]),
                                  RfdBy = drow[6].ToString(),
                                  RfdReason = drow[7].ToString(),
                                  RfdAgtCode = drow[8].ToString(),
                                  RfdProcessInd = drow[9].ToString(),
                                  RfdRvNo = drow[10].ToString(),
                                  RfdPvNo = drow[11].ToString(),
                                  RfdBalType = drow[12].ToString(),
                                  RfdCreatedBy = drow[13].ToString(),
                                  RfdCreatedDate = drow[14].ToString(),
                                  RfdEffectiveEndDate = drow[15].ToString(),
                                  RfdStatus = Convert.ToInt16(drow[16]),
                                  RfdPolicyNo = drow[17].ToString(),
                                  RfdProposalNo = drow[18].ToString(),
                                  RfdPaymentMTD = drow[19].ToString()
                              }).ToList();
            }
            catch (Exception ex)
            {
                if (dataReader != null || connection.State == ConnectionState.Open)
                {
                    dataReader.Close();
                    connection.Close();
                }
            }
            return refundList;
        }



        public IEnumerable<Refund> GetNonConfirmedRefunds()
        {

            //RFD_STATUS 
            //1 - UPLOADED
            //2 - APPROVED
            //3 - REJECTED


            List<Refund> refundList = new List<Refund>();
            DataTable dataTable = new DataTable();
            OracleDataReader dataReader = null;
            OracleConnection connection = new OracleConnection(ConnectionString);
            OracleCommand command;
            string sql = "SELECT " +
            "CASE WHEN t.RFD_ID IS NULL THEN 0  ELSE t.RFD_ID END, " +
            "CASE WHEN t.RFD_RECEIPT_NO IS NULL THEN ''  ELSE t.RFD_RECEIPT_NO END, " +
            "CASE WHEN t.RFD_REFUND_DATE IS NULL THEN to_date('01/01/1900', 'DD/MM/RRRR')  ELSE to_date(t.RFD_REFUND_DATE, 'DD/MM/RRRR') END, " +
            "CASE WHEN t.RFD_TYPE IS NULL THEN 0  ELSE t.RFD_TYPE END, " +
            "CASE WHEN t.RFD_AMT IS NULL THEN 0  ELSE t.RFD_AMT END, " +
            "CASE WHEN t.RFD_PERCENTAGE IS NULL THEN 0  ELSE t.RFD_PERCENTAGE END, " +
            "CASE WHEN t.RFD_BY IS NULL THEN ''  ELSE t.RFD_BY END, " +
            "CASE WHEN t.RFD_REASON IS NULL THEN ''  ELSE t.RFD_REASON END, " +
            "CASE WHEN t.RFD_AGT_CODE IS NULL THEN ''  ELSE t.RFD_AGT_CODE END, " +
            "CASE WHEN t.RFD_PROCESS_IND IS NULL THEN ''  ELSE t.RFD_PROCESS_IND END, " +
            "CASE WHEN t.RFD_RV_NO IS NULL THEN ''  ELSE t.RFD_RV_NO END, " +
            "CASE WHEN t.RFD_PV_NO IS NULL THEN ''  ELSE t.RFD_PV_NO END, " +
            "CASE WHEN t.RFD_BAL_TYPE IS NULL THEN ''  ELSE t.RFD_BAL_TYPE END, " +
            "CASE WHEN t.RFD_CREATED_BY IS NULL THEN ''  ELSE t.RFD_CREATED_BY END, " +
            "CASE WHEN t.RFD_CREATED_DATE IS NULL THEN to_date('01/01/1900', 'DD/MM/RRRR')  ELSE to_date(t.RFD_CREATED_DATE, 'DD/MM/RRRR') END, " +
            "CASE WHEN t.RFD_EFFECTIVE_END_DATE IS NULL THEN to_date('01/01/1900', 'DD/MM/RRRR')  ELSE to_date(t.RFD_EFFECTIVE_END_DATE, 'DD/MM/RRRR') END, " +
            "t.RFD_STATUS " +
            "FROM HCI_TBL_REFUND_TEMP t WHERE T.RFD_STATUS = 1 ";
            command = new OracleCommand(sql, connection);
            try
            {
                connection.Open();
                dataReader = command.ExecuteReader();
                dataTable.Load(dataReader);
                dataReader.Close();
                connection.Close();
                refundList = (from DataRow drow in dataTable.Rows
                              select new Refund()
                              {
                                  RfdId = Convert.ToInt32(drow[0]),
                                  RfdReceiptNo = drow[1].ToString(),
                                  RfdRefundDate = drow[2].ToString(),
                                  RfdType = Convert.ToInt32(drow[3]),
                                  RfdAmt = Convert.ToInt32(drow[4]),
                                  RfdPercentage = Convert.ToInt32(drow[5]),
                                  RfdBy = drow[6].ToString(),
                                  RfdReason = drow[7].ToString(),
                                  RfdAgtCode = drow[8].ToString(),
                                  RfdProcessInd = drow[9].ToString(),
                                  RfdRvNo = drow[10].ToString(),
                                  RfdPvNo = drow[11].ToString(),
                                  RfdBalType = drow[12].ToString(),
                                  RfdCreatedBy = drow[13].ToString(),
                                  RfdCreatedDate = drow[14].ToString(),
                                  RfdEffectiveEndDate = drow[15].ToString(),
                                  RfdStatus = Convert.ToInt16(drow[16])
                              }).ToList();
            }
            catch (Exception exception)
            {
                if (dataReader != null || connection.State == ConnectionState.Open)
                {
                    dataReader.Close();
                    connection.Close();
                }
            }
            return refundList;
        }




        [HttpGet]
        [ActionName("GetSystemMappingFailedRefunds")]
        public IEnumerable<Refund> GetSystemMappingFailedRefunds()
        {

            //RFD_STATUS 
            //1 - UPLOADED
            //2 - APPROVED
            //3 - REJECTED


            List<Refund> refundList = new List<Refund>();
            DataTable dataTable = new DataTable();
            OracleDataReader dataReader = null;
            OracleConnection connection = new OracleConnection(ConnectionString);
            OracleCommand command;
            string sql = "SELECT " +
            "CASE WHEN t.RFD_ID IS NULL THEN 0  ELSE t.RFD_ID END, " +
            "CASE WHEN t.RFD_RECEIPT_NO IS NULL THEN ''  ELSE t.RFD_RECEIPT_NO END, " +
            "CASE WHEN t.RFD_REFUND_DATE IS NULL THEN to_date('01/01/1900', 'DD/MM/RRRR')  ELSE to_date(t.RFD_REFUND_DATE, 'DD/MM/RRRR') END, " +
            "CASE WHEN t.RFD_TYPE IS NULL THEN 0  ELSE t.RFD_TYPE END, " +
            "CASE WHEN t.RFD_AMT IS NULL THEN 0  ELSE t.RFD_AMT END, " +
            "CASE WHEN t.RFD_PERCENTAGE IS NULL THEN 0  ELSE t.RFD_PERCENTAGE END, " +
            "CASE WHEN t.RFD_BY IS NULL THEN ''  ELSE t.RFD_BY END, " +
            "CASE WHEN t.RFD_REASON IS NULL THEN ''  ELSE t.RFD_REASON END, " +
            "CASE WHEN t.RFD_AGT_CODE IS NULL THEN ''  ELSE t.RFD_AGT_CODE END, " +
            "CASE WHEN t.RFD_PROCESS_IND IS NULL THEN ''  ELSE t.RFD_PROCESS_IND END, " +
            "CASE WHEN t.RFD_RV_NO IS NULL THEN ''  ELSE t.RFD_RV_NO END, " +
            "CASE WHEN t.RFD_PV_NO IS NULL THEN ''  ELSE t.RFD_PV_NO END, " +
            "CASE WHEN t.RFD_BAL_TYPE IS NULL THEN ''  ELSE t.RFD_BAL_TYPE END, " +
            "CASE WHEN t.RFD_CREATED_BY IS NULL THEN ''  ELSE t.RFD_CREATED_BY END, " +
            "CASE WHEN t.RFD_CREATED_DATE IS NULL THEN to_date('01/01/1900', 'DD/MM/RRRR')  ELSE to_date(t.RFD_CREATED_DATE, 'DD/MM/RRRR') END, " +
            "CASE WHEN t.RFD_EFFECTIVE_END_DATE IS NULL THEN to_date('01/01/1900', 'DD/MM/RRRR')  ELSE to_date(t.RFD_EFFECTIVE_END_DATE, 'DD/MM/RRRR') END, " +
            "t.RFD_STATUS ," +
            "t.RFD_PROPOSAL_NO ," +
            "t.RFD_POLICY_NO " +
            "FROM HCI_TBL_REFUND_NOT t WHERE T.RFD_STATUS = 1 AND T.RFD_EFFECTIVE_END_DATE IS NULL ";
            command = new OracleCommand(sql, connection);
            try
            {
                connection.Open();
                dataReader = command.ExecuteReader();
                dataTable.Load(dataReader);
                dataReader.Close();
                connection.Close();
                refundList = (from DataRow drow in dataTable.Rows
                              select new Refund()
                              {
                                  RfdId = Convert.ToInt32(drow[0]),
                                  RfdReceiptNo = drow[1].ToString(),
                                  RfdRefundDate = drow[2].ToString(),
                                  RfdType = Convert.ToInt32(drow[3]),
                                  RfdAmt = Convert.ToInt32(drow[4]),
                                  RfdPercentage = Convert.ToInt32(drow[5]),
                                  RfdBy = drow[6].ToString(),
                                  RfdReason = drow[7].ToString(),
                                  RfdAgtCode = drow[8].ToString(),
                                  RfdProcessInd = drow[9].ToString(),
                                  RfdRvNo = drow[10].ToString(),
                                  RfdPvNo = drow[11].ToString(),
                                  RfdBalType = drow[12].ToString(),
                                  RfdCreatedBy = drow[13].ToString(),
                                  RfdCreatedDate = drow[14].ToString(),
                                  RfdEffectiveEndDate = drow[15].ToString(),
                                  RfdStatus = Convert.ToInt16(drow[16]),
                                  RfdProposalNo = drow[17].ToString(),
                                  RfdPolicyNo = drow[18].ToString()
                              }).ToList();
            }
            catch (Exception exception)
            {
                if (dataReader != null || connection.State == ConnectionState.Open)
                {
                    dataReader.Close();
                    connection.Close();
                }
            }
            return refundList;
        }



        public IEnumerable<Refund> Get()
        {
            List<Refund> refundList = new List<Refund>();
            DataTable dataTable = new DataTable();
            OracleDataReader dataReader = null;
            OracleConnection connection = new OracleConnection(ConnectionString);
            OracleCommand command;
            string sql = "SELECT " +
            "CASE WHEN t.RFD_ID IS NULL THEN 0  ELSE t.RFD_ID END, " +
            "CASE WHEN t.RFD_RECEIPT_NO IS NULL THEN ''  ELSE t.RFD_RECEIPT_NO END, " +
            "CASE WHEN t.RFD_REFUND_DATE IS NULL THEN to_date('01/01/1900', 'DD/MM/RRRR')  ELSE to_date(t.RFD_REFUND_DATE, 'DD/MM/RRRR') END, " +
            "CASE WHEN t.RFD_TYPE IS NULL THEN 0  ELSE t.RFD_TYPE END, " +
            "CASE WHEN t.RFD_AMT IS NULL THEN 0  ELSE t.RFD_AMT END, " +
            "CASE WHEN t.RFD_PERCENTAGE IS NULL THEN 0  ELSE t.RFD_PERCENTAGE END, " +
            "CASE WHEN t.RFD_BY IS NULL THEN ''  ELSE t.RFD_BY END, " +
            "CASE WHEN t.RFD_REASON IS NULL THEN ''  ELSE t.RFD_REASON END, " +
            "CASE WHEN t.RFD_AGT_CODE IS NULL THEN ''  ELSE t.RFD_AGT_CODE END, " +
            "CASE WHEN t.RFD_PROCESS_IND IS NULL THEN ''  ELSE t.RFD_PROCESS_IND END, " +
            "CASE WHEN t.RFD_RV_NO IS NULL THEN ''  ELSE t.RFD_RV_NO END, " +
            "CASE WHEN t.RFD_PV_NO IS NULL THEN ''  ELSE t.RFD_PV_NO END, " +
            "CASE WHEN t.RFD_BAL_TYPE IS NULL THEN ''  ELSE t.RFD_BAL_TYPE END, " +
            "CASE WHEN t.RFD_CREATED_BY IS NULL THEN ''  ELSE t.RFD_CREATED_BY END, " +
            "CASE WHEN t.RFD_CREATED_DATE IS NULL THEN to_date('01/01/1900', 'DD/MM/RRRR')  ELSE to_date(t.RFD_CREATED_DATE, 'DD/MM/RRRR') END, " +
            "CASE WHEN t.RFD_EFFECTIVE_END_DATE IS NULL THEN to_date('01/01/1900', 'DD/MM/RRRR')  ELSE to_date(t.RFD_EFFECTIVE_END_DATE, 'DD/MM/RRRR') END, " +
            "t.RFD_STATUS " +
            "FROM HCI_TBL_REFUNDS t ";
            command = new OracleCommand(sql, connection);
            try
            {
                connection.Open();
                dataReader = command.ExecuteReader();
                dataTable.Load(dataReader);
                dataReader.Close();
                connection.Close();
                refundList = (from DataRow drow in dataTable.Rows
                              select new Refund()
                              {
                                  RfdId = Convert.ToInt32(drow[0]),
                                  RfdReceiptNo = drow[1].ToString(),
                                  RfdRefundDate = drow[2].ToString(),
                                  RfdType = Convert.ToInt32(drow[3]),
                                  RfdAmt = Convert.ToInt32(drow[4]),
                                  RfdPercentage = Convert.ToInt32(drow[5]),
                                  RfdBy = drow[6].ToString(),
                                  RfdReason = drow[7].ToString(),
                                  RfdAgtCode = drow[8].ToString(),
                                  RfdProcessInd = drow[9].ToString(),
                                  RfdRvNo = drow[10].ToString(),
                                  RfdPvNo = drow[11].ToString(),
                                  RfdBalType = drow[12].ToString(),
                                  RfdCreatedBy = drow[13].ToString(),
                                  RfdCreatedDate = drow[14].ToString(),
                                  RfdEffectiveEndDate = drow[15].ToString(),
                                  RfdStatus = Convert.ToInt16(drow[16])
                              }).ToList();
            }
            catch (Exception exception)
            {
                if (dataReader != null || connection.State == ConnectionState.Open)
                {
                    //dataReader.Close();
                    connection.Close();
                }
            }
            return refundList;
        }


        [HttpGet]
        public Refund Get(int id)
        {
            Refund refund = new Refund();
            OracleDataReader dataReader = null;
            OracleConnection connection = new OracleConnection(ConnectionString);
            OracleCommand command;
            string sql = "SELECT " +
            "CASE WHEN t.RFD_ID IS NULL THEN 0  ELSE t.RFD_ID END, " +
            "CASE WHEN t.RFD_RECEIPT_NO IS NULL THEN ''  ELSE t.RFD_RECEIPT_NO END, " +
            "CASE WHEN t.RFD_REFUND_DATE IS NULL THEN to_date('01/01/1900', 'DD/MM/RRRR')  ELSE to_date(t.RFD_REFUND_DATE, 'DD/MM/RRRR') END, " +
            "CASE WHEN t.RFD_TYPE IS NULL THEN 0  ELSE t.RFD_TYPE END, " +
            "CASE WHEN t.RFD_AMT IS NULL THEN 0  ELSE t.RFD_AMT END, " +
            "CASE WHEN t.RFD_PERCENTAGE IS NULL THEN 0  ELSE t.RFD_PERCENTAGE END, " +
            "CASE WHEN t.RFD_BY IS NULL THEN ''  ELSE t.RFD_BY END, " +
            "CASE WHEN t.RFD_REASON IS NULL THEN ''  ELSE t.RFD_REASON END, " +
            "CASE WHEN t.RFD_AGT_CODE IS NULL THEN ''  ELSE t.RFD_AGT_CODE END, " +
            "CASE WHEN t.RFD_PROCESS_IND IS NULL THEN ''  ELSE t.RFD_PROCESS_IND END, " +
            "CASE WHEN t.RFD_RV_NO IS NULL THEN ''  ELSE t.RFD_RV_NO END, " +
            "CASE WHEN t.RFD_PV_NO IS NULL THEN ''  ELSE t.RFD_PV_NO END, " +
            "CASE WHEN t.RFD_BAL_TYPE IS NULL THEN ''  ELSE t.RFD_BAL_TYPE END, " +
            "CASE WHEN t.RFD_CREATED_BY IS NULL THEN ''  ELSE t.RFD_CREATED_BY END, " +
            "CASE WHEN t.RFD_CREATED_DATE IS NULL THEN to_date('01/01/1900', 'DD/MM/RRRR')  ELSE to_date(t.RFD_CREATED_DATE, 'DD/MM/RRRR') END, " +
            "CASE WHEN t.RFD_EFFECTIVE_END_DATE IS NULL THEN to_date('01/01/1900', 'DD/MM/RRRR')  ELSE to_date(t.RFD_EFFECTIVE_END_DATE, 'DD/MM/RRRR') END, " +
            "CASE WHEN t.RFD_CONFIRM IS NULL THEN ''  ELSE t.RFD_CONFIRM END " +
            "FROM HCI_TBL_REFUNDS t  WHERE t.RFD_ID=:V_RFD_ID";
            command = new OracleCommand(sql, connection);
            command.Parameters.Add(new OracleParameter("V_RFD_ID", id));
            connection.Open();
            try
            {
                dataReader = command.ExecuteReader();
                if (dataReader.HasRows)
                {
                    dataReader.Read();
                    refund.RfdId = Convert.ToInt32(dataReader[0]);
                    refund.RfdReceiptNo = dataReader[1].ToString();
                    refund.RfdRefundDate = dataReader[2].ToString();
                    refund.RfdType = Convert.ToInt32(dataReader[3]);
                    refund.RfdAmt = Convert.ToInt32(dataReader[4]);
                    refund.RfdPercentage = Convert.ToInt32(dataReader[5]);
                    refund.RfdBy = dataReader[6].ToString();
                    refund.RfdReason = dataReader[7].ToString();
                    refund.RfdAgtCode = dataReader[8].ToString();
                    refund.RfdProcessInd = dataReader[9].ToString();
                    refund.RfdRvNo = dataReader[10].ToString();
                    refund.RfdPvNo = dataReader[11].ToString();
                    refund.RfdBalType = dataReader[12].ToString();
                    refund.RfdCreatedBy = dataReader[13].ToString();
                    refund.RfdCreatedDate = dataReader[14].ToString();
                    refund.RfdEffectiveEndDate = dataReader[15].ToString();
                    refund.RfdStatus = Convert.ToInt16(dataReader[16]);

                    dataReader.Close();
                    connection.Close();
                }
                else
                {
                    return null;
                }
            }
            catch (Exception exception)
            {
                if (dataReader != null || connection.State == ConnectionState.Open)
                {
                    dataReader.Close();
                    connection.Close();
                }
            }
            finally
            {
                connection.Close();
            }
            return refund;
        }


        [HttpGet]
        public Refund GetRefundNOT(string RFD_ID)
        {
            Refund refund = new Refund();
            OracleDataReader dataReader = null;
            OracleConnection connection = new OracleConnection(ConnectionString);
            OracleCommand command;
            string sql = "SELECT " +
            "CASE WHEN t.RFD_ID IS NULL THEN 0  ELSE t.RFD_ID END, " +
            "CASE WHEN t.RFD_RECEIPT_NO IS NULL THEN ''  ELSE t.RFD_RECEIPT_NO END, " +
            "CASE WHEN t.RFD_REFUND_DATE IS NULL THEN to_date('01/01/1900', 'DD/MM/RRRR')  ELSE to_date(t.RFD_REFUND_DATE, 'DD/MM/RRRR') END, " +
            "CASE WHEN t.RFD_TYPE IS NULL THEN 0  ELSE t.RFD_TYPE END, " +
            "CASE WHEN t.RFD_AMT IS NULL THEN 0  ELSE t.RFD_AMT END, " +
            "CASE WHEN t.RFD_PERCENTAGE IS NULL THEN 0  ELSE t.RFD_PERCENTAGE END, " +
            "CASE WHEN t.RFD_BY IS NULL THEN ''  ELSE t.RFD_BY END, " +
            "CASE WHEN t.RFD_REASON IS NULL THEN ''  ELSE t.RFD_REASON END, " +
            "CASE WHEN t.RFD_AGT_CODE IS NULL THEN ''  ELSE t.RFD_AGT_CODE END, " +
            "CASE WHEN t.RFD_PROCESS_IND IS NULL THEN ''  ELSE t.RFD_PROCESS_IND END, " +
            "CASE WHEN t.RFD_RV_NO IS NULL THEN ''  ELSE t.RFD_RV_NO END, " +
            "CASE WHEN t.RFD_PV_NO IS NULL THEN ''  ELSE t.RFD_PV_NO END, " +
            "CASE WHEN t.RFD_BAL_TYPE IS NULL THEN ''  ELSE t.RFD_BAL_TYPE END, " +
            "CASE WHEN t.RFD_CREATED_BY IS NULL THEN ''  ELSE t.RFD_CREATED_BY END, " +
            "CASE WHEN t.RFD_CREATED_DATE IS NULL THEN to_date('01/01/1900', 'DD/MM/RRRR')  ELSE to_date(t.RFD_CREATED_DATE, 'DD/MM/RRRR') END, " +
            "CASE WHEN t.RFD_EFFECTIVE_END_DATE IS NULL THEN to_date('01/01/1900', 'DD/MM/RRRR')  ELSE to_date(t.RFD_EFFECTIVE_END_DATE, 'DD/MM/RRRR') END, " +
            "CASE WHEN t.RFD_STATUS IS NULL THEN 0  ELSE t.RFD_STATUS END , " +
            "t.RFD_PROPOSAL_NO ," +
            "t.RFD_POLICY_NO " +
            "FROM HCI_TBL_REFUND_NOT t " +
            "WHERE t.RFD_ID =:V_RFD_ID";

            command = new OracleCommand(sql, connection);
            command.Parameters.Add(new OracleParameter("V_RFD_ID", RFD_ID));
            connection.Open();
            try
            {
                dataReader = command.ExecuteReader();
                if (dataReader.HasRows)
                {
                    dataReader.Read();
                    refund.RfdId = Convert.ToInt32(dataReader[0]);
                    refund.RfdReceiptNo = dataReader[1].ToString();
                    refund.RfdRefundDate = dataReader[2].ToString();
                    refund.RfdType = Convert.ToInt32(dataReader[3]);
                    refund.RfdAmt = Convert.ToInt32(dataReader[4]);
                    refund.RfdPercentage = Convert.ToInt32(dataReader[5]);
                    refund.RfdBy = dataReader[6].ToString();
                    refund.RfdReason = dataReader[7].ToString();
                    refund.RfdAgtCode = dataReader[8].ToString();
                    refund.RfdProcessInd = dataReader[9].ToString();
                    refund.RfdRvNo = dataReader[10].ToString();
                    refund.RfdPvNo = dataReader[11].ToString();
                    refund.RfdBalType = dataReader[12].ToString();
                    refund.RfdCreatedBy = dataReader[13].ToString();
                    refund.RfdCreatedDate = dataReader[14].ToString();
                    refund.RfdEffectiveEndDate = dataReader[15].ToString();
                    refund.RfdStatus = Convert.ToInt16(dataReader[16]);
                    refund.RfdProposalNo = dataReader[17].ToString();
                    refund.RfdPolicyNo = dataReader[18].ToString();

                    dataReader.Close();
                    connection.Close();
                }
                else
                {
                    return null;
                }
            }
            catch (Exception exception)
            {
                if (dataReader != null || connection.State == ConnectionState.Open)
                {
                    dataReader.Close();
                    connection.Close();
                }
            }
            finally
            {
                connection.Close();
            }
            return refund;
        }




        [HttpPost]
        [ActionName("SaveRefundNOT")]
        public string SaveRefundNOT(Refund obj)
        {
            OracleConnection connection = new OracleConnection(ConnectionString);
            OracleCommand command;
            try
            {
                connection.Open();
                command = new OracleCommand("HCI_SP_REFUND_NOT_INSERT");
                command.CommandType = CommandType.StoredProcedure;
                command.Connection = connection;
                command.Parameters.Add("V_RFD_ID", OracleType.Number).Value = obj.RfdId;
                command.Parameters.Add("V_RFD_RECEIPT_NO", OracleType.VarChar).Value = obj.RfdReceiptNo;
                command.Parameters.Add("V_RFD_REFUND_DATE", OracleType.VarChar).Value = obj.RfdRefundDate;
                command.Parameters.Add("V_RFD_TYPE", OracleType.Double).Value = Convert.ToDouble(obj.RfdType);
                command.Parameters.Add("V_RFD_AMT", OracleType.Double).Value = Convert.ToDouble(obj.RfdAmt);
                command.Parameters.Add("V_RFD_PERCENTAGE", OracleType.Double).Value = Convert.ToDouble(obj.RfdPercentage);
                command.Parameters.Add("V_RFD_BY", OracleType.VarChar).Value = obj.RfdCreatedBy;// obj.RfdBy;
                command.Parameters.Add("V_RFD_REASON", OracleType.VarChar).Value = obj.RfdReason;
                command.Parameters.Add("V_RFD_AGT_CODE", OracleType.VarChar).Value = obj.RfdAgtCode;
                command.Parameters.Add("V_RFD_PROCESS_IND", OracleType.VarChar).Value = obj.RfdProcessInd;
                command.Parameters.Add("V_RFD_RV_NO", OracleType.VarChar).Value = obj.RfdRvNo;
                command.Parameters.Add("V_RFD_PV_NO", OracleType.VarChar).Value = obj.RfdPvNo;
                command.Parameters.Add("V_RFD_BAL_TYPE", OracleType.VarChar).Value = obj.RfdBalType;
                command.Parameters.Add("V_RFD_CREATED_BY", OracleType.VarChar).Value = obj.RfdCreatedBy;
                //command.Parameters.Add("V_RFD_CREATED_DATE", OracleType.DateTime).Value = obj.RfdCreatedDate;
                //command.Parameters.Add("V_RFD_EFFECTIVE_END_DATE", OracleType.DateTime).Value = obj.RfdEffectiveEndDate;
                command.Parameters.Add("V_RFD_STATUS", OracleType.VarChar).Value = obj.RfdStatus;
                command.Parameters.Add("V_RFD_PROPOSAL_NO", OracleType.VarChar).Value = obj.RfdProposalNo;
                command.Parameters.Add("V_RFD_POL_NO", OracleType.VarChar).Value = obj.RfdPolicyNo;
                command.Parameters.Add("V_RFD_CACELLATION_FEE", OracleType.Double).Value = Convert.ToDouble(obj.RfdCancellationFee);
                command.Parameters.Add("V_RFD_RECOVERY_FEE", OracleType.Double).Value = Convert.ToDouble(obj.RfdRecoveryFee);

                command.Parameters.Add("V_PV_NUMBER", OracleType.VarChar, 20).Direction = ParameterDirection.Output;

                command.ExecuteNonQuery();

                String V_PV_NUMBER = command.Parameters["V_PV_NUMBER"].Value.ToString();

                connection.Close();


                V_PV_NUMBER = V_PV_NUMBER + "|" + V_PV_NUMBER + "||";


                return V_PV_NUMBER + "Successfully Saved";//Request.CreateResponse(HttpStatusCode.OK);

            }
            catch (Exception exception)
            {
                connection.Close();
                return exception.Message;// return Request.CreateResponse(HttpStatusCode.ExpectationFailed);
            }
        }


        [HttpPost]
        [ActionName("SaveRefund")]
        public string SaveRefundl(Refund obj)
        {
            OracleConnection connection = new OracleConnection(ConnectionString);
            OracleCommand command;
            try
            {
                connection.Open();
                command = new OracleCommand("HCI_SP_REFUND_INSERT");
                command.CommandType = CommandType.StoredProcedure;
                command.Connection = connection;
                command.Parameters.Add("V_RFD_ID", OracleType.Number).Value = 0;
                command.Parameters.Add("V_RFD_RECEIPT_NO", OracleType.VarChar).Value = obj.RfdReceiptNo;
                command.Parameters.Add("V_RFD_REFUND_DATE", OracleType.VarChar).Value = obj.RfdRefundDate;
                command.Parameters.Add("V_RFD_TYPE", OracleType.Double).Value = Convert.ToDouble(obj.RfdType);
                command.Parameters.Add("V_RFD_AMT", OracleType.Double).Value = Convert.ToDouble(obj.RfdAmt);
                command.Parameters.Add("V_RFD_PERCENTAGE", OracleType.Double).Value = Convert.ToDouble(obj.RfdPercentage);
                command.Parameters.Add("V_RFD_BY", OracleType.VarChar).Value = obj.RfdCreatedBy;// obj.RfdBy;
                command.Parameters.Add("V_RFD_REASON", OracleType.VarChar).Value = obj.RfdReason;
                command.Parameters.Add("V_RFD_AGT_CODE", OracleType.VarChar).Value = obj.RfdAgtCode;
                command.Parameters.Add("V_RFD_PROCESS_IND", OracleType.VarChar).Value = obj.RfdProcessInd;
                command.Parameters.Add("V_RFD_RV_NO", OracleType.VarChar).Value = obj.RfdRvNo;
                command.Parameters.Add("V_RFD_PV_NO", OracleType.VarChar).Value = obj.RfdPvNo;
                command.Parameters.Add("V_RFD_BAL_TYPE", OracleType.VarChar).Value = obj.RfdBalType;
                command.Parameters.Add("V_RFD_CREATED_BY", OracleType.VarChar).Value = obj.RfdCreatedBy;
                //command.Parameters.Add("V_RFD_CREATED_DATE", OracleType.DateTime).Value = obj.RfdCreatedDate;
                //command.Parameters.Add("V_RFD_EFFECTIVE_END_DATE", OracleType.DateTime).Value = obj.RfdEffectiveEndDate;
                command.Parameters.Add("V_RFD_STATUS", OracleType.VarChar).Value = obj.RfdStatus;
                command.Parameters.Add("V_RFD_PROPOSAL_NO", OracleType.VarChar).Value = obj.RfdProposalNo;
                command.Parameters.Add("V_RFD_POL_NO", OracleType.VarChar).Value = obj.RfdPolicyNo;
                command.Parameters.Add("V_RFD_CACELLATION_FEE", OracleType.Double).Value = Convert.ToDouble(obj.RfdCancellationFee);
                command.Parameters.Add("V_RFD_RECOVERY_FEE", OracleType.Double).Value = Convert.ToDouble(obj.RfdRecoveryFee);

                command.Parameters.Add("V_PV_NUMBER", OracleType.VarChar, 20).Direction = ParameterDirection.Output;

                command.ExecuteNonQuery();

                String V_PV_NUMBER = command.Parameters["V_PV_NUMBER"].Value.ToString();

                connection.Close();


                V_PV_NUMBER = V_PV_NUMBER + "|" + V_PV_NUMBER + "||";


                return V_PV_NUMBER + "Successfully Saved";//Request.CreateResponse(HttpStatusCode.OK);

            }
            catch (Exception exception)
            {
                connection.Close();
                return exception.Message;// return Request.CreateResponse(HttpStatusCode.ExpectationFailed);
            }
        }




        [HttpPost]
        [ActionName("RefundStatusChange")]
        public HttpResponseMessage RefundStatusChange(Refund obj)
        {
            OracleConnection connection1 = new OracleConnection(ConnectionString);
            OracleCommand command1;
            try
            {
                connection1.Open();
                command1 = new OracleCommand("HCI_SP_REFUND_STATUS_CHANGE");
                command1.CommandType = CommandType.StoredProcedure;
                command1.Connection = connection1;

                command1.Parameters.Add("V_RFD_ID", OracleType.Number).Value = obj.RfdId;
                command1.Parameters.Add("V_RFD_RECEIPT_NO", OracleType.VarChar).Value = obj.RfdReceiptNo;
                command1.Parameters.Add("V_RFD_UPDATED_BY", OracleType.VarChar).Value = obj.RfdCreatedBy;
                command1.Parameters.Add("V_RFD_STATUS", OracleType.VarChar).Value = obj.RfdStatus;

                command1.ExecuteNonQuery();
                connection1.Close();
                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception exception)
            {
                connection1.Close();
                return Request.CreateResponse(HttpStatusCode.ExpectationFailed);
            }
        }



        [HttpPost]
        [ActionName("RefundStatusChangeBulk")]
        public HttpResponseMessage RefundStatusChangeBulk(Refund obj)
        {
            OracleConnection connection1 = new OracleConnection(ConnectionString);
            OracleCommand command1;
            try
            {
                connection1.Open();
                command1 = new OracleCommand("HCI_SP_REFUND_STATUS_CHANGE_BULK");
                command1.CommandType = CommandType.StoredProcedure;
                command1.Connection = connection1;

                command1.Parameters.Add("V_RFD_ID", OracleType.Number).Value = obj.RfdId;
                command1.Parameters.Add("V_RFD_RECEIPT_NO", OracleType.VarChar).Value = obj.RfdReceiptNo;
                command1.Parameters.Add("V_RFD_PROPOSAL_NO", OracleType.VarChar).Value = obj.RfdProposalNo;
                command1.Parameters.Add("V_RFD_UPDATED_BY", OracleType.VarChar).Value = obj.RfdCreatedBy;
                command1.Parameters.Add("V_RFD_STATUS", OracleType.VarChar).Value = obj.RfdStatus;

                command1.ExecuteNonQuery();
                connection1.Close();
                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception exception)
            {
                connection1.Close();
                return Request.CreateResponse(HttpStatusCode.ExpectationFailed);
            }
        }



        [HttpPost]
        [ActionName("UpdateRecStatus")]
        public HttpResponseMessage UpdateRecStatus(string[] args)
        {

            string TempReceiptNo = "";
            string TempRefundStatus = "";
            string TempRefundID = "";
            string TempUserID = "";

            OracleConnection connection1 = new OracleConnection(ConnectionString);
            OracleCommand command1;

            foreach (string item in args)
            {


                TempReceiptNo = item.Substring(item.IndexOf("Parameter1:") + 11, item.IndexOf("Parameter2:") - 11);

                TempRefundStatus = item.Substring(item.IndexOf("Parameter2:") + 11, item.IndexOf("Parameter3:") - (item.IndexOf("Parameter2:") + 11));

                TempRefundID = item.Substring(item.IndexOf("Parameter3:") + 11, item.IndexOf("Parameter4:") - (item.IndexOf("Parameter3:") + 11));

                TempUserID = item.Substring(item.IndexOf("Parameter4:") + 11, item.IndexOf("Parameter5:") - (item.IndexOf("Parameter4:") + 11));



                try
                {


                    connection1.Open();
                    command1 = new OracleCommand("HCI_SP_REC_STATUS_UPDATE");
                    command1.CommandType = CommandType.StoredProcedure;
                    command1.Connection = connection1;

                    command1.Parameters.Add("V_RFD_ID", OracleType.Number).Value = TempRefundID;
                    command1.Parameters.Add("V_RFD_RECEIPT_NO", OracleType.VarChar).Value = TempReceiptNo;// obj.RfdReceiptNo;
                    command1.Parameters.Add("V_RFD_RECEIPT_AMT", OracleType.VarChar).Value = 0;
                    command1.Parameters.Add("V_RFD_PROPOSAL_NO", OracleType.VarChar).Value = "";
                    command1.Parameters.Add("V_RFD_UPDATED_BY", OracleType.VarChar).Value = TempUserID;
                    command1.Parameters.Add("V_RFD_REC_NARRATION", OracleType.VarChar).Value = "";
                    command1.Parameters.Add("V_RFD_STATUS", OracleType.Number).Value = Convert.ToInt64(TempRefundStatus);


                    command1.ExecuteNonQuery();
                    connection1.Close();


                }
                catch (Exception ex)
                {
                    connection1.Close();
                    return Request.CreateResponse(HttpStatusCode.ExpectationFailed);
                }

            }


            //Batch execute
            connection1.Open();
            command1 = new OracleCommand("HCI_SP_REFUND_APPROVAL_REJECT");
            command1.CommandType = CommandType.StoredProcedure;
            command1.Connection = connection1;
            command1.Parameters.Add("REFUND_ID", OracleType.Number).Value = TempRefundID;
            command1.ExecuteNonQuery();
            connection1.Close();


            return Request.CreateResponse(HttpStatusCode.OK);

        }





        //[HttpPost]
        //[ActionName("UpdateRecStatusTEST")]
        //public HttpResponseMessage UpdateRecStatusTEST(Refund obj)
        //{
        //    OracleConnection connection1 = new OracleConnection(ConnectionString);
        //    OracleCommand command1;
        //    try
        //    {


        //        if (obj.RfdReceiptNo == "")
        //        {
        //            return Request.CreateResponse(HttpStatusCode.OK);
        //        }


        //        string[] ids = { obj.RfdReceiptNo };
        //        string ReceiptNoList = String.Join(",", ids);
        //        ReceiptNoList = ReceiptNoList.Substring(0, ReceiptNoList.Length - 1);//'18HDO00059466','18HDO00065976','18HDO00065972'


        //        ReceiptNoList = ReceiptNoList.Replace("'", "");


        //        connection1.Open();
        //        command1 = new OracleCommand("HCI_SP_REC_STATUS_UPDATE");
        //        command1.CommandType = CommandType.StoredProcedure;
        //        command1.Connection = connection1;

        //        command1.Parameters.Add("V_RFD_ID", OracleType.Number).Value = obj.RfdId;
        //        command1.Parameters.Add("V_RFD_RECEIPT_NO", OracleType.VarChar).Value = ReceiptNoList;// obj.RfdReceiptNo;
        //        command1.Parameters.Add("V_RFD_RECEIPT_AMT", OracleType.VarChar).Value = obj.RfdAmt;
        //        command1.Parameters.Add("V_RFD_PROPOSAL_NO", OracleType.VarChar).Value = obj.RfdProposalNo;
        //        command1.Parameters.Add("V_RFD_UPDATED_BY", OracleType.VarChar).Value = obj.RfdRecUpdatedBy;
        //        command1.Parameters.Add("V_RFD_REC_NARRATION", OracleType.VarChar).Value = obj.RfdRecNarration;
        //        command1.Parameters.Add("V_RFD_STATUS", OracleType.VarChar).Value = obj.RfdStatus;


        //        command1.ExecuteNonQuery();
        //        connection1.Close();
        //        return Request.CreateResponse(HttpStatusCode.OK);
        //    }
        //    catch (Exception exception)
        //    {
        //        connection1.Close();
        //        return Request.CreateResponse(HttpStatusCode.ExpectationFailed);
        //    }
        //}


    }
}
