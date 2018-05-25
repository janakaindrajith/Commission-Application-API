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
    public class AgentHistoryController : ApiController
    {
        static string ConnectionString = ConfigurationManager.ConnectionStrings["OracleConString"].ToString();

        [HttpGet]
        public IEnumerable<agentHistory> GetAgetHistoryByCode(string AGT_CODE, Int16 TYPE)
        {
            //1 = Transfer Branch History
            //2 = Stop Commission History
            //3 = Release Commission History

            List<agentHistory> agentHistoryList = new List<agentHistory>();
            DataTable dataTable = new DataTable();
            OracleDataReader dataReader = null;
            OracleConnection connection = new OracleConnection(ConnectionString);
            OracleCommand command;
            string sql = "";

            if (TYPE == 1)
            {
                sql = " SELECT "+
                      " AGT.AGT_TRNS_BRANCH_CODE, "+
                      " AGT.AGT_TRNS_BRANCH_DATE, "+
                      " AGT.AGT_REMARKS, " +
                      " AGT.AGT_CREATED_BY, "+
                      " AGT.AGT_CREATED_DATE " +
                      " FROM HCI_TBL_AGENT AGT WHERE AGT.AGT_CODE =:V_AGET_CODE ORDER BY AGT.AGT_CREATED_DATE DESC";
            }

            if (TYPE == 2)
            {
                sql = " SELECT " +
                      " AGT.AGT_STOP_COMM_DATE, " +
                      " AGT.AGT_STOP_COMM_REASON, " +
                      " AGT.AGT_REMARKS, " +
                      " AGT.AGT_CREATED_BY, " +
                      " AGT.AGT_CREATED_DATE " +
                      " FROM HCI_TBL_AGENT AGT WHERE AGT.AGT_CODE =:V_AGET_CODE ORDER BY AGT.AGT_CREATED_DATE DESC";
            }

            if (TYPE == 3)
            {
                sql = " SELECT " +
                      " AGT.AGT_RELEASE_COMM_DATE, " +
                      " AGT.AGT_RELEASE_COMM_REASON, " +
                      " AGT.AGT_REMARKS, " +
                      " AGT.AGT_CREATED_BY, " +
                      " AGT.AGT_CREATED_DATE " +
                      " FROM HCI_TBL_AGENT AGT WHERE AGT.AGT_CODE =:V_AGET_CODE ORDER BY AGT.AGT_CREATED_DATE DESC";
            }


            command = new OracleCommand(sql, connection);
            command.Parameters.Add(new OracleParameter("V_AGET_CODE", AGT_CODE));

            try
            {
                connection.Open();
                dataReader = command.ExecuteReader();
                dataTable.Load(dataReader);
                dataReader.Close();
                connection.Close();
                agentHistoryList = (from DataRow drow in dataTable.Rows
                                    select new agentHistory()
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
                if (dataReader != null || connection.State == ConnectionState.Open)
                {
                    dataReader.Close();
                    connection.Close();
                }
            }
            return agentHistoryList;
        }


    }
}
