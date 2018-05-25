using ComissionWebAPI.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.OracleClient;
using System.Linq;
using System.Web.Http;
using System.Net.Http;


using System.Net;



namespace ComissionWebAPI.Controllers
{
    public class FSTAllowanceController : ApiController
    {



        static string ConnectionString = ConfigurationManager.ConnectionStrings["OracleConString"].ToString();[HttpGet]
        public IEnumerable<FSTAllowance> Get()
        {
            List<FSTAllowance> fSTAllowanceList = new List<FSTAllowance>();
            DataTable dataTable = new DataTable();
            OracleDataReader dataReader = null;
            OracleConnection connection = new OracleConnection(ConnectionString);
            OracleCommand command;
            string sql = "SELECT " +
            "CASE WHEN t.ID IS NULL THEN 0  ELSE t.ID END, " +
            "CASE WHEN t.CODE IS NULL THEN ''  ELSE t.CODE END, " +
            "CASE WHEN t.DESCRIPTION IS NULL THEN ''  ELSE t.DESCRIPTION END, " +
            "CASE WHEN t.MAXIMUM_YEARS IS NULL THEN 0  ELSE t.MAXIMUM_YEARS END, " +
            "CASE WHEN t.MAXIMUM_ALLOWANCE IS NULL THEN 0  ELSE t.MAXIMUM_ALLOWANCE END, " +
            "CASE WHEN t.POLICY_COUNT IS NULL THEN 0  ELSE t.POLICY_COUNT END, " +
            "CASE WHEN t.FST_MINIMUM_AMT IS NULL THEN 0  ELSE t.FST_MINIMUM_AMT END, " +
            "CASE WHEN t.FST_MAXIMUM_AMT IS NULL THEN 0  ELSE t.FST_MAXIMUM_AMT END, " +
            "CASE WHEN t.ALLOWANCE IS NULL THEN 0  ELSE t.ALLOWANCE END, " +
            "CASE WHEN t.SQL IS NULL THEN ''  ELSE t.SQL END, " +
            "CASE WHEN t.FROM_DATE IS NULL THEN to_date('01/01/1900', 'DD/MM/RRRR')  ELSE to_date(t.FROM_DATE, 'DD/MM/RRRR') END, " +
            "CASE WHEN t.TO_DATE IS NULL THEN to_date('01/01/1900', 'DD/MM/RRRR')  ELSE to_date(t.TO_DATE, 'DD/MM/RRRR') END, " +
            "CASE WHEN t.ACTIVE_STATUS IS NULL THEN 0  ELSE t.ACTIVE_STATUS END, " +
            "CASE WHEN t.CREATED_BY IS NULL THEN ''  ELSE t.CREATED_BY END, " +
            "CASE WHEN t.CREATED_DATE IS NULL THEN to_date('01/01/1900', 'DD/MM/RRRR')  ELSE to_date(t.CREATED_DATE, 'DD/MM/RRRR') END, " +
            "CASE WHEN t.EFFECTIVE_END_DATE IS NULL THEN to_date('01/01/1900', 'DD/MM/RRRR')  ELSE to_date(t.EFFECTIVE_END_DATE, 'DD/MM/RRRR') END " +
            " FROM HCI_TBL_FST_RATE_CHART t WHERE EFFECTIVE_END_DATE IS NULL";
            command = new OracleCommand(sql, connection);
            try
            {
                connection.Open();
                dataReader = command.ExecuteReader();
                dataTable.Load(dataReader);
                dataReader.Close();
                connection.Close();
                fSTAllowanceList = (from DataRow drow in dataTable.Rows
                                    select new FSTAllowance()
                                    {
                                        Id = Convert.ToInt32(drow[0]),
                                        Code = drow[1].ToString(),
                                        Description = drow[2].ToString(),
                                        MaximumYears = Convert.ToInt32(drow[3]),
                                        MaximumAllowance = Convert.ToInt32(drow[4]),
                                        PolicyCount = Convert.ToInt32(drow[5]),
                                        FstMinimumAmt = Convert.ToInt32(drow[6]),
                                        FstMaximumAmt = Convert.ToInt32(drow[7]),
                                        Allowance = Convert.ToInt32(drow[8]),
                                        Sql = drow[9].ToString(),
                                        FromDate = drow[10].ToString(),
                                        ToDate = drow[11].ToString(),
                                        ActiveStatus = Convert.ToInt32(drow[12]),
                                        CreatedBy = drow[13].ToString(),
                                        CreatedDate = drow[14].ToString(),
                                        EffectiveEndDate = drow[15].ToString()
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
            return fSTAllowanceList;
        }
        [HttpGet]
        public FSTAllowance Get(int id)
        {
            FSTAllowance fSTAllowance = new FSTAllowance();
            OracleDataReader dataReader = null;
            OracleConnection connection = new OracleConnection(ConnectionString);
            OracleCommand command;
            string sql = "SELECT " +
            "CASE WHEN t.ID IS NULL THEN 0  ELSE t.ID END, " +
            "CASE WHEN t.CODE IS NULL THEN ''  ELSE t.CODE END, " +
            "CASE WHEN t.DESCRIPTION IS NULL THEN ''  ELSE t.DESCRIPTION END, " +
            "CASE WHEN t.MAXIMUM_YEARS IS NULL THEN 0  ELSE t.MAXIMUM_YEARS END, " +
            "CASE WHEN t.MAXIMUM_ALLOWANCE IS NULL THEN 0  ELSE t.MAXIMUM_ALLOWANCE END, " +
            "CASE WHEN t.POLICY_COUNT IS NULL THEN 0  ELSE t.POLICY_COUNT END, " +
            "CASE WHEN t.FST_MINIMUM_AMT IS NULL THEN 0  ELSE t.FST_MINIMUM_AMT END, " +
            "CASE WHEN t.FST_MAXIMUM_AMT IS NULL THEN 0  ELSE t.FST_MAXIMUM_AMT END, " +
            "CASE WHEN t.ALLOWANCE IS NULL THEN 0  ELSE t.ALLOWANCE END, " +
            "CASE WHEN t.SQL IS NULL THEN ''  ELSE t.SQL END, " +
            "CASE WHEN t.FROM_DATE IS NULL THEN to_date('01/01/1900', 'DD/MM/RRRR')  ELSE to_date(t.FROM_DATE, 'DD/MM/RRRR') END, " +
            "CASE WHEN t.TO_DATE IS NULL THEN to_date('01/01/1900', 'DD/MM/RRRR')  ELSE to_date(t.TO_DATE, 'DD/MM/RRRR') END, " +
            "CASE WHEN t.ACTIVE_STATUS IS NULL THEN 0  ELSE t.ACTIVE_STATUS END, " +
            "CASE WHEN t.CREATED_BY IS NULL THEN ''  ELSE t.CREATED_BY END, " +
            "CASE WHEN t.CREATED_DATE IS NULL THEN to_date('01/01/1900', 'DD/MM/RRRR')  ELSE to_date(t.CREATED_DATE, 'DD/MM/RRRR') END, " +
            "CASE WHEN t.EFFECTIVE_END_DATE IS NULL THEN to_date('01/01/1900', 'DD/MM/RRRR')  ELSE to_date(t.EFFECTIVE_END_DATE, 'DD/MM/RRRR') END " +
            " FROM HCI_TBL_FST_RATE_CHART t  WHERE t.ID=:V_ID AND EFFECTIVE_END_DATE IS NULL ";
            command = new OracleCommand(sql, connection);
            command.Parameters.Add(new OracleParameter("V_ID", id));
            connection.Open();
            try
            {
                dataReader = command.ExecuteReader();
                if (dataReader.HasRows)
                {
                    dataReader.Read();
                    fSTAllowance.Id = Convert.ToInt32(dataReader[0]);
                    fSTAllowance.Code = dataReader[1].ToString();
                    fSTAllowance.Description = dataReader[2].ToString();
                    fSTAllowance.MaximumYears = Convert.ToInt32(dataReader[3]);
                    fSTAllowance.MaximumAllowance = Convert.ToInt32(dataReader[4]);
                    fSTAllowance.PolicyCount = Convert.ToInt32(dataReader[5]);
                    fSTAllowance.FstMinimumAmt = Convert.ToInt32(dataReader[6]);
                    fSTAllowance.FstMaximumAmt = Convert.ToInt32(dataReader[7]);
                    fSTAllowance.Allowance = Convert.ToInt32(dataReader[8]);
                    fSTAllowance.Sql = dataReader[9].ToString();
                    fSTAllowance.FromDate = dataReader[10].ToString();
                    fSTAllowance.ToDate = dataReader[11].ToString();
                    fSTAllowance.ActiveStatus = Convert.ToInt32(dataReader[12]);
                    fSTAllowance.CreatedBy = dataReader[13].ToString();
                    fSTAllowance.CreatedDate = dataReader[14].ToString();
                    fSTAllowance.EffectiveEndDate = dataReader[15].ToString();

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
            return fSTAllowance;
        }



        [HttpPost]
        [ActionName("SaveFSTAllowance")]
        public HttpResponseMessage SaveFSTAllowancel(FSTAllowance obj)
        {
            OracleConnection connection = new OracleConnection(ConnectionString);
            OracleCommand command;
            try
            {
                connection.Open();
                command = new OracleCommand("HCI_SP_FST_RATE_CHART_INSERT");
                command.CommandType = CommandType.StoredProcedure;
                command.Connection = connection;
                //command.Parameters.Add("V_ID", OracleType.Number).Value = obj.Id;
                command.Parameters.Add("V_CODE", OracleType.VarChar).Value = obj.Code;
                command.Parameters.Add("V_DESCRIPTION", OracleType.VarChar).Value = obj.Description;
                command.Parameters.Add("V_MAXIMUM_YEARS", OracleType.Number).Value = obj.MaximumYears;
                command.Parameters.Add("V_MAXIMUM_ALLOWANCE", OracleType.Number).Value = obj.MaximumAllowance;
                command.Parameters.Add("V_POLICY_COUNT", OracleType.Number).Value = obj.PolicyCount;
                command.Parameters.Add("V_FST_MINIMUM_AMT", OracleType.Number).Value = obj.FstMinimumAmt;
                command.Parameters.Add("V_FST_MAXIMUM_AMT", OracleType.Number).Value = obj.FstMaximumAmt;
                command.Parameters.Add("V_ALLOWANCE", OracleType.Number).Value = obj.Allowance;
                command.Parameters.Add("V_SQL", OracleType.VarChar).Value = obj.Sql;
                command.Parameters.Add("V_FROM_DATE", OracleType.DateTime).Value = obj.FromDate;
                command.Parameters.Add("V_TO_DATE", OracleType.DateTime).Value = obj.ToDate;
                command.Parameters.Add("V_ACTIVE_STATUS", OracleType.Number).Value = obj.ActiveStatus;
                command.Parameters.Add("V_CREATED_BY", OracleType.VarChar).Value = obj.CreatedBy;
                ////command.Parameters.Add("V_CREATED_DATE", OracleType.DateTime).Value = obj.CreatedDate;
                ////command.Parameters.Add("V_EFFECTIVE_END_DATE", OracleType.DateTime).Value = obj.EffectiveEndDate;
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
