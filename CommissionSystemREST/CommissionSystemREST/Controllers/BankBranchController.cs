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
    public class BankBranchController : ApiController
    {
        static string ConnectionString = ConfigurationManager.ConnectionStrings["OracleConString"].ToString();


        // GET: api/Bank
        [System.Web.Http.HttpGet]
        public IEnumerable<BankBranch> Get(String BankID)
        {


            List<BankBranch> BBranchList = new List<BankBranch>();
            DataTable dataTable = new DataTable();
            OracleDataReader dataReader = null;
            OracleConnection connection = new OracleConnection(ConnectionString);
            OracleCommand command;

            string sql = " select t.id,t.bank_id,t.code, t.description from hci_tbl_bank_branch t , hci_tbl_bank b " +
             " where t.bank_id = b.id and t.bank_id =:vBankID";

            command = new OracleCommand(sql, connection);
            command.Parameters.Add(new OracleParameter("vBankID", BankID));

            try
            {

                connection.Open();
                dataReader = command.ExecuteReader();
                dataTable.Load(dataReader);
                dataReader.Close();
                connection.Close();
                BBranchList = (from DataRow drow in dataTable.Rows
                        select new BankBranch()
                        {
                            ID = Convert.ToInt32(drow[0]),
                            BANK_ID = Convert.ToInt32(drow[1]),
                            CODE = drow[2].ToString(),
                            DESCRIPTION = drow[3].ToString()
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
            return BBranchList;

         }

    }
}
