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
    public class AgentAttachedController : ApiController
    {
        static string ConnectionString = ConfigurationManager.ConnectionStrings["OracleConString"].ToString();


        // GET: api/AgentAttachedList
        [System.Web.Http.HttpGet]
        public IEnumerable<AgentAttachedList> GetAttachedListByAgentCode(string vAGT_CODE)
        {
            List<AgentAttachedList> AgentAttachedList = new List<AgentAttachedList>();
            DataTable dataTable = new DataTable();
            OracleDataReader dataReader = null;
            OracleConnection connection = new OracleConnection(ConnectionString);
            OracleCommand command;

            string sql = " SELECT A.AGT_ID, A.AGT_CODE, A.AGT_FULL_NAME FROM HCI_TBL_AGENT A "+
                         " WHERE A.AGT_LEADER_AGENT_CODE_V =:vAGT_CODE AND A.AGT_EFFECTIVE_END_DATE IS NULL";

            command = new OracleCommand(sql, connection);
            command.Parameters.Add(new OracleParameter("vAGT_CODE", vAGT_CODE));

            try
            {

                connection.Open();
                dataReader = command.ExecuteReader();
                dataTable.Load(dataReader);
                dataReader.Close();
                connection.Close();
                AgentAttachedList = (from DataRow drow in dataTable.Rows
                            select new AgentAttachedList()
                            {
                                AGT_ID = Convert.ToInt64(drow[0]),
                                AGT_CODE = Convert.ToString(drow[1].ToString()),
                                AGT_NAME = drow[2].ToString()
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
            return AgentAttachedList;

        }

    }
}
