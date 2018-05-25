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
    public class BankController : ApiController
    {
        static string ConnectionString = ConfigurationManager.ConnectionStrings["OracleConString"].ToString();


        // GET: api/Bank
        [System.Web.Http.HttpGet]
        public IEnumerable<Bank> Get()
        {
            List<Bank> bankList = new List<Bank>();
            DataTable dataTable = new DataTable();
            OracleDataReader dataReader = null;
            OracleConnection connection = new OracleConnection(ConnectionString);
            OracleCommand command;

            string sql = "select t.id,t.code,t.description from HCI_TBL_BANK t ORDER BY T.ID";

            command = new OracleCommand(sql, connection);
            try
            {

                connection.Open();
                dataReader = command.ExecuteReader();
                dataTable.Load(dataReader);
                dataReader.Close();
                connection.Close();
                bankList = (from DataRow drow in dataTable.Rows
                            select new Bank()
                            {
                                ID = Convert.ToInt64(drow[0]),
                                CODE = Convert.ToString(drow[1].ToString()),
                                DESCRIPTION = drow[2].ToString()
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
            return bankList;

        }

        // POST: api/Quotation
        [HttpPost]
        [ActionName("SaveBank")]
        public string SaveBank(Bank BANK)
        {
            try
            {
                OracleConnection connection = new OracleConnection(ConnectionString);
                OracleCommand command;

                string returnVal = "";
                connection.Open();
                OracleCommand cmd = null;

                cmd = new OracleCommand("hci_sp_bank");

                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Connection = connection;

                //cmd.Parameters.Add("vID", OracleType.Number).Value = BANK.ID;
                cmd.Parameters.Add("vCODE", OracleType.VarChar).Value = BANK.CODE;
                cmd.Parameters.Add("vDESCRIPTION", OracleType.VarChar).Value = BANK.DESCRIPTION;

                cmd.ExecuteNonQuery();

                connection.Close();

                return "Successfully Saved";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
    }
}
