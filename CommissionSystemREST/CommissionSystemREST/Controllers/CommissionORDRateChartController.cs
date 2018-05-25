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
    public class CommissionORDRateChartController : ApiController
    {

        static string ConnectionString = ConfigurationManager.ConnectionStrings["OracleConString"].ToString();[HttpGet]
        public IEnumerable<CommissionORDRateChart> Get()
        {
            List<CommissionORDRateChart> commissionORDRateChartList = new List<CommissionORDRateChart>();
            DataTable dataTable = new DataTable();
            OracleDataReader dataReader = null;
            OracleConnection connection = new OracleConnection(ConnectionString);
            OracleCommand command;
            string sql = "SELECT " +
            "CASE WHEN t.ID IS NULL THEN 0  ELSE t.ID END, " +
            "CASE WHEN t.CODE IS NULL THEN ''  ELSE t.CODE END, " +
            "CASE WHEN t.DESCRIPTION IS NULL THEN ''  ELSE t.DESCRIPTION END, " +
            "CASE WHEN t.AGT_TYPE_ID IS NULL THEN 0  ELSE t.AGT_TYPE_ID END, " +
            "CASE WHEN t.COM_LEVEL_ID IS NULL THEN 0  ELSE t.COM_LEVEL_ID END, " +
            "CASE WHEN t.RATE IS NULL THEN 0  ELSE t.RATE END, " +
            "CASE WHEN t.FROM_DATE IS NULL THEN to_date('01/01/1900', 'DD/MM/RRRR')  ELSE to_date(t.FROM_DATE, 'DD/MM/RRRR') END, " +
            "CASE WHEN t.TO_DATE IS NULL THEN to_date('01/01/1900', 'DD/MM/RRRR')  ELSE to_date(t.TO_DATE, 'DD/MM/RRRR') END, " +
            "CASE WHEN t.SQL IS NULL THEN ''  ELSE t.SQL END, " +
            "CASE WHEN t.CREATED_BY IS NULL THEN ''  ELSE t.CREATED_BY END, " +
            "CASE WHEN t.CREATED_DATE IS NULL THEN to_date('01/01/1900', 'DD/MM/RRRR')  ELSE to_date(t.CREATED_DATE, 'DD/MM/RRRR') END, " +
            "CASE WHEN t.EFFECTIVE_END_DATE IS NULL THEN to_date('01/01/1900', 'DD/MM/RRRR')  ELSE to_date(t.EFFECTIVE_END_DATE, 'DD/MM/RRRR') END, " +
            "CASE WHEN t.ACTIVE_STATUS IS NULL THEN 0  ELSE t.ACTIVE_STATUS END ," +
            "CASE WHEN t.COM_YEAR_ID IS NULL THEN 0  ELSE t.COM_YEAR_ID END " +
            " FROM HCI_TBL_COM_ORD_RATE_CHART t WHERE t.EFFECTIVE_END_DATE IS NULL";
            command = new OracleCommand(sql, connection);
            try
            {
                connection.Open();
                dataReader = command.ExecuteReader();
                dataTable.Load(dataReader);
                dataReader.Close();
                connection.Close();
                commissionORDRateChartList = (from DataRow drow in dataTable.Rows
                                              select new CommissionORDRateChart()
                                              {
                                                  Id = Convert.ToInt32(drow[0]),
                                                  Code = drow[1].ToString(),
                                                  Description = drow[2].ToString(),
                                                  AgtTypeId = Convert.ToInt32(drow[3]),
                                                  ComLevelId = Convert.ToInt32(drow[4]), 
                                                  Rate = Convert.ToInt32(drow[5]),
                                                  FromDate = drow[6].ToString(),
                                                  ToDate = drow[7].ToString(),
                                                  Sql = drow[8].ToString(),
                                                  CreatedBy = drow[9].ToString(),
                                                  CreatedDate = drow[10].ToString(),
                                                  EffectiveEndDate = drow[11].ToString(),
                                                  ActiveStatus = Convert.ToInt32(drow[12]),
                                                  ComYearId = Convert.ToInt32(drow[13]),
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
            return commissionORDRateChartList;
        }
        [HttpGet]
        public CommissionORDRateChart Get(int id)
        {
            CommissionORDRateChart commissionORDRateChart = new CommissionORDRateChart();
            OracleDataReader dataReader = null;
            OracleConnection connection = new OracleConnection(ConnectionString);
            OracleCommand command;
            string sql = "SELECT " +
            "CASE WHEN t.ID IS NULL THEN 0  ELSE t.ID END, " +
            "CASE WHEN t.CODE IS NULL THEN ''  ELSE t.CODE END, " +
            "CASE WHEN t.DESCRIPTION IS NULL THEN ''  ELSE t.DESCRIPTION END, " +
            "CASE WHEN t.AGT_TYPE_ID IS NULL THEN 0  ELSE t.AGT_TYPE_ID END, " +
            "CASE WHEN t.COM_LEVEL_ID IS NULL THEN 0  ELSE t.COM_LEVEL_ID END, " +
            "CASE WHEN t.RATE IS NULL THEN 0  ELSE t.RATE END, " +
            "CASE WHEN t.FROM_DATE IS NULL THEN to_date('01/01/1900', 'DD/MM/RRRR')  ELSE to_date(t.FROM_DATE, 'DD/MM/RRRR') END, " +
            "CASE WHEN t.TO_DATE IS NULL THEN to_date('01/01/1900', 'DD/MM/RRRR')  ELSE to_date(t.TO_DATE, 'DD/MM/RRRR') END, " +
            "CASE WHEN t.SQL IS NULL THEN ''  ELSE t.SQL END, " +
            "CASE WHEN t.CREATED_BY IS NULL THEN ''  ELSE t.CREATED_BY END, " +
            "CASE WHEN t.CREATED_DATE IS NULL THEN to_date('01/01/1900', 'DD/MM/RRRR')  ELSE to_date(t.CREATED_DATE, 'DD/MM/RRRR') END, " +
            "CASE WHEN t.EFFECTIVE_END_DATE IS NULL THEN to_date('01/01/1900', 'DD/MM/RRRR')  ELSE to_date(t.EFFECTIVE_END_DATE, 'DD/MM/RRRR') END, " +
            "CASE WHEN t.ACTIVE_STATUS IS NULL THEN 0  ELSE t.ACTIVE_STATUS END ," +
            "CASE WHEN t.COM_YEAR_ID IS NULL THEN 0  ELSE t.COM_YEAR_ID END " +
            " FROM HCI_TBL_COM_ORD_RATE_CHART t  WHERE t.ID=:V_ID AND t.EFFECTIVE_END_DATE IS NULL";
            command = new OracleCommand(sql, connection);
            command.Parameters.Add(new OracleParameter("V_ID", id));
            connection.Open();
            try
            {
                dataReader = command.ExecuteReader();
                if (dataReader.HasRows)
                {
                    dataReader.Read();
                    commissionORDRateChart.Id = Convert.ToInt32(dataReader[0]);
                    commissionORDRateChart.Code = dataReader[1].ToString();
                    commissionORDRateChart.Description = dataReader[2].ToString();
                    commissionORDRateChart.AgtTypeId = Convert.ToInt32(dataReader[3]);
                    commissionORDRateChart.ComLevelId = Convert.ToInt32(dataReader[4]);
                    commissionORDRateChart.Rate = Convert.ToInt32(dataReader[5]);
                    commissionORDRateChart.FromDate = dataReader[6].ToString();
                    commissionORDRateChart.ToDate = dataReader[7].ToString();
                    commissionORDRateChart.Sql = dataReader[8].ToString();
                    commissionORDRateChart.CreatedBy = dataReader[9].ToString();
                    commissionORDRateChart.CreatedDate = dataReader[10].ToString();
                    commissionORDRateChart.EffectiveEndDate = dataReader[11].ToString();
                    commissionORDRateChart.ActiveStatus = Convert.ToInt32(dataReader[12]);
                    commissionORDRateChart.ComYearId = Convert.ToInt32(dataReader[13]);

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
            return commissionORDRateChart;
        }





        [HttpPost]
        [ActionName("SaveCommissionORDRateChart")]
        public HttpResponseMessage SaveCommissionORDRateChartl(CommissionORDRateChart obj)
        {
            OracleConnection connection = new OracleConnection(ConnectionString);
            OracleCommand command;
            try
            {
                connection.Open();
                command = new OracleCommand("HCI_SP_ORD_RATE_INSERT");
                command.CommandType = CommandType.StoredProcedure;
                command.Connection = connection;
                command.Parameters.Add("V_ID", OracleType.Number).Value = obj.Id;
                command.Parameters.Add("V_CODE", OracleType.VarChar).Value = obj.Code;
                command.Parameters.Add("V_DESCRIPTION", OracleType.VarChar).Value = obj.Description;
                command.Parameters.Add("V_AGT_TYPE_ID", OracleType.Number).Value = obj.AgtTypeId;
                command.Parameters.Add("V_COM_LEVEL_ID", OracleType.Number).Value = obj.ComLevelId;
                command.Parameters.Add("V_COM_YEAR_ID", OracleType.Number).Value = obj.ComYearId;
                command.Parameters.Add("V_RATE", OracleType.Number).Value = obj.Rate;
                command.Parameters.Add("V_FROM_DATE", OracleType.DateTime).Value = obj.FromDate;
                command.Parameters.Add("V_TO_DATE", OracleType.DateTime).Value = obj.ToDate;
                command.Parameters.Add("V_SQL", OracleType.VarChar).Value = obj.Sql;
                command.Parameters.Add("V_CREATED_BY", OracleType.VarChar).Value = obj.CreatedBy;
                command.Parameters.Add("V_ACTIVE_STATUS", OracleType.Number).Value = obj.ActiveStatus;
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



    }
}
