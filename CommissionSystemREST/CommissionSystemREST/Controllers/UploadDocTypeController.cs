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
    public class UploadDocTypeController : ApiController
    {


        static string ConnectionString = ConfigurationManager.ConnectionStrings["OracleConString"].ToString();[HttpGet]
        public IEnumerable<UploadDocType> Get()
        {
            List<UploadDocType> uploadDocTypeList = new List<UploadDocType>();
            DataTable dataTable = new DataTable();
            OracleDataReader dataReader = null;
            OracleConnection connection = new OracleConnection(ConnectionString);
            OracleCommand command;
            string sql = "SELECT " +
            "CASE WHEN t.DOC_TYPE_ID IS NULL THEN 0  ELSE t.DOC_TYPE_ID END, " +
            "CASE WHEN t.DOC_TYPE_NAME IS NULL THEN ''  ELSE t.DOC_TYPE_NAME END " +
            " FROM HCI_TBL_UPLOADED_DOC_TYPE t ";
            command = new OracleCommand(sql, connection);
            try
            {
                connection.Open();
                dataReader = command.ExecuteReader();
                dataTable.Load(dataReader);
                dataReader.Close();
                connection.Close();
                uploadDocTypeList = (from DataRow drow in dataTable.Rows
                                     select new UploadDocType()
                                     {
                                         DocTypeId = Convert.ToInt32(drow[0]),
                                         DocTypeName = drow[1].ToString()
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
            return uploadDocTypeList;
        }
        [HttpGet]
        public UploadDocType Get(int id)
        {
            UploadDocType uploadDocType = new UploadDocType();
            OracleDataReader dataReader = null;
            OracleConnection connection = new OracleConnection(ConnectionString);
            OracleCommand command;
            string sql = "SELECT " +
            "CASE WHEN t.DOC_TYPE_ID IS NULL THEN 0  ELSE t.DOC_TYPE_ID END, " +
            "CASE WHEN t.DOC_TYPE_NAME IS NULL THEN ''  ELSE t.DOC_TYPE_NAME END " +
            " FROM HCI_TBL_UPLOADED_DOC_TYPE t  WHERE t.DOC_TYPE_ID=:V_DOC_TYPE_ID";
            command = new OracleCommand(sql, connection);
            command.Parameters.Add(new OracleParameter("V_DOC_TYPE_ID", id));
            connection.Open();
            try
            {
                dataReader = command.ExecuteReader();
                if (dataReader.HasRows)
                {
                    dataReader.Read();
                    uploadDocType.DocTypeId = Convert.ToInt32(dataReader[0]);
                    uploadDocType.DocTypeName = dataReader[1].ToString();

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
            return uploadDocType;
        }


        [HttpPost]
        [ActionName("SaveUploadDocType")]
        public HttpResponseMessage SaveUploadDocType(UploadDocType obj)
        {
            OracleConnection connection = new OracleConnection(ConnectionString);
            OracleCommand command;
            try
            {
                connection.Open();
                command = new OracleCommand("INSERT_HCI_TBL_UPLOADED_DOC_TYPE");
                command.CommandType = CommandType.StoredProcedure;
                command.Connection = connection;
                command.Parameters.Add("V_DOC_TYPE_ID", OracleType.Number).Value = obj.DocTypeId;
                command.Parameters.Add("V_DOC_TYPE_NAME", OracleType.VarChar).Value = obj.DocTypeName;
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
