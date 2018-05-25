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
    public class TargetController : ApiController
    {


        static string ConnectionString = ConfigurationManager.ConnectionStrings["OracleConString"].ToString();[HttpGet]

        public IEnumerable<targets> Get()
        {
            List<targets> targetsList = new List<targets>();
            DataTable dataTable = new DataTable();
            OracleDataReader dataReader = null;
            OracleConnection connection = new OracleConnection(ConnectionString);
            OracleCommand command;
            string sql = "SELECT " +
            "CASE WHEN t.ID IS NULL THEN 0  ELSE t.ID END, " +
            "CASE WHEN t.LEVEL_ID IS NULL THEN 0  ELSE t.LEVEL_ID END, " +
            "CASE WHEN t.TYPE_ID IS NULL THEN 0  ELSE t.TYPE_ID END, " +
            "CASE WHEN t.YEAR IS NULL THEN 0  ELSE t.YEAR END, " +
            "CASE WHEN t.YEARLY_TARGET IS NULL THEN 0  ELSE t.YEARLY_TARGET END, " +
            "CASE WHEN t.CREATED_BY IS NULL THEN ''  ELSE t.CREATED_BY END, " +
            "CASE WHEN t.CREATED_DATE IS NULL THEN to_date('01/01/1900', 'DD/MM/RRRR')  ELSE to_date(t.CREATED_DATE, 'DD/MM/RRRR') END, " +
            "CASE WHEN t.EFFECTIVE_END_DATE IS NULL THEN to_date('01/01/1900', 'DD/MM/RRRR')  ELSE to_date(t.EFFECTIVE_END_DATE, 'DD/MM/RRRR') END, " +
            "CASE WHEN t.ACTIVE_STATUS IS NULL THEN 0  ELSE t.ACTIVE_STATUS END " +
            " FROM HCI_TBL_TARGETS t ";
            command = new OracleCommand(sql, connection);
            try
            {
                connection.Open();
                dataReader = command.ExecuteReader();
                dataTable.Load(dataReader);
                dataReader.Close();
                connection.Close();
                targetsList = (from DataRow drow in dataTable.Rows
                               select new targets()
                               {
                                   Id = Convert.ToInt32(drow[0]),
                                   LevelId = Convert.ToInt32(drow[1]),
                                   TypeId = Convert.ToInt32(drow[2]),
                                   Year = Convert.ToInt32(drow[3]),
                                   YearlyTarget = Convert.ToInt32(drow[4]),
                                   CreatedBy = drow[5].ToString(),
                                   CreatedDate = drow[6].ToString(),
                                   EffectiveEndDate = drow[7].ToString(),
                                   ActiveStatus = Convert.ToInt32(drow[8])
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
            return targetsList;
        }
        [HttpGet]
        public targets Get(int id)
        {
            targets targets = new targets();
            OracleDataReader dataReader = null;
            OracleConnection connection = new OracleConnection(ConnectionString);
            OracleCommand command;
            string sql = "SELECT " +
            "CASE WHEN t.ID IS NULL THEN 0  ELSE t.ID END, " +
            "CASE WHEN t.LEVEL_ID IS NULL THEN 0  ELSE t.LEVEL_ID END, " +
            "CASE WHEN t.TYPE_ID IS NULL THEN 0  ELSE t.TYPE_ID END, " +
            "CASE WHEN t.YEAR IS NULL THEN 0  ELSE t.YEAR END, " +
            "CASE WHEN t.YEARLY_TARGET IS NULL THEN 0  ELSE t.YEARLY_TARGET END, " +
            "CASE WHEN t.CREATED_BY IS NULL THEN ''  ELSE t.CREATED_BY END, " +
            "CASE WHEN t.CREATED_DATE IS NULL THEN to_date('01/01/1900', 'DD/MM/RRRR')  ELSE to_date(t.CREATED_DATE, 'DD/MM/RRRR') END, " +
            "CASE WHEN t.EFFECTIVE_END_DATE IS NULL THEN to_date('01/01/1900', 'DD/MM/RRRR')  ELSE to_date(t.EFFECTIVE_END_DATE, 'DD/MM/RRRR') END, " +
            "CASE WHEN t.ACTIVE_STATUS IS NULL THEN 0  ELSE t.ACTIVE_STATUS END " +
            " FROM HCI_TBL_TARGETS t  WHERE t.ID=:V_ID";
            command = new OracleCommand(sql, connection);
            command.Parameters.Add(new OracleParameter("V_ID", id));
            connection.Open();
            try
            {
                dataReader = command.ExecuteReader();
                if (dataReader.HasRows)
                {
                    dataReader.Read();
                    targets.Id = Convert.ToInt32(dataReader[0]);
                    targets.LevelId = Convert.ToInt32(dataReader[1]);
                    targets.TypeId = Convert.ToInt32(dataReader[2]);
                    targets.Year = Convert.ToInt32(dataReader[3]);
                    targets.YearlyTarget = Convert.ToInt32(dataReader[4]);
                    targets.CreatedBy = dataReader[5].ToString();
                    targets.CreatedDate = dataReader[6].ToString();
                    targets.EffectiveEndDate = dataReader[7].ToString();
                    targets.ActiveStatus = Convert.ToInt32(dataReader[8]);

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
            return targets;
        }



        [HttpPost]
        [ActionName("Savetargets")]
        public HttpResponseMessage Savetarget(targets obj)
        {
            OracleConnection connection = new OracleConnection(ConnectionString);
            OracleCommand command;
            try
            {
                connection.Open();
                command = new OracleCommand("INSERT_HCI_TBL_TARGETS");
                command.CommandType = CommandType.StoredProcedure;
                command.Connection = connection;
                command.Parameters.Add("V_ID", OracleType.Number).Value = obj.Id;
                command.Parameters.Add("V_LEVEL_ID", OracleType.Number).Value = obj.LevelId;
                command.Parameters.Add("V_TYPE_ID", OracleType.Number).Value = obj.TypeId;
                command.Parameters.Add("V_YEAR", OracleType.Number).Value = obj.Year;
                command.Parameters.Add("V_YEARLY_TARGET", OracleType.Number).Value = obj.YearlyTarget;
                command.Parameters.Add("V_CREATED_BY", OracleType.VarChar).Value = obj.CreatedBy;
                command.Parameters.Add("V_CREATED_DATE", OracleType.DateTime).Value = obj.CreatedDate;
                command.Parameters.Add("V_EFFECTIVE_END_DATE", OracleType.DateTime).Value = obj.EffectiveEndDate;
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
