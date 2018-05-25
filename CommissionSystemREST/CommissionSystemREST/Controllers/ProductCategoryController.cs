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
    public class ProductCategoryController : ApiController
    {

        static string ConnectionString = ConfigurationManager.ConnectionStrings["OracleConString"].ToString();[HttpGet]


        [HttpPost]
        [ActionName("Saveproductcategory")]
        public HttpResponseMessage Saveproductcategory(productcategory obj)
        {
            OracleConnection connection = new OracleConnection(ConnectionString);
            OracleCommand command;
            try
            {
                connection.Open();
                command = new OracleCommand("HCI_SP_PROD_CATEGORY_INSERT");
                command.CommandType = CommandType.StoredProcedure;
                command.Connection = connection;
                command.Parameters.Add("V_ID", OracleType.Number).Value = obj.Id;
                command.Parameters.Add("V_CODE", OracleType.VarChar).Value = obj.Code;
                command.Parameters.Add("V_DESCRIPTION", OracleType.VarChar).Value = obj.Description;
                command.Parameters.Add("V_BUSSINESS_TYPE", OracleType.VarChar).Value = obj.BussinessType;
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


        public IEnumerable<productcategory> Get()
        {
            List<productcategory> productcategoryList = new List<productcategory>();
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
            "CASE WHEN t.ACTIVE_STATUS IS NULL THEN 0  ELSE t.ACTIVE_STATUS END, " +
            "CASE WHEN t.BUSSINESS_TYPE IS NULL THEN ''  ELSE t.BUSSINESS_TYPE END " +
            "FROM HCI_TBL_PRODUCT_CATEGORY t WHERE T.EFFECTIVE_END_DATE IS NULL";

            command = new OracleCommand(sql, connection);
            try
            {
                connection.Open();
                dataReader = command.ExecuteReader();
                dataTable.Load(dataReader);
                dataReader.Close();
                connection.Close();
                productcategoryList = (from DataRow drow in dataTable.Rows
                                       select new productcategory()
                                       {
                                           Id = Convert.ToInt32(drow[0]),
                                           Code = drow[1].ToString(),
                                           Description = drow[2].ToString(),
                                           CreatedBy = drow[3].ToString(),
                                           CreatedDate = drow[4].ToString(),
                                           EffectiveEndDate = drow[5].ToString(),
                                           ActiveStatus = Convert.ToInt32(drow[6]),
                                           BussinessType = drow[7].ToString()
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
            return productcategoryList;
        }

        [HttpGet]
        public productcategory Get(int id)
        {
            productcategory productcategory = new productcategory();
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
            "CASE WHEN t.ACTIVE_STATUS IS NULL THEN 0  ELSE t.ACTIVE_STATUS END , " +
            "CASE WHEN t.BUSSINESS_TYPE IS NULL THEN ''  ELSE t.BUSSINESS_TYPE END " +
            " FROM HCI_TBL_PRODUCT_CATEGORY t  WHERE t.ID=:V_ID";
            command = new OracleCommand(sql, connection);
            command.Parameters.Add(new OracleParameter("V_ID", id));
            connection.Open();
            try
            {
                dataReader = command.ExecuteReader();
                if (dataReader.HasRows)
                {
                    dataReader.Read();
                    productcategory.Id = Convert.ToInt32(dataReader[0]);
                    productcategory.Code = dataReader[1].ToString();
                    productcategory.Description = dataReader[2].ToString();
                    productcategory.CreatedBy = dataReader[3].ToString();
                    productcategory.CreatedDate = dataReader[4].ToString();
                    productcategory.EffectiveEndDate = dataReader[5].ToString();
                    productcategory.ActiveStatus = Convert.ToInt32(dataReader[6]);
                    productcategory.BussinessType = dataReader[7].ToString();

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
            return productcategory;
        }




    }
}
