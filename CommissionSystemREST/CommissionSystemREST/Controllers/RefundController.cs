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
            "FROM HCI_TBL_REFUNDS t WHERE t.rfd_type = 1 and t.RFD_PROPOSAL_NO = '" + ProposalNo  + "'";// WHERE T.RFD_STATUS = 1 ";

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
            "FROM HCI_TBL_REFUNDS_TEST t WHERE t.rfd_type = 1 and t.rfd_rec_status = 1";// WHERE T.RFD_STATUS = 1 ";

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
        [ActionName("GetNonConfirmedRefunds")]
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
            " FROM HCI_TBL_REFUNDS t  WHERE t.RFD_ID=:V_RFD_ID";
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
        public HttpResponseMessage UpdateRecStatus(Refund obj)
        {
            OracleConnection connection1 = new OracleConnection(ConnectionString);
            OracleCommand command1;
            try
            {
                connection1.Open();
                command1 = new OracleCommand("HCI_SP_REC_STATUS_UPDATE");
                command1.CommandType = CommandType.StoredProcedure;
                command1.Connection = connection1;

                command1.Parameters.Add("V_RFD_ID", OracleType.Number).Value = obj.RfdId;
                command1.Parameters.Add("V_RFD_RECEIPT_NO", OracleType.VarChar).Value = obj.RfdReceiptNo;
                command1.Parameters.Add("V_RFD_RECEIPT_AMT", OracleType.VarChar).Value = obj.RfdReceiptNo;
                command1.Parameters.Add("V_RFD_PROPOSAL_NO", OracleType.VarChar).Value = obj.RfdProposalNo;
                command1.Parameters.Add("V_RFD_UPDATED_BY", OracleType.VarChar).Value = obj.RfdRecUpdatedBy;
                command1.Parameters.Add("V_RFD_REC_NARRATION", OracleType.VarChar).Value = obj.RfdRecNarration;
                command1.Parameters.Add("V_RFD_REC_STATUS", OracleType.VarChar).Value = obj.RfdRecStatus;

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


    }
}
