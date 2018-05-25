using ComissionWebAPI.Models;
using System;
using System.Configuration;
using System.Data;
using System.Data.OracleClient;
using System.Web.Http;


namespace ComissionWebAPI.Controllers
{
    public class AgentLeaderCodeByAgentCodeController : ApiController
    {
        static string ConnectionString = ConfigurationManager.ConnectionStrings["OracleConString"].ToString();

        [HttpGet]
        public AgentLeaderCodeByAgentCode GetLeaderCodeByAgentCode(string AgentCode)
        {
            AgentLeaderCodeByAgentCode LeaderCode = new AgentLeaderCodeByAgentCode();
            OracleDataReader dataReader = null;
            OracleConnection connection = new OracleConnection(ConnectionString);
            OracleCommand command;

            string sql = " SELECT AGT_LEADER_LEADER_CODE_V" +
                         " FROM HCI_TBL_AGENT t WHERE t.AGT_CODE =:vAGT_CODE AND t.agt_effective_end_date is null";

            command = new OracleCommand(sql, connection);
            command.Parameters.Add(new OracleParameter("vAGT_CODE", AgentCode));
            connection.Open();
            try
            {
                dataReader = command.ExecuteReader();
                if (dataReader.HasRows)
                {
                    dataReader.Read();

                    LeaderCode.LEADER_LEADER_CODE = dataReader[0].ToString();

                    dataReader.Close();
                    connection.Close();
                }
                else
                {
                    LeaderCode.LEADER_LEADER_CODE = "-";
                    return LeaderCode;
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
            return LeaderCode;

        }



    }
}
