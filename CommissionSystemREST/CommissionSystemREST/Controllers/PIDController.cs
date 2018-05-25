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
    public class PIDController : ApiController
    {

        static string ConnectionString = ConfigurationManager.ConnectionStrings["OracleConString"].ToString();[HttpGet]

        public IEnumerable<PID> Get()
        {
            List<PID> pIDList = new List<PID>();
            DataTable dataTable = new DataTable();
            OracleDataReader dataReader = null;
            OracleConnection connection = new OracleConnection(ConnectionString);
            OracleCommand command;
            string sql = "SELECT " +
            "CASE WHEN t.PID_RECEIPT_NO IS NULL THEN ''  ELSE t.PID_RECEIPT_NO END, " +
            "CASE WHEN t.PID_RECEIPT_DATE IS NULL THEN to_date('01/01/1900', 'DD/MM/RRRR')  ELSE to_date(t.PID_RECEIPT_DATE, 'DD/MM/RRRR') END, " +
            "CASE WHEN t.PID_CUSTOMER_NAME IS NULL THEN ''  ELSE t.PID_CUSTOMER_NAME END, " +
            "CASE WHEN t.PID_PROPOSAL_NO IS NULL THEN ''  ELSE t.PID_PROPOSAL_NO END, " +
            "CASE WHEN t.PID_POLICY_NO IS NULL THEN ''  ELSE t.PID_POLICY_NO END, " +
            "CASE WHEN t.PID_RECEIPT_AMT IS NULL THEN 0  ELSE t.PID_RECEIPT_AMT END, " +
            "CASE WHEN t.PID_PAYMENT_MTD IS NULL THEN ''  ELSE t.PID_PAYMENT_MTD END, " +
            "CASE WHEN t.PID_BRANCH IS NULL THEN ''  ELSE t.PID_BRANCH END, " +
            "CASE WHEN t.PID_BANK_CODE IS NULL THEN ''  ELSE t.PID_BANK_CODE END, " +
            "CASE WHEN t.PID_AGT_CODE IS NULL THEN ''  ELSE t.PID_AGT_CODE END, " +
            "CASE WHEN t.PID_TIME_SLAB IS NULL THEN ''  ELSE t.PID_TIME_SLAB END, " +
            "CASE WHEN t.PID_TERM IS NULL THEN 0  ELSE t.PID_TERM END, " +
            "CASE WHEN t.PID_INSTALLMENT_AMT IS NULL THEN 0  ELSE t.PID_INSTALLMENT_AMT END, " +
            "CASE WHEN t.PID_TABLE IS NULL THEN 0  ELSE t.PID_TABLE END, " +
            "CASE WHEN t.PID_PAY_FREQ IS NULL THEN ''  ELSE t.PID_PAY_FREQ END, " +
            "CASE WHEN t.PID_CONFIRM_IND IS NULL THEN ''  ELSE t.PID_CONFIRM_IND END, " +
            "CASE WHEN t.PID_CONFIRM_AMT IS NULL THEN 0  ELSE t.PID_CONFIRM_AMT END, " +
            "CASE WHEN t.PID_REFUND_IND IS NULL THEN ''  ELSE t.PID_REFUND_IND END, " +
            "CASE WHEN t.PID_AVAILABLE_AMT IS NULL THEN 0  ELSE t.PID_AVAILABLE_AMT END, " +
            "CASE WHEN t.PID_REVERSE_IND IS NULL THEN ''  ELSE t.PID_REVERSE_IND END, " +
            "CASE WHEN t.PID_COMM_CAL_IND IS NULL THEN ''  ELSE t.PID_COMM_CAL_IND END, " +
            "CASE WHEN t.PID_CONFIRM_BY IS NULL THEN ''  ELSE t.PID_CONFIRM_BY END, " +
            "CASE WHEN t.PID_CONFIRM_DATE IS NULL THEN to_date('01/01/1900', 'DD/MM/RRRR')  ELSE to_date(t.PID_CONFIRM_DATE, 'DD/MM/RRRR') END, " +
            "CASE WHEN t.PID_LAST_MODIFIED_BY IS NULL THEN ''  ELSE t.PID_LAST_MODIFIED_BY END, " +
            "CASE WHEN t.PID_LAST_MODIFIED_DATE IS NULL THEN to_date('01/01/1900', 'DD/MM/RRRR')  ELSE to_date(t.PID_LAST_MODIFIED_DATE, 'DD/MM/RRRR') END, " +
            "CASE WHEN t.PID_LAST_MODIFIED_RESON IS NULL THEN ''  ELSE t.PID_LAST_MODIFIED_RESON END, " +
            "CASE WHEN t.PID_IS_HELD_RECEIPT IS NULL THEN ''  ELSE t.PID_IS_HELD_RECEIPT END, " +
            "CASE WHEN t.PID_RECEIPT_TYPE IS NULL THEN ''  ELSE t.PID_RECEIPT_TYPE END, " +
            "CASE WHEN t.PID_TEMP_FIELD_1 IS NULL THEN ''  ELSE t.PID_TEMP_FIELD_1 END, " +
            "CASE WHEN t.PID_TEMP_FIELD_2 IS NULL THEN 0  ELSE t.PID_TEMP_FIELD_2 END, " +
            "CASE WHEN t.PID_TEMP_FIELD_3 IS NULL THEN ''  ELSE t.PID_TEMP_FIELD_3 END, " +
            "CASE WHEN t.PID_CHEQUE_RET_IND IS NULL THEN ''  ELSE t.PID_CHEQUE_RET_IND END, " +
            "CASE WHEN t.PID_PRODUCT_CODE IS NULL THEN ''  ELSE t.PID_PRODUCT_CODE END, " +
            "CASE WHEN t.PID_POLICY_BANK IS NULL THEN ''  ELSE t.PID_POLICY_BANK END, " +
            "CASE WHEN t.PID_POLICY_YEAR IS NULL THEN 0  ELSE t.PID_POLICY_YEAR END, " +
            "CASE WHEN t.PID_UPLOAD_DATE IS NULL THEN to_date('01/01/1900', 'DD/MM/RRRR')  ELSE to_date(t.PID_UPLOAD_DATE, 'DD/MM/RRRR') END, " +
            "CASE WHEN t.PID_RV_NO IS NULL THEN ''  ELSE t.PID_RV_NO END, " +
            "CASE WHEN t.PID_POLICY_FEE IS NULL THEN 0  ELSE t.PID_POLICY_FEE END, " +
            "CASE WHEN t.PID_TOTAL_AMT IS NULL THEN 0  ELSE t.PID_TOTAL_AMT END, " +
            "CASE WHEN t.PID_SYS_MAN IS NULL THEN ''  ELSE t.PID_SYS_MAN END, " +
            "CASE WHEN t.PID_BAL_TYPE IS NULL THEN ''  ELSE t.PID_BAL_TYPE END, " +
            "CASE WHEN t.PID_ACT_CONFIRM_DATE IS NULL THEN to_date('01/01/1900', 'DD/MM/RRRR')  ELSE to_date(t.PID_ACT_CONFIRM_DATE, 'DD/MM/RRRR') END, " +
            "CASE WHEN t.PID_FMT_OPBAL IS NULL THEN 0  ELSE t.PID_FMT_OPBAL END, " +
            "CASE WHEN t.PID_NEXT_OPBAL IS NULL THEN ''  ELSE t.PID_NEXT_OPBAL END " +
            " FROM HCI_TBL_PID t ";
            command = new OracleCommand(sql, connection);
            try
            {
                connection.Open();
                dataReader = command.ExecuteReader();
                dataTable.Load(dataReader);
                dataReader.Close();
                connection.Close();
                pIDList = (from DataRow drow in dataTable.Rows
                           select new PID()
                           {
                               PidReceiptNo = drow[0].ToString(),
                               PidReceiptDate = drow[1].ToString(),
                               PidCustomerName = drow[2].ToString(),
                               PidProposalNo = drow[3].ToString(),
                               PidPolicyNo = drow[4].ToString(),
                               PidReceiptAmt = Convert.ToInt32(drow[5]),
                               PidPaymentMtd = drow[6].ToString(),
                               PidBranch = drow[7].ToString(),
                               PidBankCode = drow[8].ToString(),
                               PidAgtCode = drow[9].ToString(),
                               PidTimeSlab = drow[10].ToString(),
                               PidTerm = Convert.ToInt32(drow[11]),
                               PidInstallmentAmt = Convert.ToInt32(drow[12]),
                               PidTable = Convert.ToInt32(drow[13]),
                               PidPayFreq = drow[14].ToString(),
                               PidConfirmInd = drow[15].ToString(),
                               PidConfirmAmt = Convert.ToInt32(drow[16]),
                               PidRefundInd = drow[17].ToString(),
                               PidAvailableAmt = Convert.ToInt32(drow[18]),
                               PidReverseInd = drow[19].ToString(),
                               PidCommCalInd = drow[20].ToString(),
                               PidConfirmBy = drow[21].ToString(),
                               PidConfirmDate = drow[22].ToString(),
                               PidLastModifiedBy = drow[23].ToString(),
                               PidLastModifiedDate = drow[24].ToString(),
                               PidLastModifiedReson = drow[25].ToString(),
                               PidIsHeldReceipt = drow[26].ToString(),
                               PidReceiptType = drow[27].ToString(),
                               PidTempField1 = drow[28].ToString(),
                               PidTempField2 = Convert.ToInt32(drow[29]),
                               PidTempField3 = drow[30].ToString(),
                               PidChequeRetInd = drow[31].ToString(),
                               PidProductCode = drow[32].ToString(),
                               PidPolicyBank = drow[33].ToString(),
                               PidPolicyYear = Convert.ToInt32(drow[34]),
                               PidUploadDate = drow[35].ToString(),
                               PidRvNo = drow[36].ToString(),
                               PidPolicyFee = Convert.ToInt32(drow[37]),
                               PidTotalAmt = Convert.ToInt32(drow[38]),
                               PidSysMan = drow[39].ToString(),
                               PidBalType = drow[40].ToString(),
                               PidActConfirmDate = drow[41].ToString(),
                               PidFmtOpbal = Convert.ToInt32(drow[42]),
                               PidNextOpbal = drow[43].ToString()
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
            return pIDList;
        }

        [HttpGet]
        public PID Get(string id)
        {
            PID pID = new PID();
            OracleDataReader dataReader = null;
            OracleConnection connection = new OracleConnection(ConnectionString);
            OracleCommand command;
            string sql = "SELECT " +
            "CASE WHEN t.PID_RECEIPT_NO IS NULL THEN ''  ELSE t.PID_RECEIPT_NO END, " +
            "CASE WHEN t.PID_RECEIPT_DATE IS NULL THEN to_date('01/01/1900', 'DD/MM/RRRR')  ELSE to_date(t.PID_RECEIPT_DATE, 'DD/MM/RRRR') END, " +
            "CASE WHEN t.PID_CUSTOMER_NAME IS NULL THEN ''  ELSE t.PID_CUSTOMER_NAME END, " +
            "CASE WHEN t.PID_PROPOSAL_NO IS NULL THEN ''  ELSE t.PID_PROPOSAL_NO END, " +
            "CASE WHEN t.PID_POLICY_NO IS NULL THEN ''  ELSE t.PID_POLICY_NO END, " +
            "CASE WHEN t.PID_RECEIPT_AMT IS NULL THEN 0  ELSE t.PID_RECEIPT_AMT END, " +
            "CASE WHEN t.PID_PAYMENT_MTD IS NULL THEN ''  ELSE t.PID_PAYMENT_MTD END, " +
            "CASE WHEN t.PID_BRANCH IS NULL THEN ''  ELSE t.PID_BRANCH END, " +
            "CASE WHEN t.PID_BANK_CODE IS NULL THEN ''  ELSE t.PID_BANK_CODE END, " +
            "CASE WHEN t.PID_AGT_CODE IS NULL THEN ''  ELSE t.PID_AGT_CODE END, " +
            "CASE WHEN t.PID_TIME_SLAB IS NULL THEN ''  ELSE t.PID_TIME_SLAB END, " +
            "CASE WHEN t.PID_TERM IS NULL THEN 0  ELSE t.PID_TERM END, " +
            "CASE WHEN t.PID_INSTALLMENT_AMT IS NULL THEN 0  ELSE t.PID_INSTALLMENT_AMT END, " +
            "CASE WHEN t.PID_TABLE IS NULL THEN 0  ELSE t.PID_TABLE END, " +
            "CASE WHEN t.PID_PAY_FREQ IS NULL THEN ''  ELSE t.PID_PAY_FREQ END, " +
            "CASE WHEN t.PID_CONFIRM_IND IS NULL THEN ''  ELSE t.PID_CONFIRM_IND END, " +
            "CASE WHEN t.PID_CONFIRM_AMT IS NULL THEN 0  ELSE t.PID_CONFIRM_AMT END, " +
            "CASE WHEN t.PID_REFUND_IND IS NULL THEN ''  ELSE t.PID_REFUND_IND END, " +
            "CASE WHEN t.PID_AVAILABLE_AMT IS NULL THEN 0  ELSE t.PID_AVAILABLE_AMT END, " +
            "CASE WHEN t.PID_REVERSE_IND IS NULL THEN ''  ELSE t.PID_REVERSE_IND END, " +
            "CASE WHEN t.PID_COMM_CAL_IND IS NULL THEN ''  ELSE t.PID_COMM_CAL_IND END, " +
            "CASE WHEN t.PID_CONFIRM_BY IS NULL THEN ''  ELSE t.PID_CONFIRM_BY END, " +
            "CASE WHEN t.PID_CONFIRM_DATE IS NULL THEN to_date('01/01/1900', 'DD/MM/RRRR')  ELSE to_date(t.PID_CONFIRM_DATE, 'DD/MM/RRRR') END, " +
            "CASE WHEN t.PID_LAST_MODIFIED_BY IS NULL THEN ''  ELSE t.PID_LAST_MODIFIED_BY END, " +
            "CASE WHEN t.PID_LAST_MODIFIED_DATE IS NULL THEN to_date('01/01/1900', 'DD/MM/RRRR')  ELSE to_date(t.PID_LAST_MODIFIED_DATE, 'DD/MM/RRRR') END, " +
            "CASE WHEN t.PID_LAST_MODIFIED_RESON IS NULL THEN ''  ELSE t.PID_LAST_MODIFIED_RESON END, " +
            "CASE WHEN t.PID_IS_HELD_RECEIPT IS NULL THEN ''  ELSE t.PID_IS_HELD_RECEIPT END, " +
            "CASE WHEN t.PID_RECEIPT_TYPE IS NULL THEN ''  ELSE t.PID_RECEIPT_TYPE END, " +
            "CASE WHEN t.PID_TEMP_FIELD_1 IS NULL THEN ''  ELSE t.PID_TEMP_FIELD_1 END, " +
            "CASE WHEN t.PID_TEMP_FIELD_2 IS NULL THEN 0  ELSE t.PID_TEMP_FIELD_2 END, " +
            "CASE WHEN t.PID_TEMP_FIELD_3 IS NULL THEN ''  ELSE t.PID_TEMP_FIELD_3 END, " +
            "CASE WHEN t.PID_CHEQUE_RET_IND IS NULL THEN ''  ELSE t.PID_CHEQUE_RET_IND END, " +
            "CASE WHEN t.PID_PRODUCT_CODE IS NULL THEN ''  ELSE t.PID_PRODUCT_CODE END, " +
            "CASE WHEN t.PID_POLICY_BANK IS NULL THEN ''  ELSE t.PID_POLICY_BANK END, " +
            "CASE WHEN t.PID_POLICY_YEAR IS NULL THEN 0  ELSE t.PID_POLICY_YEAR END, " +
            "CASE WHEN t.PID_UPLOAD_DATE IS NULL THEN to_date('01/01/1900', 'DD/MM/RRRR')  ELSE to_date(t.PID_UPLOAD_DATE, 'DD/MM/RRRR') END, " +
            "CASE WHEN t.PID_RV_NO IS NULL THEN ''  ELSE t.PID_RV_NO END, " +
            "CASE WHEN t.PID_POLICY_FEE IS NULL THEN 0  ELSE t.PID_POLICY_FEE END, " +
            "CASE WHEN t.PID_TOTAL_AMT IS NULL THEN 0  ELSE t.PID_TOTAL_AMT END, " +
            "CASE WHEN t.PID_SYS_MAN IS NULL THEN ''  ELSE t.PID_SYS_MAN END, " +
            "CASE WHEN t.PID_BAL_TYPE IS NULL THEN ''  ELSE t.PID_BAL_TYPE END, " +
            "CASE WHEN t.PID_ACT_CONFIRM_DATE IS NULL THEN to_date('01/01/1900', 'DD/MM/RRRR')  ELSE to_date(t.PID_ACT_CONFIRM_DATE, 'DD/MM/RRRR') END, " +
            "CASE WHEN t.PID_FMT_OPBAL IS NULL THEN 0  ELSE t.PID_FMT_OPBAL END, " +
            "CASE WHEN t.PID_NEXT_OPBAL IS NULL THEN ''  ELSE t.PID_NEXT_OPBAL END " +
            "FROM hci_tbl_pid_dm_all_rec2 t  WHERE t.PID_RECEIPT_NO=:V_PID_RECEIPT_NO";
            command = new OracleCommand(sql, connection);
            command.Parameters.Add(new OracleParameter("V_PID_RECEIPT_NO", id));
            connection.Open();
            try
            {
                dataReader = command.ExecuteReader();
                if (dataReader.HasRows)
                {
                    dataReader.Read();
                    pID.PidReceiptNo = dataReader[0].ToString();
                    pID.PidReceiptDate = dataReader[1].ToString();
                    pID.PidCustomerName = dataReader[2].ToString();
                    pID.PidProposalNo = dataReader[3].ToString();
                    pID.PidPolicyNo = dataReader[4].ToString();
                    pID.PidReceiptAmt = Convert.ToInt32(dataReader[5]);
                    pID.PidPaymentMtd = dataReader[6].ToString();
                    pID.PidBranch = dataReader[7].ToString();
                    pID.PidBankCode = dataReader[8].ToString();
                    pID.PidAgtCode = dataReader[9].ToString();
                    pID.PidTimeSlab = dataReader[10].ToString();
                    pID.PidTerm = Convert.ToInt32(dataReader[11]);
                    pID.PidInstallmentAmt = Convert.ToInt32(dataReader[12]);
                    pID.PidTable = Convert.ToInt32(dataReader[13]);
                    pID.PidPayFreq = dataReader[14].ToString();
                    pID.PidConfirmInd = dataReader[15].ToString();
                    pID.PidConfirmAmt = Convert.ToInt32(dataReader[16]);
                    pID.PidRefundInd = dataReader[17].ToString();
                    pID.PidAvailableAmt = Convert.ToInt32(dataReader[18]);
                    pID.PidReverseInd = dataReader[19].ToString();
                    pID.PidCommCalInd = dataReader[20].ToString();
                    pID.PidConfirmBy = dataReader[21].ToString();
                    pID.PidConfirmDate = dataReader[22].ToString();
                    pID.PidLastModifiedBy = dataReader[23].ToString();
                    pID.PidLastModifiedDate = dataReader[24].ToString();
                    pID.PidLastModifiedReson = dataReader[25].ToString();
                    pID.PidIsHeldReceipt = dataReader[26].ToString();
                    pID.PidReceiptType = dataReader[27].ToString();
                    pID.PidTempField1 = dataReader[28].ToString();
                    pID.PidTempField2 = Convert.ToInt32(dataReader[29]);
                    pID.PidTempField3 = dataReader[30].ToString();
                    pID.PidChequeRetInd = dataReader[31].ToString();
                    pID.PidProductCode = dataReader[32].ToString();
                    pID.PidPolicyBank = dataReader[33].ToString();
                    pID.PidPolicyYear = Convert.ToInt32(dataReader[34]);
                    pID.PidUploadDate = dataReader[35].ToString();
                    pID.PidRvNo = dataReader[36].ToString();
                    pID.PidPolicyFee = Convert.ToInt32(dataReader[37]);
                    pID.PidTotalAmt = Convert.ToInt32(dataReader[38]);
                    pID.PidSysMan = dataReader[39].ToString();
                    pID.PidBalType = dataReader[40].ToString();
                    pID.PidActConfirmDate = dataReader[41].ToString();
                    pID.PidFmtOpbal = Convert.ToInt32(dataReader[42]);
                    pID.PidNextOpbal = dataReader[43].ToString();

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
            return pID;
        }

        [HttpPost]
        [ActionName("SavePID")]
        public HttpResponseMessage SavePIDl(PID obj)
        {
            OracleConnection connection = new OracleConnection(ConnectionString);
            OracleCommand command;
            try
            {
                connection.Open();
                command = new OracleCommand("INSERT_HCI_TBL_PID");
                command.CommandType = CommandType.StoredProcedure;
                command.Connection = connection;
                command.Parameters.Add("V_PID_RECEIPT_NO", OracleType.VarChar).Value = obj.PidReceiptNo;
                command.Parameters.Add("V_PID_RECEIPT_DATE", OracleType.DateTime).Value = obj.PidReceiptDate;
                command.Parameters.Add("V_PID_CUSTOMER_NAME", OracleType.VarChar).Value = obj.PidCustomerName;
                command.Parameters.Add("V_PID_PROPOSAL_NO", OracleType.VarChar).Value = obj.PidProposalNo;
                command.Parameters.Add("V_PID_POLICY_NO", OracleType.VarChar).Value = obj.PidPolicyNo;
                command.Parameters.Add("V_PID_RECEIPT_AMT", OracleType.Number).Value = obj.PidReceiptAmt;
                command.Parameters.Add("V_PID_PAYMENT_MTD", OracleType.VarChar).Value = obj.PidPaymentMtd;
                command.Parameters.Add("V_PID_BRANCH", OracleType.VarChar).Value = obj.PidBranch;
                command.Parameters.Add("V_PID_BANK_CODE", OracleType.VarChar).Value = obj.PidBankCode;
                command.Parameters.Add("V_PID_AGT_CODE", OracleType.VarChar).Value = obj.PidAgtCode;
                command.Parameters.Add("V_PID_TIME_SLAB", OracleType.VarChar).Value = obj.PidTimeSlab;
                command.Parameters.Add("V_PID_TERM", OracleType.Number).Value = obj.PidTerm;
                command.Parameters.Add("V_PID_INSTALLMENT_AMT", OracleType.Number).Value = obj.PidInstallmentAmt;
                command.Parameters.Add("V_PID_TABLE", OracleType.Number).Value = obj.PidTable;
                command.Parameters.Add("V_PID_PAY_FREQ", OracleType.VarChar).Value = obj.PidPayFreq;
                command.Parameters.Add("V_PID_CONFIRM_IND", OracleType.VarChar).Value = obj.PidConfirmInd;
                command.Parameters.Add("V_PID_CONFIRM_AMT", OracleType.Number).Value = obj.PidConfirmAmt;
                command.Parameters.Add("V_PID_REFUND_IND", OracleType.VarChar).Value = obj.PidRefundInd;
                command.Parameters.Add("V_PID_AVAILABLE_AMT", OracleType.Number).Value = obj.PidAvailableAmt;
                command.Parameters.Add("V_PID_REVERSE_IND", OracleType.VarChar).Value = obj.PidReverseInd;
                command.Parameters.Add("V_PID_COMM_CAL_IND", OracleType.VarChar).Value = obj.PidCommCalInd;
                command.Parameters.Add("V_PID_CONFIRM_BY", OracleType.VarChar).Value = obj.PidConfirmBy;
                command.Parameters.Add("V_PID_CONFIRM_DATE", OracleType.DateTime).Value = obj.PidConfirmDate;
                command.Parameters.Add("V_PID_LAST_MODIFIED_BY", OracleType.VarChar).Value = obj.PidLastModifiedBy;
                command.Parameters.Add("V_PID_LAST_MODIFIED_DATE", OracleType.DateTime).Value = obj.PidLastModifiedDate;
                command.Parameters.Add("V_PID_LAST_MODIFIED_RESON", OracleType.VarChar).Value = obj.PidLastModifiedReson;
                command.Parameters.Add("V_PID_IS_HELD_RECEIPT", OracleType.VarChar).Value = obj.PidIsHeldReceipt;
                command.Parameters.Add("V_PID_RECEIPT_TYPE", OracleType.VarChar).Value = obj.PidReceiptType;
                command.Parameters.Add("V_PID_TEMP_FIELD_1", OracleType.VarChar).Value = obj.PidTempField1;
                command.Parameters.Add("V_PID_TEMP_FIELD_2", OracleType.Number).Value = obj.PidTempField2;
                command.Parameters.Add("V_PID_TEMP_FIELD_3", OracleType.VarChar).Value = obj.PidTempField3;
                command.Parameters.Add("V_PID_CHEQUE_RET_IND", OracleType.VarChar).Value = obj.PidChequeRetInd;
                command.Parameters.Add("V_PID_PRODUCT_CODE", OracleType.VarChar).Value = obj.PidProductCode;
                command.Parameters.Add("V_PID_POLICY_BANK", OracleType.VarChar).Value = obj.PidPolicyBank;
                command.Parameters.Add("V_PID_POLICY_YEAR", OracleType.Number).Value = obj.PidPolicyYear;
                command.Parameters.Add("V_PID_UPLOAD_DATE", OracleType.DateTime).Value = obj.PidUploadDate;
                command.Parameters.Add("V_PID_RV_NO", OracleType.VarChar).Value = obj.PidRvNo;
                command.Parameters.Add("V_PID_POLICY_FEE", OracleType.Number).Value = obj.PidPolicyFee;
                command.Parameters.Add("V_PID_TOTAL_AMT", OracleType.Number).Value = obj.PidTotalAmt;
                command.Parameters.Add("V_PID_SYS_MAN", OracleType.VarChar).Value = obj.PidSysMan;
                command.Parameters.Add("V_PID_BAL_TYPE", OracleType.VarChar).Value = obj.PidBalType;
                command.Parameters.Add("V_PID_ACT_CONFIRM_DATE", OracleType.DateTime).Value = obj.PidActConfirmDate;
                command.Parameters.Add("V_PID_FMT_OPBAL", OracleType.Number).Value = obj.PidFmtOpbal;
                command.Parameters.Add("V_PID_NEXT_OPBAL", OracleType.VarChar).Value = obj.PidNextOpbal;
                command.ExecuteNonQuery();
                connection.Close();
                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception exception)
            {
                connection.Close();
                return Request.CreateResponse(HttpStatusCode.ExpectationFailed);
            }
        }


    }
}
