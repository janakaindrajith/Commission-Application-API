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
    public class AgentTypeController : ApiController
    {
        static string ConnectionString = ConfigurationManager.ConnectionStrings["OracleConString"].ToString();


        // GET: api/Bank
        [System.Web.Http.HttpGet]
        public IEnumerable<AgentType> Get()
        {
            List<AgentType> AgentTypeList = new List<AgentType>();
            DataTable dataTable = new DataTable();
            OracleDataReader dataReader = null;
            OracleConnection connection = new OracleConnection(ConnectionString);
            OracleCommand command;

            string sql = "select t.id,t.description from hci_tbl_agent_type t ORDER BY t.id";

            command = new OracleCommand(sql, connection);
            try
            {

                connection.Open();
                dataReader = command.ExecuteReader();
                dataTable.Load(dataReader);
                dataReader.Close();
                connection.Close();
                AgentTypeList = (from DataRow drow in dataTable.Rows
                            select new AgentType()
                            {
                                ID = Convert.ToInt32(drow[0]),
                                DESCRIPTION = drow[1].ToString()
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
            return AgentTypeList;

        }

    }
}
