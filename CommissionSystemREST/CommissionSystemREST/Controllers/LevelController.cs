using CommissionSystemREST.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.OracleClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace CommissionSystemREST.Controllers
{
    public class LevelController : ApiController
    {
        static string ConnectionString = ConfigurationManager.ConnectionStrings["OracleConString"].ToString();[HttpGet]
        public IEnumerable<Level> Get()
        {
            List<Level> levelList = new List<Level>();
            DataTable dataTable = new DataTable();
            OracleDataReader dataReader = null;
            OracleConnection connection = new OracleConnection(ConnectionString);
            OracleCommand command;
            string sql = "SELECT " +
            "CASE WHEN t.ID IS NULL THEN 0  ELSE t.ID END, " +
            "CASE WHEN t.CODE IS NULL THEN ''  ELSE t.CODE END, " +
            "CASE WHEN t.DESCRIPTION IS NULL THEN ''  ELSE t.DESCRIPTION END, " +
            "CASE WHEN t.CREATED_BY IS NULL THEN ''  ELSE t.CREATED_BY END, " +
            "CASE WHEN t.CREATED_DATE IS NULL THEN to_date('01/01/1900', 'DD/MM/RRRR')  ELSE to_date(t.CREATED_DATE, 'DD/MM/RRRR') END, " +
            "CASE WHEN t.EFFECTIVE_END_DATE IS NULL THEN to_date('01/01/1900', 'DD/MM/RRRR')  ELSE to_date(t.EFFECTIVE_END_DATE, 'DD/MM/RRRR') END, " +
            "CASE WHEN t.SEQ_NO IS NULL THEN 0  ELSE t.SEQ_NO END " +
            " FROM HCI_TBL_LEVEL t WHERE  T.EFFECTIVE_END_DATE IS NULL";
            command = new OracleCommand(sql, connection);
            try
            {
                connection.Open();
                dataReader = command.ExecuteReader();
                dataTable.Load(dataReader);
                dataReader.Close();
                connection.Close();
                levelList = (from DataRow drow in dataTable.Rows
                             select new Level()
                             {
                                 Id = Convert.ToInt32(drow[0]),
                                 Code = drow[1].ToString(),
                                 Description = drow[2].ToString(),
                                 CreatedBy = drow[3].ToString(),
                                 CreatedDate = drow[4].ToString(),
                                 EffectiveEndDate = drow[5].ToString(),
                                 SeqNo = Convert.ToInt32(drow[6])
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
            return levelList;
        }
        [HttpGet]
        public Level Get(int id)
        {
            Level level = new Level();
            OracleDataReader dataReader = null;
            OracleConnection connection = new OracleConnection(ConnectionString);
            OracleCommand command;

            string sql = "SELECT " +
            "CASE WHEN t.ID IS NULL THEN 0  ELSE t.ID END, " +
            "CASE WHEN t.CODE IS NULL THEN ''  ELSE t.CODE END, " +
            "CASE WHEN t.DESCRIPTION IS NULL THEN ''  ELSE t.DESCRIPTION END, " +
            "CASE WHEN t.CREATED_BY IS NULL THEN ''  ELSE t.CREATED_BY END, " +
            "CASE WHEN t.CREATED_DATE IS NULL THEN to_date('01/01/1900', 'DD/MM/RRRR')  ELSE to_date(t.CREATED_DATE, 'DD/MM/RRRR') END, " +
            "CASE WHEN t.EFFECTIVE_END_DATE IS NULL THEN to_date('01/01/1900', 'DD/MM/RRRR')  ELSE to_date(t.EFFECTIVE_END_DATE, 'DD/MM/RRRR') END, " +
            "CASE WHEN t.SEQ_NO IS NULL THEN 0  ELSE t.SEQ_NO END, " +
            "t.TYPE , " +
            "t.ACTIVE_STATUS " +
            " FROM HCI_TBL_LEVEL t  WHERE t.ID=:V_ID";

            command = new OracleCommand(sql, connection);
            command.Parameters.Add(new OracleParameter("V_ID", id));
            connection.Open();
            try
            {
                dataReader = command.ExecuteReader();
                if (dataReader.HasRows)
                {
                    dataReader.Read();
                    level.Id = Convert.ToInt32(dataReader[0]);
                    level.Code = dataReader[1].ToString();
                    level.Description = dataReader[2].ToString();
                    level.CreatedBy = dataReader[3].ToString();
                    level.CreatedDate = dataReader[4].ToString();
                    level.EffectiveEndDate = dataReader[5].ToString();
                    level.SeqNo = Convert.ToInt32(dataReader[6]);
                    level.Type = Convert.ToInt16(dataReader[7]);
                    level.ActiveStatus = Convert.ToInt16(dataReader[8].ToString());

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
            return level;
        }

        [HttpPost]
        [ActionName("SaveLevel")]
        public HttpResponseMessage SaveLevell(Level obj)
        {
            OracleConnection connection = new OracleConnection(ConnectionString);
            OracleCommand command;
            try
            {
                connection.Open();
                command = new OracleCommand("HCI_SP_LEVEL_INSERT");
                command.CommandType = CommandType.StoredProcedure;
                command.Connection = connection;

                command.Parameters.Add("V_ID", OracleType.Number).Value = obj.Id;
                command.Parameters.Add("V_CODE", OracleType.VarChar).Value = obj.Code;
                command.Parameters.Add("V_DESCRIPTION", OracleType.VarChar).Value = obj.Description;
                command.Parameters.Add("V_CREATED_BY", OracleType.VarChar).Value = obj.CreatedBy;
                command.Parameters.Add("V_SEQ_NO", OracleType.Number).Value = 0;
                command.Parameters.Add("V_TYPE", OracleType.Number).Value = Convert.ToInt16(obj.Type);
                command.Parameters.Add("V_ACTIVE_STATUS", OracleType.Number).Value = Convert.ToInt16(obj.ActiveStatus);

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
        [HttpPut]
        [ActionName("UpdateLevel")]
        public HttpResponseMessage UpdateLevell(Level obj)
        {
            OracleConnection connection = new OracleConnection(ConnectionString);
            OracleCommand command;
            try
            {
                connection.Open();
                command = new OracleCommand("UPDATE_HCI_TBL_LEVEL");
                command.CommandType = CommandType.StoredProcedure;
                command.Connection = connection;
                command.Parameters.Add("V_ID", OracleType.Number).Value = obj.Id;
                command.Parameters.Add("V_CODE", OracleType.VarChar).Value = obj.Code;
                command.Parameters.Add("V_DESCRIPTION", OracleType.VarChar).Value = obj.Description;
                command.Parameters.Add("V_CREATED_BY", OracleType.VarChar).Value = obj.CreatedBy;
                command.Parameters.Add("V_CREATED_DATE", OracleType.DateTime).Value = obj.CreatedDate;
                command.Parameters.Add("V_EFFECTIVE_END_DATE", OracleType.DateTime).Value = obj.EffectiveEndDate;
                command.Parameters.Add("V_SEQ_NO", OracleType.Number).Value = obj.SeqNo;
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
