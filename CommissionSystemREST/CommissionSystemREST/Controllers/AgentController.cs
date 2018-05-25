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
    public class AgentController : ApiController
    {
        static string ConnectionString = ConfigurationManager.ConnectionStrings["OracleConString"].ToString();


        // GET: api/Bank
        [System.Web.Http.HttpGet]
        public IEnumerable<Agent> Get()
        {
            List<Agent> AgentList = new List<Agent>();
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
                AgentList = (from DataRow drow in dataTable.Rows
                            select new Agent()
                            {
                                AGT_ID = drow[0].ToString(),
                                //AGT_CODE_ID = drow[1].ToString(),
                                //AGT_TYPE_ID = drow[2].ToString(),
                                //AGT_FIRST_NAME = drow[3].ToString(),
                                //AGT_LAST_NAME = drow[3].ToString()

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




        [HttpGet]
        public Agent GetAgentByID(int id)
        {
            Agent agent = new Agent();
            OracleDataReader dataReader = null;
            OracleConnection connection = new OracleConnection(ConnectionString);
            OracleCommand command;
            string sql = "SELECT " +
            "CASE WHEN t.AGT_FULL_NAME IS NULL THEN ''  ELSE t.AGT_FULL_NAME END, " +
            "CASE WHEN t.AGT_FULL_ADDRESS IS NULL THEN ''  ELSE t.AGT_FULL_ADDRESS END, " +
            "CASE WHEN t.AGT_BUSINESS_TYPE IS NULL THEN 0  ELSE t.AGT_BUSINESS_TYPE END, " +
            "CASE WHEN t.AGT_LEVEL_CODE IS NULL THEN ''  ELSE t.AGT_LEVEL_CODE END, " +
            "CASE WHEN t.AGT_LEADER_CODE IS NULL THEN ''  ELSE t.AGT_LEADER_CODE END, " +
            "CASE WHEN t.AGT_STATUS IS NULL THEN 0  ELSE t.AGT_STATUS END, " +
            "CASE WHEN t.AGT_TERMINATE_STATUS IS NULL THEN 0  ELSE t.AGT_TERMINATE_STATUS END, " +
            "CASE WHEN t.AGT_STOP_COMM_STATUS IS NULL THEN 0  ELSE t.AGT_STOP_COMM_STATUS END, " +
            "CASE WHEN t.AGT_ISS_STATUS IS NULL THEN 0  ELSE t.AGT_ISS_STATUS END, " +
            "CASE WHEN t.AGT_ISS_AMOUNT IS NULL THEN 0  ELSE t.AGT_ISS_AMOUNT END, " +
            "CASE WHEN t.AGT_ISS_GIVEN_DATE IS NULL THEN to_date('01/01/1900', 'DD/MM/RRRR')  ELSE to_date(t.AGT_ISS_GIVEN_DATE, 'DD/MM/RRRR') END, " +
            "CASE WHEN t.AGT_ISS_CLOSE_DATE IS NULL THEN to_date('01/01/1900', 'DD/MM/RRRR')  ELSE to_date(t.AGT_ISS_CLOSE_DATE, 'DD/MM/RRRR') END, " +
            "CASE WHEN t.AGT_RETAINER_STATUS IS NULL THEN 0  ELSE t.AGT_RETAINER_STATUS END, " +
            "CASE WHEN t.AGT_RETAINER_AMOUNT IS NULL THEN 0  ELSE t.AGT_RETAINER_AMOUNT END, " +
            "CASE WHEN t.AGT_RETAINER_GIVEN_DATE IS NULL THEN to_date('01/01/1900', 'DD/MM/RRRR')  ELSE to_date(t.AGT_RETAINER_GIVEN_DATE, 'DD/MM/RRRR') END, " +
            "CASE WHEN t.AGT_RETAINER_CLOSE_DATE IS NULL THEN to_date('01/01/1900', 'DD/MM/RRRR')  ELSE to_date(t.AGT_RETAINER_CLOSE_DATE, 'DD/MM/RRRR') END, " +
            "CASE WHEN t.AGT_ID IS NULL THEN 0  ELSE t.AGT_ID END, " +
            "CASE WHEN t.AGT_CODE IS NULL THEN ''  ELSE t.AGT_CODE END, " +
            "CASE WHEN t.AGT_MDRT_STATUS IS NULL THEN 0  ELSE t.AGT_MDRT_STATUS END, " +
            "CASE WHEN t.AGT_SYSTEM_ID IS NULL THEN ''  ELSE t.AGT_SYSTEM_ID END, " +
            "CASE WHEN t.AGT_SUB_CODE IS NULL THEN ''  ELSE t.AGT_SUB_CODE END, " +
            "CASE WHEN t.AGT_LINE_OF_BUSINESS IS NULL THEN 0  ELSE t.AGT_LINE_OF_BUSINESS END, " +
            "CASE WHEN t.AGT_CHANNEL IS NULL THEN ''  ELSE t.AGT_CHANNEL END, " +
            "CASE WHEN t.AGT_LEVEL IS NULL THEN 0  ELSE t.AGT_LEVEL END, " +
            "CASE WHEN t.AGT_SUPER_CODE IS NULL THEN ''  ELSE t.AGT_SUPER_CODE END, " +
            "CASE WHEN t.AGT_TITLE IS NULL THEN ''  ELSE t.AGT_TITLE END, " +
            "CASE WHEN t.AGT_FIRST_NAME IS NULL THEN ''  ELSE t.AGT_FIRST_NAME END, " +
            "CASE WHEN t.AGT_LAST_NAME IS NULL THEN ''  ELSE t.AGT_LAST_NAME END, " +
            "CASE WHEN t.AGT_ADD1 IS NULL THEN ''  ELSE t.AGT_ADD1 END, " +
            "CASE WHEN t.AGT_ADD2 IS NULL THEN ''  ELSE t.AGT_ADD2 END, " +
            "CASE WHEN t.AGT_ADD3 IS NULL THEN ''  ELSE t.AGT_ADD3 END, " +
            "CASE WHEN t.AGT_NIC_NO IS NULL THEN ''  ELSE t.AGT_NIC_NO END, " +
            "CASE WHEN t.AGT_DOB IS NULL THEN to_date('01/01/1900', 'DD/MM/RRRR')  ELSE to_date(t.AGT_DOB, 'DD/MM/RRRR') END, " +
            "CASE WHEN t.AGT_MOBILE IS NULL THEN ''  ELSE t.AGT_MOBILE END, " +
            "CASE WHEN t.AGT_TEL_NO IS NULL THEN ''  ELSE t.AGT_TEL_NO END, " +
            "CASE WHEN t.AGT_FAX_NO IS NULL THEN ''  ELSE t.AGT_FAX_NO END, " +
            "CASE WHEN t.AGT_BRANCH_CODE IS NULL THEN ''  ELSE t.AGT_BRANCH_CODE END, " +
            "CASE WHEN t.AGT_BANK_ID IS NULL THEN 0  ELSE t.AGT_BANK_ID END, " +
            "CASE WHEN t.AGT_BANK_BRANCH_ID IS NULL THEN 0  ELSE t.AGT_BANK_BRANCH_ID END, " +
            "CASE WHEN t.AGT_BANK_ACC_NO IS NULL THEN ''  ELSE t.AGT_BANK_ACC_NO END, " +
            "CASE WHEN t.AGT_ID_ISSUED IS NULL THEN 0  ELSE t.AGT_ID_ISSUED END, " +
            "CASE WHEN t.AGT_ID_ISSUED_DATE IS NULL THEN to_date('01/01/1900', 'DD/MM/RRRR')  ELSE to_date(t.AGT_ID_ISSUED_DATE, 'DD/MM/RRRR') END, " +
            "CASE WHEN t.AGT_APPOINT_DATE IS NULL THEN to_date('01/01/1900', 'DD/MM/RRRR')  ELSE to_date(t.AGT_APPOINT_DATE, 'DD/MM/RRRR') END, " +
            "CASE WHEN t.AGT_SLII_EXAM IS NULL THEN 0  ELSE t.AGT_SLII_EXAM END, " +
            "CASE WHEN t.AGT_SLII_EXAM_DATE IS NULL THEN to_date('01/01/1900', 'DD/MM/RRRR')  ELSE to_date(t.AGT_SLII_EXAM_DATE, 'DD/MM/RRRR') END, " +
            "CASE WHEN t.AGT_MDRT_YEAR IS NULL THEN 0  ELSE t.AGT_MDRT_YEAR END, " +

            "CASE WHEN t.AGT_AGMT_DATE_RECEIVED IS NULL THEN to_date('01/01/1900', 'DD/MM/RRRR')  ELSE to_date(t.AGT_AGMT_DATE_RECEIVED, 'DD/MM/RRRR') END, " +
            "CASE WHEN t.AGT_AGMT_DATE_ISSUED IS NULL THEN to_date('01/01/1900', 'DD/MM/RRRR')  ELSE to_date(t.AGT_AGMT_DATE_ISSUED, 'DD/MM/RRRR') END, " +
            "CASE WHEN t.AGT_APP_DATE_RECEIVED IS NULL THEN to_date('01/01/1900', 'DD/MM/RRRR')  ELSE to_date(t.AGT_APP_DATE_RECEIVED, 'DD/MM/RRRR') END, " +
            "CASE WHEN t.AGT_APP_DATE_ISSUED IS NULL THEN to_date('01/01/1900', 'DD/MM/RRRR')  ELSE to_date(t.AGT_APP_DATE_ISSUED, 'DD/MM/RRRR') END, " +


            "CASE WHEN t.AGT_TRNS_BRANCH_CODE IS NULL THEN ''  ELSE t.AGT_TRNS_BRANCH_CODE END, " +
            "CASE WHEN t.AGT_TRNS_BRANCH_DATE IS NULL THEN to_date('01/01/1900', 'DD/MM/RRRR')  ELSE to_date(t.AGT_TRNS_BRANCH_DATE, 'DD/MM/RRRR') END, " +
            "CASE WHEN t.AGT_STOP_COMM_DATE IS NULL THEN to_date('01/01/1900', 'DD/MM/RRRR')  ELSE to_date(t.AGT_STOP_COMM_DATE, 'DD/MM/RRRR') END, " +
            "CASE WHEN t.AGT_STOP_COMM_REASON IS NULL THEN ''  ELSE t.AGT_STOP_COMM_REASON END, " +
            "CASE WHEN t.AGT_RELEASE_COMM_DATE IS NULL THEN to_date('01/01/1900', 'DD/MM/RRRR')  ELSE to_date(t.AGT_RELEASE_COMM_DATE, 'DD/MM/RRRR') END, " +
            "CASE WHEN t.AGT_RELEASE_COMM_REASON IS NULL THEN ''  ELSE t.AGT_RELEASE_COMM_REASON END, " +
            "CASE WHEN t.AGT_CUSTOMER_COMPLAIN IS NULL THEN ''  ELSE t.AGT_CUSTOMER_COMPLAIN END, " +
            "CASE WHEN t.AGT_TERMINATE_NOTICE_DATE IS NULL THEN to_date('01/01/1900', 'DD/MM/RRRR')  ELSE to_date(t.AGT_TERMINATE_NOTICE_DATE, 'DD/MM/RRRR') END, " +
            "CASE WHEN t.AGT_TERMINATE_DATE IS NULL THEN to_date('01/01/1900', 'DD/MM/RRRR')  ELSE to_date(t.AGT_TERMINATE_DATE, 'DD/MM/RRRR') END, " +
            "CASE WHEN t.AGT_TERMINATE_REASON IS NULL THEN ''  ELSE t.AGT_TERMINATE_REASON END, " +
            "CASE WHEN t.AGT_BLACK_LISTED_DATE IS NULL THEN to_date('01/01/1900', 'DD/MM/RRRR')  ELSE to_date(t.AGT_BLACK_LISTED_DATE, 'DD/MM/RRRR') END, " +
            "CASE WHEN t.AGT_DUES_TO_COMPANY IS NULL THEN ''  ELSE t.AGT_DUES_TO_COMPANY END, " +
            "CASE WHEN t.AGT_REJOINED_DATE IS NULL THEN to_date('01/01/1900', 'DD/MM/RRRR')  ELSE to_date(t.AGT_REJOINED_DATE, 'DD/MM/RRRR') END, " +
            "CASE WHEN t.AGT_REMARKS IS NULL THEN ''  ELSE t.AGT_REMARKS END ," +
            "CASE WHEN t.AGT_CODE_ID IS NULL THEN 0  ELSE t.AGT_CODE_ID END , " +
            "CASE WHEN t.AGT_TYPE_ID IS NULL THEN 0  ELSE t.AGT_TYPE_ID END , " +
            "CASE WHEN t.AGT_LEADER_AGENT_CODE_V IS NULL THEN ''  ELSE t.AGT_LEADER_AGENT_CODE_V END ," +
            "CASE WHEN t.AGT_LEADER_LEADER_CODE_V IS NULL THEN ''  ELSE t.AGT_LEADER_LEADER_CODE_V END ," +
            "CASE WHEN t.AGT_LEADER_AGENT_CODE_H IS NULL THEN ''  ELSE t.AGT_LEADER_AGENT_CODE_H END ," +
            "CASE WHEN t.AGT_LEADER_LEADER_CODE_H IS NULL THEN ''  ELSE t.AGT_LEADER_LEADER_CODE_H END , " +
            "CASE WHEN t.AGT_CREATED_BY IS NULL THEN ''  ELSE t.AGT_CREATED_BY END ," +
            "CASE WHEN t.AGT_LANGUAGE IS NULL THEN 0  ELSE t.AGT_LANGUAGE END , " +
            "CASE WHEN t.AGT_DESIGNATION_ID IS NULL THEN 0  ELSE t.AGT_DESIGNATION_ID END, " +
            "CASE WHEN t.AGT_HIERARCHY_TYPE_ID IS NULL THEN 0  ELSE t.AGT_HIERARCHY_TYPE_ID END, " +
            "CASE WHEN t.AGT_CHANGE_REASON_ID IS NULL THEN 0  ELSE t.AGT_CHANGE_REASON_ID END, " +
            "CASE WHEN t.AGT_EFFECTIVE_DATE IS NULL THEN to_date('01/01/1900', 'DD/MM/RRRR')  ELSE to_date(t.AGT_EFFECTIVE_DATE, 'DD/MM/RRRR') END, " +
            "CASE WHEN t.AGT_CHANGE_REASON IS NULL THEN ''  ELSE t.AGT_CHANGE_REASON END, " +
            "CASE WHEN t.AGT_EMAIL IS NULL THEN ''  ELSE t.AGT_EMAIL END, " +
            "CASE WHEN t.AGT_LEADER_GIVEN_DATE IS NULL THEN to_date('01/01/1900', 'DD/MM/RRRR')  ELSE to_date(t.AGT_LEADER_GIVEN_DATE, 'DD/MM/RRRR') END " +
            "FROM HCI_TBL_AGENT t  WHERE t.AGT_ID=:vAGT_ID AND t.agt_effective_end_date is null";

            command = new OracleCommand(sql, connection);
            command.Parameters.Add(new OracleParameter("vAGT_ID", id));
            connection.Open();
            try
            {
                dataReader = command.ExecuteReader();
                if (dataReader.HasRows)
                {
                    dataReader.Read();
                    agent.AGT_FULL_NAME = dataReader[0].ToString();
                    agent.AGT_FULL_ADDRESS = dataReader[1].ToString();
                    agent.AGT_BUSINESS_TYPE = Convert.ToInt16(dataReader[2]);
                    agent.AGT_LEVEL_CODE = dataReader[3].ToString();
                    agent.AGT_LEADER_CODE = dataReader[4].ToString();
                    agent.AGT_STATUS = Convert.ToInt16(dataReader[5]);
                    agent.AGT_TERMINATE_STATUS = Convert.ToInt32(dataReader[6]);
                    agent.AGT_STOP_COMM_STATUS = Convert.ToInt32(dataReader[7]);
                    agent.AGT_ISS_STATUS = Convert.ToInt32(dataReader[8]);
                    agent.AGT_ISS_AMOUNT = Convert.ToDouble(dataReader[9]);
                    agent.AGT_ISS_GIVEN_DATE = dataReader[10].ToString();
                    agent.AGT_ISS_CLOSE_DATE = dataReader[11].ToString();
                    agent.AGT_RETAINER_STATUS = dataReader[12].ToString();
                    agent.AGT_RETAINER_AMOUNT = Convert.ToDouble(dataReader[13]);
                    agent.AGT_RETAINER_GIVEN_DATE = dataReader[14].ToString();
                    agent.AGT_RETAINER_CLOSE_DATE = dataReader[15].ToString();
                    agent.AGT_ID = dataReader[16].ToString();
                    agent.AGT_CODE = dataReader[17].ToString();
                    agent.AGT_MDRT_STATUS = Convert.ToInt32(dataReader[18]);
                    agent.AGT_SYSTEM_ID = dataReader[19].ToString();
                    agent.AGT_SUB_CODE = dataReader[20].ToString();
                    agent.AGT_LINE_OF_BUSINESS = Convert.ToInt32(dataReader[21]);
                    agent.AGT_CHANNEL = dataReader[22].ToString();
                    agent.AGT_LEVEL = Convert.ToInt32(dataReader[23]);
                    agent.AGT_SUPER_CODE = dataReader[24].ToString();
                    agent.AGT_TITLE = dataReader[25].ToString();
                    agent.AGT_FIRST_NAME = dataReader[26].ToString();
                    agent.AGT_LAST_NAME = dataReader[27].ToString();
                    agent.AGT_ADD1 = dataReader[28].ToString();
                    agent.AGT_ADD2 = dataReader[29].ToString();
                    agent.AGT_ADD3 = dataReader[30].ToString();
                    agent.AGT_NIC_NO = dataReader[31].ToString();
                    agent.AGT_DOB = dataReader[32].ToString();
                    agent.AGT_MOBILE = dataReader[33].ToString();
                    agent.AGT_TEL_NO = dataReader[34].ToString();
                    agent.AGT_FAX_NO = dataReader[35].ToString();
                    agent.AGT_BRANCH_CODE = dataReader[36].ToString();
                    agent.AGT_BANK_ID = Convert.ToInt32(dataReader[37]);
                    agent.AGT_BANK_BRANCH_ID = Convert.ToInt32(dataReader[38]);
                    agent.AGT_BANK_ACC_NO = dataReader[39].ToString();
                    agent.AGT_ID_ISSUED = Convert.ToInt32(dataReader[40]);
                    agent.AGT_ID_ISSUED_DATE = dataReader[41].ToString();
                    agent.AGT_APPOINT_DATE = dataReader[42].ToString();
                    agent.AGT_SLII_EXAM = Convert.ToInt32(dataReader[43]);
                    agent.AGT_SLII_EXAM_DATE = dataReader[44].ToString();
                    agent.AGT_MDRT_YEAR = Convert.ToInt32(dataReader[45]);

                    agent.AGT_AGMT_DATE_RECEIVED = dataReader[46].ToString();
                    agent.AGT_AGMT_DATE_ISSUED = dataReader[47].ToString();
                    agent.AGT_APP_DATE_RECEIVED  = dataReader[48].ToString();
                    agent.AGT_APP_DATE_ISSUED = dataReader[49].ToString();

                    agent.AGT_TRNS_BRANCH_CODE = dataReader[50].ToString();
                    agent.AGT_TRNS_BRANCH_DATE = dataReader[51].ToString();
                    agent.AGT_STOP_COMM_DATE = dataReader[52].ToString();
                    agent.AGT_STOP_COMM_REASON = dataReader[53].ToString();
                    agent.AGT_RELEASE_COMM_DATE = dataReader[54].ToString();
                    agent.AGT_RELEASE_COMM_REASON = dataReader[55].ToString();
                    agent.AGT_CUSTOMER_COMPLAIN = dataReader[56].ToString();
                    agent.AGT_TERMINATE_NOTICE_DATE = dataReader[57].ToString();
                    agent.AGT_TERMINATE_DATE = dataReader[58].ToString();
                    agent.AGT_TERMINATE_REASON = dataReader[59].ToString();
                    agent.AGT_BLACK_LISTED_DATE = dataReader[60].ToString();
                    agent.AGT_DUES_TO_COMPANY = dataReader[61].ToString();
                    agent.AGT_REJOINED_DATE = dataReader[62].ToString();
                    agent.AGT_REMARKS = dataReader[63].ToString();
                    agent.AGT_CODE_ID = Convert.ToInt32(dataReader[64]);
                    agent.AGT_TYPE_ID = Convert.ToInt32(dataReader[65]);
                    agent.AGT_LEADER_AGENT_CODE_V = dataReader[66].ToString();
                    agent.AGT_LEADER_LEADER_CODE_V = dataReader[67].ToString();
                    agent.AGT_LEADER_AGENT_CODE_H = dataReader[68].ToString();
                    agent.AGT_LEADER_LEADER_CODE_H = dataReader[69].ToString();
                    agent.AGT_CREATED_BY = dataReader[70].ToString();
                    agent.AGT_LANGUAGE = Convert.ToInt32(dataReader[71].ToString());
                    agent.AGT_DESIGNATION_ID = Convert.ToInt32(dataReader[72].ToString());
                    agent.AGT_HIERARCHY_TYPE_ID = Convert.ToInt32(dataReader[73].ToString());
                    agent.AGT_CHANGE_REASON_ID = Convert.ToInt32(dataReader[74].ToString());
                    agent.AGT_EFFECTIVE_DATE = dataReader[75].ToString();
                    agent.AGT_CHANGE_REASON = dataReader[76].ToString();
                    agent.AGT_EMAIL = dataReader[77].ToString();
                    agent.AGT_LEADER_GIVEN_DATE = dataReader[78].ToString();

                    dataReader.Close();
                    connection.Close();
                }
                else
                {
                    return null;
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
            return agent;
        }

        public bool AcceptAllCertifications(object sender, System.Security.Cryptography.X509Certificates.X509Certificate certification, System.Security.Cryptography.X509Certificates.X509Chain chain, System.Net.Security.SslPolicyErrors sslPolicyErrors)
        {
            return true;
        }


        // POST: api/Quotation
        [HttpPost]
        [ActionName("AddAgent")]
        public string AddAgent(Agent Agent)
        {
            try
            {

                OracleConnection connection = new OracleConnection(ConnectionString);
                OracleCommand command;

                string returnVal = "";
                connection.Open();
                OracleCommand cmd = null;

                cmd = new OracleCommand("HCI_SP_AGENT");

                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Connection = connection;

                if (Agent.AGT_ID == "")
                {
                    cmd.Parameters.Add("vAGT_ID", OracleType.VarChar).Value = "NEW";
                }
                else
                {
                    cmd.Parameters.Add("vAGT_ID", OracleType.VarChar).Value = Agent.AGT_ID;
                }

                cmd.Parameters.Add("vAGT_CODE", OracleType.VarChar).Value = Agent.AGT_CODE;

                cmd.Parameters.Add("vAGT_CODE_ID", OracleType.Number).Value = Agent.AGT_CODE_ID;
                cmd.Parameters.Add("vAGT_TYPE_ID", OracleType.Number).Value = Agent.AGT_TYPE_ID;
                //Tab 1
                cmd.Parameters.Add("vAGT_MDRT_STATUS", OracleType.Number).Value = Agent.AGT_MDRT_STATUS;
                cmd.Parameters.Add("vAGT_MDRT_YEAR", OracleType.Number).Value = Agent.AGT_MDRT_YEAR;
                cmd.Parameters.Add("vAGT_SYSTEM_ID", OracleType.VarChar).Value = Agent.AGT_SYSTEM_ID;
                cmd.Parameters.Add("vAGT_SUB_CODE", OracleType.VarChar).Value = Agent.AGT_SUB_CODE;
                cmd.Parameters.Add("vAGT_LINE_OF_BUSINESS", OracleType.Number).Value = Agent.AGT_LINE_OF_BUSINESS;
                cmd.Parameters.Add("vAGT_CHANNEL", OracleType.VarChar).Value = Agent.AGT_CHANNEL;
                cmd.Parameters.Add("vAGT_LEVEL", OracleType.Number).Value = Agent.AGT_LEVEL;
                cmd.Parameters.Add("vAGT_LANGUAGE", OracleType.Number).Value = Agent.AGT_LANGUAGE;
                cmd.Parameters.Add("vAGT_SUPER_CODE", OracleType.VarChar).Value = Agent.AGT_SUPER_CODE;
                cmd.Parameters.Add("vAGT_TITLE", OracleType.VarChar).Value = Agent.AGT_TITLE;
                cmd.Parameters.Add("vAGT_FIRST_NAME", OracleType.VarChar).Value = Agent.AGT_FIRST_NAME;
                cmd.Parameters.Add("vAGT_LAST_NAME", OracleType.VarChar).Value = Agent.AGT_LAST_NAME;
                cmd.Parameters.Add("vAGT_ADD1", OracleType.VarChar).Value = Agent.AGT_ADD1;
                cmd.Parameters.Add("vAGT_ADD2", OracleType.VarChar).Value = Agent.AGT_ADD2;
                cmd.Parameters.Add("vAGT_ADD3", OracleType.VarChar).Value = Agent.AGT_ADD3;
                cmd.Parameters.Add("vAGT_NIC_NO", OracleType.VarChar).Value = Agent.AGT_NIC_NO;
                cmd.Parameters.Add("vAGT_DOB", OracleType.DateTime).Value = Agent.AGT_DOB;
                cmd.Parameters.Add("vAGT_MOBILE", OracleType.VarChar).Value = Agent.AGT_MOBILE;
                cmd.Parameters.Add("vAGT_TEL_NO", OracleType.VarChar).Value = Agent.AGT_TEL_NO;
                cmd.Parameters.Add("vAGT_FAX_NO", OracleType.VarChar).Value = Agent.AGT_FAX_NO;
                cmd.Parameters.Add("vAGT_EMAIL", OracleType.VarChar).Value = Agent.AGT_EMAIL;
                cmd.Parameters.Add("vAGT_BRANCH_CODE", OracleType.VarChar).Value = Agent.AGT_BRANCH_CODE;
                cmd.Parameters.Add("vAGT_BANK_ID", OracleType.Number).Value = Agent.AGT_BANK_ID;
                cmd.Parameters.Add("vAGT_BANK_BRANCH_ID", OracleType.Number).Value = Agent.AGT_BANK_BRANCH_ID;
                cmd.Parameters.Add("vAGT_BANK_ACC_NO", OracleType.VarChar).Value = Agent.AGT_BANK_ACC_NO;
                cmd.Parameters.Add("vAGT_ID_ISSUED", OracleType.Number).Value = Agent.AGT_ID_ISSUED;
                cmd.Parameters.Add("vAGT_ID_ISSUED_DATE", OracleType.DateTime).Value = Agent.AGT_ID_ISSUED_DATE;
                cmd.Parameters.Add("vAGT_APPOINT_DATE", OracleType.DateTime).Value = Agent.AGT_APPOINT_DATE;
                cmd.Parameters.Add("vAGT_SLII_EXAM", OracleType.Number).Value = Agent.AGT_SLII_EXAM;
                cmd.Parameters.Add("vAGT_SLII_EXAM_DATE", OracleType.DateTime).Value = Agent.AGT_SLII_EXAM_DATE;

                //Tab 2
                cmd.Parameters.Add("vAGT_AGMT_DATE_RECEIVED", OracleType.DateTime).Value = Agent.AGT_AGMT_DATE_RECEIVED;
                cmd.Parameters.Add("vAGT_AGMT_DATE_ISSUED", OracleType.DateTime).Value = Agent.AGT_AGMT_DATE_ISSUED;
                cmd.Parameters.Add("vAGT_APP_DATE_RECEIVED", OracleType.DateTime).Value = Agent.AGT_APP_DATE_RECEIVED;
                cmd.Parameters.Add("vAGT_APP_DATE_ISSUED", OracleType.DateTime).Value = Agent.AGT_APP_DATE_ISSUED;

                cmd.Parameters.Add("vAGT_TRNS_BRANCH_CODE", OracleType.VarChar).Value = Agent.AGT_TRNS_BRANCH_CODE;
                cmd.Parameters.Add("vAGT_TRNS_BRANCH_DATE", OracleType.DateTime).Value = Agent.AGT_TRNS_BRANCH_DATE;
                cmd.Parameters.Add("vAGT_STOP_COMM_DATE", OracleType.DateTime).Value = Agent.AGT_STOP_COMM_DATE;
                cmd.Parameters.Add("vAGT_STOP_COMM_REASON", OracleType.VarChar).Value = Agent.AGT_STOP_COMM_REASON;
                cmd.Parameters.Add("vAGT_RELEASE_COMM_DATE", OracleType.DateTime).Value = Agent.AGT_RELEASE_COMM_DATE;
                cmd.Parameters.Add("vAGT_RELEASE_COMM_REASON", OracleType.VarChar).Value = Agent.AGT_RELEASE_COMM_REASON;
                cmd.Parameters.Add("vAGT_CUSTOMER_COMPLAIN", OracleType.VarChar).Value = Agent.AGT_CUSTOMER_COMPLAIN;
                cmd.Parameters.Add("vAGT_TERMINATE_NOTICE_DATE", OracleType.DateTime).Value = Agent.AGT_TERMINATE_NOTICE_DATE;
                cmd.Parameters.Add("vAGT_TERMINATE_DATE", OracleType.DateTime).Value = Agent.AGT_TERMINATE_DATE;
                cmd.Parameters.Add("vAGT_TERMINATE_REASON", OracleType.VarChar).Value = Agent.AGT_TERMINATE_REASON;
                cmd.Parameters.Add("vAGT_BLACK_LISTED_DATE", OracleType.DateTime).Value = Agent.AGT_BLACK_LISTED_DATE;
                cmd.Parameters.Add("vAGT_DUES_TO_COMPANY", OracleType.VarChar).Value = Agent.AGT_DUES_TO_COMPANY;
                cmd.Parameters.Add("vAGT_REJOINED_DATE", OracleType.DateTime).Value = Agent.AGT_REJOINED_DATE;
                cmd.Parameters.Add("vAGT_REMARKS", OracleType.VarChar).Value = Agent.AGT_REMARKS;

                cmd.Parameters.Add("vAGT_BUSINESS_TYPE", OracleType.Number).Value = Agent.AGT_BUSINESS_TYPE;
                cmd.Parameters.Add("vAGT_LEVEL_CODE", OracleType.VarChar).Value = Agent.AGT_LEVEL_CODE;
                cmd.Parameters.Add("vAGT_LEADER_CODE", OracleType.VarChar).Value = Agent.AGT_LEADER_CODE;
                cmd.Parameters.Add("vAGT_STATUS", OracleType.Number).Value = Agent.AGT_STATUS   ;
                cmd.Parameters.Add("vAGT_TERMINATE_STATUS", OracleType.Number).Value = Agent.AGT_TERMINATE_STATUS;
                cmd.Parameters.Add("vAGT_STOP_COMM_STATUS", OracleType.Number).Value = Agent.AGT_STOP_COMM_STATUS;
                cmd.Parameters.Add("vAGT_ISS_STATUS", OracleType.Number).Value = Agent.AGT_ISS_STATUS;
                cmd.Parameters.Add("vAGT_ISS_AMOUNT", OracleType.Number).Value = Agent.AGT_ISS_AMOUNT;
                cmd.Parameters.Add("vAGT_ISS_GIVEN_DATE", OracleType.DateTime).Value = Agent.AGT_ISS_GIVEN_DATE;
                cmd.Parameters.Add("vAGT_ISS_CLOSE_DATE", OracleType.DateTime).Value = Agent.AGT_ISS_CLOSE_DATE;
                cmd.Parameters.Add("vAGT_RETAINER_STATUS", OracleType.Number).Value = Agent.AGT_RETAINER_STATUS;
                cmd.Parameters.Add("vAGT_RETAINER_AMOUNT", OracleType.Number).Value = Agent.AGT_RETAINER_AMOUNT;
                cmd.Parameters.Add("vAGT_RETAINER_GIVEN_DATE", OracleType.DateTime).Value = Agent.AGT_RETAINER_GIVEN_DATE;
                cmd.Parameters.Add("vAGT_RETAINER_CLOSE_DATE", OracleType.DateTime).Value = Agent.AGT_RETAINER_CLOSE_DATE;

                cmd.Parameters.Add("vAGT_LEADER_AGENT_CODE_V", OracleType.VarChar).Value = Agent.AGT_LEADER_AGENT_CODE_V;
                cmd.Parameters.Add("vAGT_LEADER_LEADER_CODE_V", OracleType.VarChar).Value = Agent.AGT_LEADER_LEADER_CODE_V;
                cmd.Parameters.Add("vAGT_LEADER_AGENT_CODE_H", OracleType.VarChar).Value = Agent.AGT_LEADER_AGENT_CODE_H;
                cmd.Parameters.Add("vAGT_LEADER_LEADER_CODE_H", OracleType.VarChar).Value = Agent.AGT_LEADER_LEADER_CODE_H;
                cmd.Parameters.Add("vAGT_CREATED_BY", OracleType.VarChar).Value = Agent.AGT_CREATED_BY;

                cmd.Parameters.Add("vAGT_DESIGNATION_ID", OracleType.Number).Value = Agent.AGT_DESIGNATION_ID;
                cmd.Parameters.Add("vAGT_HIERARCHY_TYPE_ID", OracleType.Number).Value = Agent.AGT_HIERARCHY_TYPE_ID;
                cmd.Parameters.Add("vAGT_CHANGE_REASON_ID", OracleType.Number).Value = Agent.AGT_CHANGE_REASON_ID;
                cmd.Parameters.Add("vAGT_EFFECTIVE_DATE", OracleType.DateTime).Value = Agent.AGT_EFFECTIVE_DATE;
                cmd.Parameters.Add("vAGT_CHANGE_REASON", OracleType.VarChar).Value = Agent.AGT_CHANGE_REASON;
                cmd.Parameters.Add("vAGT_IS_AGENT_ATTACHED_CHANGED", OracleType.VarChar).Value = Agent.AGT_IS_AGENT_ATTACHED_CHANGED;
                cmd.Parameters.Add("vAGT_LEADER_GIVEN_DATE", OracleType.DateTime).Value = Agent.AGT_LEADER_GIVEN_DATE;



                cmd.Parameters.Add("vAGT_AGENT_CODE_GENERATED", OracleType.VarChar, 20).Direction = ParameterDirection.Output;
                cmd.Parameters.Add("vAGT_LEVEL_CODE_GENERATED", OracleType.VarChar, 20).Direction = ParameterDirection.Output;


                cmd.ExecuteNonQuery();

                string vAGT_AGENT_CODE_GENERATED = cmd.Parameters["vAGT_AGENT_CODE_GENERATED"].Value.ToString();
                string vAGT_LEVEL_CODE_GENERATED = cmd.Parameters["vAGT_LEVEL_CODE_GENERATED"].Value.ToString();

                connection.Close();

                String Codes = vAGT_AGENT_CODE_GENERATED + "|" + vAGT_LEVEL_CODE_GENERATED + "||";

                return Codes +"Successfully Saved";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

    }
}
