using ComissionWebAPI.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.OracleClient;
using System.Linq;
using System.Web.Http;


namespace ComissionWebAPI.Controllers
{
    public class PIDSearchController : ApiController
    {
        static string ConnectionString = ConfigurationManager.ConnectionStrings["OracleConString"].ToString();

        // GET: api/Bank
        [HttpPost]
        [ActionName("SearchPID")]
        public IEnumerable<PIDSearch> SearchPID(PIDSearch PIDSearch)
        {

            string sql = "";
            string sqlWhere = "";


            //LIKE
            //if (PIDSearch.PID_RECEIPT_NO != null && PIDSearch.PID_RECEIPT_NO != "")
            //{
            //    sqlWhere = sqlWhere + "(UPPER(t.pid_receipt_no) LIKE '%" + PIDSearch.PID_RECEIPT_NO.ToUpper() + "%') AND";
            //}

            //if (PIDSearch.PID_AGT_CODE != null && PIDSearch.PID_AGT_CODE != "")
            //{
            //    sqlWhere = sqlWhere + "(UPPER(t.pid_agt_code) LIKE '%" + PIDSearch.PID_AGT_CODE.ToUpper() + "%') AND";
            //}

            //if (PIDSearch.PID_PROPOSAL_NO != null && PIDSearch.PID_PROPOSAL_NO != "")
            //{
            //    sqlWhere = sqlWhere + "(UPPER(t.pid_proposal_no) LIKE '%" + PIDSearch.PID_PROPOSAL_NO.ToUpper() + "%') AND";
            //}

            //if (PIDSearch.PID_POLICY_NO != null && PIDSearch.PID_POLICY_NO != "")
            //{
            //    sqlWhere = sqlWhere + "(UPPER(t.pid_policy_no) LIKE '%" + PIDSearch.PID_POLICY_NO.ToUpper() + "%') AND";
            //}


            if (PIDSearch.PID_RECEIPT_NO != null && PIDSearch.PID_RECEIPT_NO != "")
            {
                sqlWhere = sqlWhere + "t.pid_receipt_no = '"+ PIDSearch.PID_RECEIPT_NO.ToUpper() + "' AND";
            }

            if (PIDSearch.PID_AGT_CODE != null && PIDSearch.PID_AGT_CODE != "")
            {
                sqlWhere = sqlWhere + "t.pid_agt_code = '"+ PIDSearch.PID_AGT_CODE.ToUpper() + "' AND";
            }

            if (PIDSearch.PID_PROPOSAL_NO != null && PIDSearch.PID_PROPOSAL_NO != "")
            {
                sqlWhere = sqlWhere + "t.pid_proposal_no = '" + PIDSearch.PID_PROPOSAL_NO.ToUpper() + "' AND";
            }

            if (PIDSearch.PID_POLICY_NO != null && PIDSearch.PID_POLICY_NO != "")
            {
                sqlWhere = sqlWhere + "t.pid_policy_no = '" + PIDSearch.PID_POLICY_NO.ToUpper() + "' AND";
            }


            if (sqlWhere.Length > 0)
            {
                sqlWhere = sqlWhere.Substring(0, sqlWhere.Length - 3);
            }

            //sqlWhere = sqlWhere + "and t.agt_effective_end_date is null";


            List<PIDSearch> AgentList = new List<PIDSearch>();
            DataTable dataTable = new DataTable();
            OracleDataReader dataReader = null;
            OracleConnection connection = new OracleConnection(ConnectionString);
            OracleCommand command;

            sql = " select t.pid_receipt_no,t.pid_receipt_date,t.pid_customer_name,t.pid_proposal_no,t.pid_policy_no, "+
                  " CASE WHEN t.pid_receipt_amt IS NULL THEN 0  ELSE t.pid_receipt_amt END as pid_receipt_amt ,t.pid_time_slab, CASE WHEN t.pid_confirm_amt IS NULL THEN 0  ELSE t.pid_confirm_amt END as pid_confirm_amt, t.pid_confirm_date, t.pid_rv_no, t.pid_bal_type,t.pid_agt_code, t.pid_payment_mtd " +
                  " from HCI_TBL_MAY_PID_ACC t where(" + sqlWhere + ")";
            

            command = new OracleCommand(sql, connection);
            try
            {

                connection.Open();
                dataReader = command.ExecuteReader();
                dataTable.Load(dataReader);
                dataReader.Close();
                connection.Close();


                AgentList = (from DataRow drow in dataTable.Rows
                             select new PIDSearch()
                             {
                                 PID_RECEIPT_NO = drow[0].ToString(),
                                 PID_RECEIPT_DATE = drow[1].ToString(),
                                 PID_CUSTOMER = drow[2].ToString(),
                                 PID_PROPOSAL_NO = drow[3].ToString(),
                                 PID_POLICY_NO = drow[4].ToString(),
                                 PID_RECEIPT_AMT = Convert.ToDouble(drow[5].ToString()),
                                 PID_TIME_SLAB = drow[6].ToString(),
                                 PID_CONFIRM_AMT = Convert.ToDouble(drow[7].ToString()),
                                 PID_CONFIRM_DATE = drow[8].ToString(),
                                 PID_RV_NO = drow[9].ToString(),
                                 PID_BAL_TYPE = drow[10].ToString(),
                                 PID_AGT_CODE = drow[11].ToString(),
                                 PID_CHEQUE_NO = drow[12].ToString()
                             }).ToList();


            }
            catch (Exception exception)
            {
                if (dataReader != null)
                {
                    dataReader.Close();
                }

                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }

            }
            return AgentList;

        }



