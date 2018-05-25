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
    public class AgentCodeByTypeIDController : ApiController
    {

        static string ConnectionString = ConfigurationManager.ConnectionStrings["OracleConString"].ToString();


        // GET: api/Bank
        [System.Web.Http.HttpGet]
        public IEnumerable<AgentCodeByTypeID> Get(Int32 AgentTypeID)
        {


            List<AgentCodeByTypeID> AgentCodeList = new List<AgentCodeByTypeID>();
            DataTable dataTable = new DataTable();
            OracleDataReader dataReader = null;
            OracleConnection connection = new OracleConnection(ConnectionString);
            OracleCommand command;

            string sql = " SELECT AC.*, AC.rowid from HCI_TBL_AGENT_CODE AC , HCI_TBL_AGENT_TYPE AT "+
                         " WHERE AC.TYPE_ID = AT.ID AND AC.TYPE_ID =: vAgentTypeID ";

            command = new OracleCommand(sql, connection);
            command.Parameters.Add(new OracleParameter("vAgentTypeID", AgentTypeID));

            try
            {

                connection.Open();
                dataReader = command.ExecuteReader();
                dataTable.Load(dataReader);
                dataReader.Close();
                connection.Close();
                AgentCodeList = (from DataRow drow in dataTable.Rows
                               select new AgentCodeByTypeID()
                               {
                                   ID = Convert.ToInt32(drow[0]),
                                   DESCRIPTION = drow[1].ToString(),
                                   TYPE_ID = Convert.ToInt32(drow[2]),
                                   CODE_LEN = Convert.ToInt32(drow[3])
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
            return AgentCodeList;

        }

    }
}
