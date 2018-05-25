using ComissionWebAPI.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.OracleClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;


namespace ComissionWebAPI.Controllers
{
    public class CommissionRateChartController : ApiController
    {

        static string ConnectionString = ConfigurationManager.ConnectionStrings["OracleConString"].ToString();[HttpGet]



        [HttpPost]
        [ActionName("saveCommissionRateChart")]
        public HttpResponseMessage saveCommissionRateChart(CommissionRateChart obj)
        {
            OracleConnection connection = new OracleConnection(ConnectionString);
            OracleCommand command;
            try
            {
                connection.Open();
                command = new OracleCommand("HCI_SP_COM_RATE_INSERT");
                command.CommandType = CommandType.StoredProcedure;
                command.Connection = connection;

                command.Parameters.Add("V_ID", OracleType.Number).Value = obj.Id;
                command.Parameters.Add("V_CATEGORY_ID", OracleType.Number).Value = obj.CategoryId;
                command.Parameters.Add("V_AGT_TYPE_ID", OracleType.Number).Value = obj.AgtTypeId;
                command.Parameters.Add("V_CODE", OracleType.VarChar).Value = obj.Code;
                command.Parameters.Add("V_DESCRIPTION", OracleType.VarChar).Value = obj.Description;
                command.Parameters.Add("V_TERM_LOWER_LIMIT", OracleType.Number).Value = obj.TermLowerLimit;
                command.Parameters.Add("V_TERM_UPPER_LIMIT", OracleType.Number).Value = obj.TermUpperLimit;
                command.Parameters.Add("V_YEAR_LOWER_LIMIT", OracleType.Number).Value = obj.YearLowerLimit;
                command.Parameters.Add("V_YEAR_UPPER_LIMIT", OracleType.Number).Value = obj.YearUpperLimit;
                command.Parameters.Add("V_CREATED_BY", OracleType.VarChar).Value = obj.CreatedBy;
                command.Parameters.Add("V_ACTIVE_STATUS", OracleType.Number).Value = obj.ActiveStatus;
                command.Parameters.Add("V_RATE", OracleType.Number).Value = obj.Rate;
                command.Parameters.Add("V_SQL", OracleType.Number).Value = obj.Rate;
                command.Parameters.Add("V_FROM_DATE", OracleType.VarChar).Value = obj.FromDate;
                command.Parameters.Add("V_TO_DATE", OracleType.VarChar).Value = obj.ToDate;

                command.ExecuteNonQuery();
                connection.Close();
                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception exception)
            {
                connection.Close();
                return Request.CreateResponse(HttpStatusCode.ExpectationFailed);
            }
        }