        // GET: api/Bank
        [HttpPost]
        [ActionName("SearchPIDByProposalNo")]
        public IEnumerable<PIDSearch> SearchPIDByProposalNo(PIDSearch PIDSearch)
        {

            string sql = "";
            string sqlWhere = "";

            if (PIDSearch.PID_PROPOSAL_NO != null && PIDSearch.PID_PROPOSAL_NO != "")
            {
                sqlWhere = sqlWhere + "t.pid_proposal_no = '" + PIDSearch.PID_PROPOSAL_NO.ToUpper() + "' AND";
            }

            if (sqlWhere.Length > 0)
            {
                sqlWhere = sqlWhere.Substring(0, sqlWhere.Length - 3);
            }


            List<PIDSearch> AgentList = new List<PIDSearch>();
            DataTable dataTable = new DataTable();
            OracleDataReader dataReader = null;
            OracleConnection connection = new OracleConnection(ConnectionString);
            OracleCommand command;

 
            sql = " SELECT PID_RECEIPT_NO, PID_RECEIPT_DATE, PID_RECEIPT_AMT , PID_PAYMENT_MTD FROM HCI_TBL_APR_PID_ACC_T4 " +
                  " WHERE PID_PROPOSAL_NO = '" + PIDSearch.PID_PROPOSAL_NO + "' " +
                  " AND PID_AVAILABLE_AMT > 0 " +
                  " ORDER BY PID_RECEIPT_DATE DESC, " +
                  " CASE WHEN LENGTH(PID_RECEIPT_NO) = 13 THEN 1 ELSE 2 END, SUBSTR(PID_RECEIPT_NO, 13, 6) DESC, " +
                  " CASE  WHEN PID_BAL_TYPE = 'NMT' THEN 0 WHEN PID_BAL_TYPE = 'FMT' THEN 1 WHEN PID_BAL_TYPE = 'OPB' THEN 2 " +
                  " WHEN PID_BAL_TYPE = 'TFR' THEN 3  ELSE NULL END DESC";


            command = new OracleCommand(sql, connection);
            try
            {

                connection.Open();
                dataReader = command.ExecuteReader();
                dataTable.Load(dataReader);




                dataReader.Close();
                connection.Close();


                AgentList = (from DataRow drow in dataTable.Rows
                             select new PIDSearch()
                             {
                                 PID_RECEIPT_NO = drow[0].ToString(),
                                 PID_RECEIPT_DATE = drow[1].ToString(),
                                 PID_CUSTOMER = "",
                                 PID_PROPOSAL_NO = "",
                                 PID_POLICY_NO = "",
                                 PID_RECEIPT_AMT = Convert.ToDouble(drow[2].ToString()),
                                 PID_TIME_SLAB = "",
                                 PID_CONFIRM_AMT = 0,
                                 PID_CONFIRM_DATE = "",
                                 PID_RV_NO = "",
                                 PID_BAL_TYPE = "",
                                 PID_AGT_CODE = "",
                                 PID_CHEQUE_NO = drow[3].ToString()
                             }).ToList();


            }
            catch (Exception exception)
            {
                if (dataReader != null)
                {
                    dataReader.Close();
                }

                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }

            }
            return AgentList;

        }

    }
}
