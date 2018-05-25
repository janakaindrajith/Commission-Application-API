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
    public class CommonController : ApiController
    {


        static string ConnectionString = ConfigurationManager.ConnectionStrings["OracleConString"].ToString();[HttpGet]



        public IEnumerable<Common> GetRefundsByReceiptNo(string ReceiptNo)
        {
            List<Common> refundList = new List<Common>();
            DataTable dataTable = new DataTable();
            OracleDataReader dataReader1 = null;
            OracleConnection connection = new OracleConnection(ConnectionString);
            OracleCommand command1;
            string sql = " select t.rfd_receipt_no,t.rfd_amt,t.rfd_refund_date,t.rfd_agt_code ,t.rfd_reason " +
                         " from hci_tbl_refunds t where t.rfd_receipt_no =:V_RNO";

            //" FROM HCI_TBL_PROPERTY t  WHERE t.TYPE=:V_TYPE ORDER BY T.Id asc";

            command1 = new OracleCommand(sql, connection);
            command1.Parameters.Add(new OracleParameter("V_RNO", ReceiptNo));
            try
            {
                connection.Open();
                dataReader1 = command1.ExecuteReader();
                dataTable.Load(dataReader1);
                dataReader1.Close();
                connection.Close();
                refundList = (from DataRow drow in dataTable.Rows
                                select new Common()
                                {

                                    Column1 = drow[0].ToString(),
                                    Column2 = drow[1].ToString(),
                                    Column3 = drow[2].ToString(),
                                    Column4 = drow[3].ToString(),
                                    Column5 = drow[4].ToString()


                                }).ToList();
            }
            catch (Exception exception)
            {
                if (dataReader1 != null || connection.State == ConnectionState.Open)
                {
                    dataReader1.Close();
                    connection.Close();
                }
            }
            return refundList;
        }




        public IEnumerable<Common> GetCommissionByReceiptNo(string ReceiptNo)
        {
            List<Common> refundList = new List<Common>();
            DataTable dataTable = new DataTable();
            OracleDataReader dataReader2 = null;
            OracleConnection connection = new OracleConnection(ConnectionString);
            OracleCommand command2;
            string sql = " select h.cad_receipt_no,h.CAD_COMM_AMT,h.CAD_CAL_DATE,h.cad_policy_no,h.cad_pay_freq  " +
                         " from hci_tbl_comm_adv_dm h where h.cad_receipt_no =:V_RNO";


            command2 = new OracleCommand(sql, connection);
            command2.Parameters.Add(new OracleParameter("V_RNO", ReceiptNo));
            try
            {
                connection.Open();
                dataReader2 = command2.ExecuteReader();
                dataTable.Load(dataReader2);
                dataReader2.Close();
                connection.Close();
                refundList = (from DataRow drow in dataTable.Rows
                              select new Common()
                              {

                                  Column1 = drow[0].ToString(),
                                  Column2 = drow[1].ToString(),
                                  Column3 = drow[2].ToString(),
                                  Column4 = drow[3].ToString(),
                                  Column5 = drow[4].ToString()


                              }).ToList();
            }
            catch (Exception exception)
            {
                if (dataReader2 != null || connection.State == ConnectionState.Open)
                {
                    dataReader2.Close();
                    connection.Close();
                }
            }
            return refundList;
        }




        public IEnumerable<Common> GetCommissionRefundByReceiptNo(string ReceiptNo)
        {
            List<Common> refundList = new List<Common>();
            DataTable dataTable = new DataTable();
            OracleDataReader dataReader3 = null;
            OracleConnection connection = new OracleConnection(ConnectionString);
            OracleCommand command3;
            string sql = " select t1.rfc_receipt_no,t1.rfc_comm,t1.rfc_refund_date,t1.rfc_comm_year,t1.rfc_comm_type   " +
                         " from hci_tbl_refund_commission t1 where T1.rfc_receipt_no =:V_RNO";


            command3 = new OracleCommand(sql, connection);
            command3.Parameters.Add(new OracleParameter("V_RNO", ReceiptNo));
            try
            {
                connection.Open();
                dataReader3 = command3.ExecuteReader();
                dataTable.Load(dataReader3);
                dataReader3.Close();
                connection.Close();
                refundList = (from DataRow drow in dataTable.Rows
                              select new Common()
                              {

                                  Column1 = drow[0].ToString(),
                                  Column2 = drow[1].ToString(),
                                  Column3 = drow[2].ToString(),
                                  Column4 = drow[3].ToString(),
                                  Column5 = drow[4].ToString()


                              }).ToList();
            }
            catch (Exception exception)
            {
                if (dataReader3 != null || connection.State == ConnectionState.Open)
                {
                    dataReader3.Close();
                    connection.Close();
                }
            }
            return refundList;
        }



        public IEnumerable<Common> GetPermissionPagesByUser(string UserName)
        {
            List<Common> refundList = new List<Common>();
            DataTable dataTable = new DataTable();
            OracleDataReader dataReader2 = null;
            OracleConnection connection = new OracleConnection(ConnectionString);
            OracleCommand command2;

            string sql = " select t.user_name,p.user_access_page,'' as a, '' as b, '' as c from hci_tbl_user_details t, hci_tbl_user_role_previlages p " +
                         " where p.user_role_id = t.user_role_id and t.user_name = :USER_NAME";


            command2 = new OracleCommand(sql, connection);
            command2.Parameters.Add(new OracleParameter("USER_NAME", UserName));
            try
            {
                connection.Open();
                dataReader2 = command2.ExecuteReader();
                dataTable.Load(dataReader2);
                dataReader2.Close();
                connection.Close();
                refundList = (from DataRow drow in dataTable.Rows
                              select new Common()
                              {

                                  Column1 = drow[0].ToString(),
                                  Column2 = drow[1].ToString(),
                                  Column3 = drow[2].ToString(),
                                  Column4 = drow[3].ToString(),
                                  Column5 = drow[4].ToString()


                              }).ToList();
            }
            catch (Exception exception)
            {
                if (dataReader2 != null || connection.State == ConnectionState.Open)
                {
                    dataReader2.Close();
                    connection.Close();
                }
            }
            return refundList;
        }



    }
}