        public IEnumerable<CommissionRateChart> Get()
        {
            List<CommissionRateChart> commissionRateChartList = new List<CommissionRateChart>();
            DataTable dataTable = new DataTable();
            OracleDataReader dataReader = null;
            OracleConnection connection = new OracleConnection(ConnectionString);
            OracleCommand command;
            string sql = "SELECT " +
            "CASE WHEN t.ID IS NULL THEN 0  ELSE t.ID END, " +
            "CASE WHEN t.CATEGORY_ID IS NULL THEN 0  ELSE t.CATEGORY_ID END,  " +
            "CASE WHEN t.AGT_TYPE_ID IS NULL THEN 0  ELSE t.AGT_TYPE_ID END,  " +
            "CASE WHEN t.CODE IS NULL THEN ''  ELSE t.CODE END, " +
            "CASE WHEN t.DESCRIPTION IS NULL THEN ''  ELSE t.DESCRIPTION END, " +
            "CASE WHEN t.TERM_LOWER_LIMIT IS NULL THEN 0  ELSE t.TERM_LOWER_LIMIT END, " +
            "CASE WHEN t.TERM_UPPER_LIMIT IS NULL THEN 0  ELSE t.TERM_UPPER_LIMIT END, " +
            "CASE WHEN t.YEAR_LOWER_LIMIT IS NULL THEN 0  ELSE t.YEAR_LOWER_LIMIT END, " +
            "CASE WHEN t.YEAR_UPPER_LIMIT IS NULL THEN 0  ELSE t.YEAR_UPPER_LIMIT END, " +
            "CASE WHEN t.CREATED_BY IS NULL THEN ''  ELSE t.CREATED_BY END, " +
            "CASE WHEN t.CREATED_DATE IS NULL THEN to_date('01/01/1900', 'DD/MM/RRRR')  ELSE to_date(t.CREATED_DATE, 'DD/MM/RRRR') END, " +
            "CASE WHEN t.EFFECTIVE_END_DATE IS NULL THEN to_date('01/01/1900', 'DD/MM/RRRR')  ELSE to_date(t.EFFECTIVE_END_DATE, 'DD/MM/RRRR') END, " +
            "CASE WHEN t.ACTIVE_STATUS IS NULL THEN 0  ELSE t.ACTIVE_STATUS END, " +
            "CASE WHEN t.RATE IS NULL THEN 0  ELSE t.RATE END, " +
            "CASE WHEN t.SQL IS NULL THEN ''  ELSE t.SQL END, " +
            "PT.DESCRIPTION AS PRODUCT_CATEGORY, " +
            "AT.DESCRIPTION AS AGENT_TYPE, " +
            "CASE WHEN t.FROM_DATE IS NULL THEN to_date('01/01/1900', 'DD/MM/RRRR')  ELSE to_date(t.FROM_DATE, 'DD/MM/RRRR') END, " +
            "CASE WHEN t.TO_DATE IS NULL THEN to_date('01/01/1900', 'DD/MM/RRRR')  ELSE to_date(t.TO_DATE, 'DD/MM/RRRR') END " +
            "FROM HCI_TBL_COM_RATE_CHART t , HCI_TBL_PRODUCT_CATEGORY PT, HCI_TBL_AGENT_TYPE AT " +
            "WHERE PT.ID = T.CATEGORY_ID AND AT.ID = T.AGT_TYPE_ID AND T.EFFECTIVE_END_DATE IS NULL";

            command = new OracleCommand(sql, connection);
            try
            {
                connection.Open();
                dataReader = command.ExecuteReader();
                dataTable.Load(dataReader);
                dataReader.Close();
                connection.Close();
                commissionRateChartList = (from DataRow drow in dataTable.Rows
                                           select new CommissionRateChart()
                                           {
                                               Id = Convert.ToInt32(drow[0]),
                                               CategoryId = Convert.ToInt32(drow[1]),
                                               AgtTypeId = Convert.ToInt32(drow[2]),
                                               Code = drow[3].ToString(),
                                               Description = drow[4].ToString(),
                                               TermLowerLimit = Convert.ToInt32(drow[5]),
                                               TermUpperLimit = Convert.ToInt32(drow[6]),
                                               YearLowerLimit = Convert.ToInt32(drow[7]),
                                               YearUpperLimit = Convert.ToInt32(drow[8]),
                                               CreatedBy = drow[9].ToString(),
                                               CreatedDate = drow[10].ToString(),
                                               EffectiveEndDate = drow[11].ToString(),
                                               ActiveStatus = Convert.ToInt32(drow[12]),
                                               Rate = Convert.ToDouble(drow[13]),
                                               Sql = drow[14].ToString(),
                                               Product_Category = drow[15].ToString(),
                                               Agent_Type = drow[16].ToString(),
                                               FromDate = drow[17].ToString(),
                                               ToDate = drow[18].ToString(),
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
            return commissionRateChartList;
        }

        [HttpGet]
        public CommissionRateChart Get(int id)
        {
            CommissionRateChart commissionRateChart = new CommissionRateChart();
            OracleDataReader dataReader = null;
            OracleConnection connection = new OracleConnection(ConnectionString);
            OracleCommand command;
            string sql = "SELECT " +
            "CASE WHEN t.ID IS NULL THEN 0  ELSE t.ID END, " +
            "CASE WHEN t.CATEGORY_ID IS NULL THEN 0  ELSE t.CATEGORY_ID END, " +
            "CASE WHEN t.AGT_TYPE_ID IS NULL THEN 0  ELSE t.AGT_TYPE_ID END, " +
            "CASE WHEN t.CODE IS NULL THEN ''  ELSE t.CODE END, " +
            "CASE WHEN t.DESCRIPTION IS NULL THEN ''  ELSE t.DESCRIPTION END, " +
            "CASE WHEN t.TERM_LOWER_LIMIT IS NULL THEN 0  ELSE t.TERM_LOWER_LIMIT END, " +
            "CASE WHEN t.TERM_UPPER_LIMIT IS NULL THEN 0  ELSE t.TERM_UPPER_LIMIT END, " +
            "CASE WHEN t.YEAR_LOWER_LIMIT IS NULL THEN 0  ELSE t.YEAR_LOWER_LIMIT END, " +
            "CASE WHEN t.YEAR_UPPER_LIMIT IS NULL THEN 0  ELSE t.YEAR_UPPER_LIMIT END, " +
            "CASE WHEN t.CREATED_BY IS NULL THEN ''  ELSE t.CREATED_BY END, " +
            "CASE WHEN t.CREATED_DATE IS NULL THEN to_date('01/01/1900', 'DD/MM/RRRR')  ELSE to_date(t.CREATED_DATE, 'DD/MM/RRRR') END, " +
            "CASE WHEN t.EFFECTIVE_END_DATE IS NULL THEN to_date('01/01/1900', 'DD/MM/RRRR')  ELSE to_date(t.EFFECTIVE_END_DATE, 'DD/MM/RRRR') END, " +
            "CASE WHEN t.ACTIVE_STATUS IS NULL THEN 0  ELSE t.ACTIVE_STATUS END, " +
            "CASE WHEN t.RATE IS NULL THEN 0  ELSE t.RATE END, " +
            "CASE WHEN t.SQL IS NULL THEN ''  ELSE t.SQL END, " +
            "CASE WHEN t.FROM_DATE IS NULL THEN to_date('01/01/1900', 'DD/MM/RRRR')  ELSE to_date(t.FROM_DATE, 'DD/MM/RRRR') END as FROM_DATE, " +
            "CASE WHEN t.TO_DATE IS NULL THEN to_date('01/01/1900', 'DD/MM/RRRR')  ELSE to_date(t.TO_DATE, 'DD/MM/RRRR') END as TO_DATE " +
            "FROM HCI_TBL_COM_RATE_CHART t  WHERE t.ID=:V_ID";
            command = new OracleCommand(sql, connection);
            command.Parameters.Add(new OracleParameter("V_ID", id));
            connection.Open();
            try
            {
                dataReader = command.ExecuteReader();
                if (dataReader.HasRows)
                {
                    dataReader.Read();
                    commissionRateChart.Id = Convert.ToInt32(dataReader[0]);
                    commissionRateChart.CategoryId = Convert.ToInt32(dataReader[1]);
                    commissionRateChart.AgtTypeId = Convert.ToInt32(dataReader[2]);
                    commissionRateChart.Code = dataReader[3].ToString();
                    commissionRateChart.Description = dataReader[4].ToString();
                    commissionRateChart.TermLowerLimit = Convert.ToInt32(dataReader[5]);
                    commissionRateChart.TermUpperLimit = Convert.ToInt32(dataReader[6]);
                    commissionRateChart.YearLowerLimit = Convert.ToInt32(dataReader[7]);
                    commissionRateChart.YearUpperLimit = Convert.ToInt32(dataReader[8]);
                    commissionRateChart.CreatedBy = dataReader[9].ToString();
                    commissionRateChart.CreatedDate = dataReader[10].ToString();
                    commissionRateChart.EffectiveEndDate = dataReader[11].ToString();
                    commissionRateChart.ActiveStatus = Convert.ToInt32(dataReader[12]);
                    commissionRateChart.Rate = Convert.ToDouble(dataReader[13]);
                    commissionRateChart.Sql = dataReader[14].ToString();
                    commissionRateChart.FromDate = dataReader[15].ToString();
                    commissionRateChart.ToDate = dataReader[16].ToString();

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
            return commissionRateChart;
        }



    }
}
