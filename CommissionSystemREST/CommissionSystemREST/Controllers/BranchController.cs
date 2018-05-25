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
    public class BranchController : ApiController
    {
        static string ConnectionString = ConfigurationManager.ConnectionStrings["OracleConString"].ToString();


        // GET: api/Bank
        [System.Web.Http.HttpGet]
        public IEnumerable<Branch> Get()
        {
            List<Branch> List = new List<Branch>();
            DataTable dataTable = new DataTable();
            OracleDataReader dataReader = null;
            OracleConnection connection = new OracleConnection(ConnectionString);
            OracleCommand command;

            string sql = "select t.id,t.code,t.description from HCI_TBL_BRANCH t ORDER BY T.ID";

            command = new OracleCommand(sql, connection);
            try
            {

                connection.Open();
                dataReader = command.ExecuteReader();
                dataTable.Load(dataReader);
                dataReader.Close();
                connection.Close();
                List = (from DataRow drow in dataTable.Rows
                            select new Branch()
                            {
                                ID = Convert.ToInt32(drow[0]),
                                CODE = drow[1].ToString(),
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
            return List;

        }

        // POST: api/Quotation
        [HttpPost]
        [ActionName("AddBranch")]
        public string AddBank(Branch Branch)
        {
            try
            {
                OracleConnection connection = new OracleConnection(ConnectionString);
                OracleCommand command;

                string returnVal = "";
                connection.Open();
                OracleCommand cmd = null;

                cmd = new OracleCommand("SP_HC_BRANCH");

                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Connection = connection;

                cmd.Parameters.Add("vID", OracleType.Number).Value = Branch.ID;
                cmd.Parameters.Add("vCODE", OracleType.VarChar).Value = Branch.CODE;
                cmd.Parameters.Add("vDESCRIPTION", OracleType.VarChar).Value = Branch.DESCRIPTION;
     
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
