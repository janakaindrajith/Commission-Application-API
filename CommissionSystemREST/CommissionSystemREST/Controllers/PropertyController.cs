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
    public class PropertyController : ApiController
    {


        static string ConnectionString = ConfigurationManager.ConnectionStrings["OracleConString"].ToString();[HttpGet]





        public IEnumerable<property> Get(string Type)
        {
            List<property> propertyList = new List<property>();
            DataTable dataTable = new DataTable();
            OracleDataReader dataReader = null;
            OracleConnection connection = new OracleConnection(ConnectionString);
            OracleCommand command;
            string sql = "SELECT " +
            "CASE WHEN t.ID IS NULL THEN 0  ELSE t.ID END, " +
            "CASE WHEN t.TYPE IS NULL THEN ''  ELSE t.TYPE END, " +
            "CASE WHEN t.CODE IS NULL THEN ''  ELSE t.CODE END, " +
            "CASE WHEN t.DESCRIPTION IS NULL THEN ''  ELSE t.DESCRIPTION END, " +
            "CASE WHEN t.CREATED_BY IS NULL THEN ''  ELSE t.CREATED_BY END, " +
            "CASE WHEN t.CREATED_DATE IS NULL THEN to_date('01/01/1900', 'DD/MM/RRRR')  ELSE to_date(t.CREATED_DATE, 'DD/MM/RRRR') END, " +
            "CASE WHEN t.EFFECTIVE_END_DATE IS NULL THEN to_date('01/01/1900', 'DD/MM/RRRR')  ELSE to_date(t.EFFECTIVE_END_DATE, 'DD/MM/RRRR') END, " +
            "CASE WHEN t.ACTIVE_STATUS IS NULL THEN 0  ELSE t.ACTIVE_STATUS END " +
            " FROM HCI_TBL_PROPERTY t  WHERE t.TYPE=:V_TYPE ORDER BY T.Id asc";

            command = new OracleCommand(sql, connection);
            command.Parameters.Add(new OracleParameter("V_TYPE", Type));
            try
            {
                connection.Open();
                dataReader = command.ExecuteReader();
                dataTable.Load(dataReader);
                dataReader.Close();
                connection.Close();
                propertyList = (from DataRow drow in dataTable.Rows
                                select new property()
                                {
                                    Id = Convert.ToInt32(drow[0]),
                                    Type = drow[1].ToString(),
                                    Code = drow[2].ToString(),
                                    Description = drow[3].ToString(),
                                    CreatedBy = drow[4].ToString(),
                                    CreatedDate = drow[5].ToString(),
                                    EffectiveEndDate = drow[6].ToString(),
                                    ActiveStatus = Convert.ToInt32(drow[7])
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
            return propertyList;
        }







    }
}
