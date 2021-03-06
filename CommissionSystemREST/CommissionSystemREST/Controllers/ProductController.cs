﻿using System;
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
    public class ProductController : ApiController
    {

        static string ConnectionString = ConfigurationManager.ConnectionStrings["OracleConString"].ToString();[HttpGet]


        public IEnumerable<product> Get()
        {
            List<product> productcategoryList = new List<product>();
            DataTable dataTable = new DataTable();
            OracleDataReader dataReader = null;
            OracleConnection connection = new OracleConnection(ConnectionString);
            OracleCommand command;
            string sql = "SELECT " +
            "CASE WHEN t.ID IS NULL THEN 0  ELSE t.ID END, " +
            "CASE WHEN t.CATEGORY_ID IS NULL THEN ''  ELSE t.CATEGORY_ID END, " +
            "CASE WHEN t.CODE IS NULL THEN ''  ELSE t.CODE END, " +
            "CASE WHEN t.DESCRIPTION IS NULL THEN ''  ELSE t.DESCRIPTION END, " +
            "CASE WHEN t.CREATED_BY IS NULL THEN ''  ELSE t.CREATED_BY END, " +
            "CASE WHEN t.CREATED_DATE IS NULL THEN to_date('01/01/1900', 'DD/MM/RRRR')  ELSE to_date(t.CREATED_DATE, 'DD/MM/RRRR') END, " +
            "CASE WHEN t.EFFECTIVE_END_DATE IS NULL THEN to_date('01/01/1900', 'DD/MM/RRRR')  ELSE to_date(t.EFFECTIVE_END_DATE, 'DD/MM/RRRR') END, " +
            "CASE WHEN t.ACTIVE_STATUS IS NULL THEN 0  ELSE t.ACTIVE_STATUS END " +
            " FROM HCI_TBL_PRODUCT t WHERE t.EFFECTIVE_END_DATE IS NULL";
            command = new OracleCommand(sql, connection);
            try
            {
                connection.Open();
                dataReader = command.ExecuteReader();
                dataTable.Load(dataReader);
                dataReader.Close();
                connection.Close();
                productcategoryList = (from DataRow drow in dataTable.Rows
                                       select new product()
                                       {
                                           Id = Convert.ToInt32(drow[0]),
                                           CategoryID = Convert.ToInt32(drow[1]),
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
            return productcategoryList;
        }




        [HttpGet]
        public product Get(int id)
        {
            product productcategory = new product();
            OracleDataReader dataReader = null;
            OracleConnection connection = new OracleConnection(ConnectionString);
            OracleCommand command;
            string sql = "SELECT " +
            "CASE WHEN t.ID IS NULL THEN 0  ELSE t.ID END, " +
            "CASE WHEN t.CATEGORY_ID IS NULL THEN ''  ELSE t.CATEGORY_ID END, " +
            "CASE WHEN t.CODE IS NULL THEN ''  ELSE t.CODE END, " +
            "CASE WHEN t.DESCRIPTION IS NULL THEN ''  ELSE t.DESCRIPTION END, " +
            "CASE WHEN t.CREATED_BY IS NULL THEN ''  ELSE t.CREATED_BY END, " +
            "CASE WHEN t.CREATED_DATE IS NULL THEN to_date('01/01/1900', 'DD/MM/RRRR')  ELSE to_date(t.CREATED_DATE, 'DD/MM/RRRR') END, " +
            "CASE WHEN t.EFFECTIVE_END_DATE IS NULL THEN to_date('01/01/1900', 'DD/MM/RRRR')  ELSE to_date(t.EFFECTIVE_END_DATE, 'DD/MM/RRRR') END, " +
            "CASE WHEN t.ACTIVE_STATUS IS NULL THEN 0  ELSE t.ACTIVE_STATUS END " +
            "FROM HCI_TBL_PRODUCT t  WHERE t.EFFECTIVE_END_DATE IS NULL AND t.ID=:V_ID";
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
                    productcategory.CategoryID = Convert.ToInt32(dataReader[1]);
                    productcategory.Code = dataReader[2].ToString();
                    productcategory.Description = dataReader[3].ToString();
                    productcategory.CreatedBy = dataReader[4].ToString();
                    productcategory.CreatedDate = dataReader[5].ToString();
                    productcategory.EffectiveEndDate = dataReader[6].ToString();
                    productcategory.ActiveStatus = Convert.ToInt32(dataReader[7]);

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



        [HttpPost]
        [ActionName("Saveproduct")]
        public HttpResponseMessage Saveproduct(product obj)
        {
            OracleConnection connection = new OracleConnection(ConnectionString);
            OracleCommand command;
            try
            {
                connection.Open();
                command = new OracleCommand("HCI_SP_PRODUCT_INSERT");
                command.CommandType = CommandType.StoredProcedure;
                command.Connection = connection;
                command.Parameters.Add("V_ID", OracleType.Number).Value = obj.Id;
                command.Parameters.Add("V_CODE", OracleType.VarChar).Value = obj.Code;
                command.Parameters.Add("V_CATEGORY_ID", OracleType.Number).Value = obj.CategoryID;
                command.Parameters.Add("V_DESCRIPTION", OracleType.VarChar).Value = obj.Description;
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

