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
    public class AgentSearchController : ApiController
    {
        static string ConnectionString = ConfigurationManager.ConnectionStrings["OracleConString"].ToString();


        // GET: api/Bank
        [HttpPost]
        [ActionName("SearchAgents")]
        public IEnumerable<AgentSearch> SearchAgents(AgentSearch AgentSearch)
        {

            string sql = "";
            string sqlWhere = "";



            if (AgentSearch.AGT_CODE != null && AgentSearch.AGT_CODE != "")
            {
                sqlWhere = sqlWhere + "(UPPER(t.agt_code) LIKE '%" + AgentSearch.AGT_CODE.ToUpper() + "%') AND";
            }

            if (AgentSearch.AGT_NAME != null && AgentSearch.AGT_NAME != "")
            {   
                sqlWhere = sqlWhere + "(UPPER(t.AGT_FULL_NAME) LIKE '%" + AgentSearch.AGT_NAME.ToUpper() + "%') AND";
            }
                
            if (AgentSearch.AGT_ADDRESS != null && AgentSearch.AGT_ADDRESS != "")
            {
                sqlWhere = sqlWhere + "(UPPER(t.AGT_FULL_ADDRESS) LIKE '%" + AgentSearch.AGT_ADDRESS.ToUpper() + "%') AND";
            }

            if (AgentSearch.AGT_MOBILE != null && AgentSearch.AGT_MOBILE != "")
            {
                sqlWhere = sqlWhere + "(UPPER(t.AGT_MOBILE) LIKE '%" + AgentSearch.AGT_MOBILE.ToUpper() + "%') AND";
            }

            if (AgentSearch.AGT_NIC_NO != null && AgentSearch.AGT_NIC_NO != "")
            {
                sqlWhere = sqlWhere + "(UPPER(t.AGT_NIC_NO) LIKE '%" + AgentSearch.AGT_NIC_NO.ToUpper() + "%') AND";
            }

            if (sqlWhere.Length > 0)
            {
                sqlWhere = sqlWhere.Substring(0, sqlWhere.Length - 3);
            }

            sqlWhere = sqlWhere + "and t.agt_effective_end_date is null";


            List<AgentSearch> AgentList = new List<AgentSearch>();
            DataTable dataTable = new DataTable();
            OracleDataReader dataReader = null;
            OracleConnection connection = new OracleConnection(ConnectionString);
            OracleCommand command;

            sql = "select t.agt_id,t.agt_code,t.AGT_FULL_NAME,t.AGT_FULL_ADDRESS,t.agt_nic_no,t.agt_mobile from hci_tbl_agent t where (" + sqlWhere + ")";

            command = new OracleCommand(sql, connection);
            try
            {

                connection.Open();
                dataReader = command.ExecuteReader();
                dataTable.Load(dataReader);
                dataReader.Close();
                connection.Close();
                AgentList = (from DataRow drow in dataTable.Rows
                             select new AgentSearch()
                             {
                                 AGT_ID = drow[0].ToString(),
                                 AGT_CODE = drow[1].ToString(),
                                 AGT_NAME = drow[2].ToString(),
                                 AGT_ADDRESS = drow[3].ToString(),
                                 AGT_NIC_NO = drow[4].ToString(),
                                 AGT_MOBILE = drow[5].ToString()


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
