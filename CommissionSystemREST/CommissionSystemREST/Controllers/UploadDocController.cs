using ComissionWebAPI.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
//using System.Data.OracleClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
//using MRPSystem.Controllers;
using System.Threading.Tasks;
using MRPSystem.Controllers.User;
using System.IO;
using CommissionSystemREST.Controllers;
using System.Data.OleDb;
using System.Web.Configuration;
using Oracle.DataAccess.Client;


namespace ComissionWebAPI.Controllers
{
    public class UploadDocController : ApiController
    {

        static string ConnectionString = ConfigurationManager.ConnectionStrings["OracleConString"].ToString();[HttpGet]
        public IEnumerable<UploadDoc> Get()
        {
            List<UploadDoc> uploadDocList = new List<UploadDoc>();
            DataTable dataTable = new DataTable();
            OracleDataReader dataReader = null;
            OracleConnection connection = new OracleConnection(ConnectionString);
            OracleCommand command;
            string sql = "SELECT " +
            "CASE WHEN t.ID IS NULL THEN 0  ELSE t.ID END, " +
            "CASE WHEN t.AGT_CODE IS NULL THEN ''  ELSE t.AGT_CODE END, " +
            "CASE WHEN t.DOC_TYPE_ID IS NULL THEN 0  ELSE t.DOC_TYPE_ID END, " +
            "CASE WHEN t.DOC_URL IS NULL THEN ''  ELSE t.DOC_URL END, " +
            "CASE WHEN t.CREATED_BY IS NULL THEN ''  ELSE t.CREATED_BY END, " +
            "CASE WHEN t.CREATED_DATE IS NULL THEN to_date('01/01/1900', 'DD/MM/RRRR')  ELSE to_date(t.CREATED_DATE, 'DD/MM/RRRR') END, " +
            "CASE WHEN t.EFFECTIVE_END_DATE IS NULL THEN to_date('01/01/1900', 'DD/MM/RRRR')  ELSE to_date(t.EFFECTIVE_END_DATE, 'DD/MM/RRRR') END,t.doc_description  " +
            " FROM HCI_TBL_UPLOADED_DOC t where t.EFFECTIVE_END_DATE is null";
            command = new OracleCommand(sql, connection);
            try
            {
                connection.Open();
                dataReader = command.ExecuteReader();
                dataTable.Load(dataReader);
                dataReader.Close();
                connection.Close();
                uploadDocList = (from DataRow drow in dataTable.Rows
                                 select new UploadDoc()
                                 {
                                     Id = Convert.ToInt32(drow[0]),
                                     AgtCode = drow[1].ToString(),
                                     DocTypeId = Convert.ToInt32(drow[2]),
                                     DocUrl = drow[3].ToString(),
                                     CreatedBy = drow[4].ToString(),
                                     CreatedDate = drow[5].ToString(),
                                     EffectiveEndDate = drow[6].ToString(),
                                     DocDescription = drow[7].ToString()
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
            return uploadDocList;
        }


        [HttpPost]
        [ActionName("SaveUploadDoc")]
        public HttpResponseMessage SaveUploadDocl(UploadDoc obj)
        {
            OracleConnection connection = new OracleConnection(ConnectionString);
            OracleCommand command;
            try
            {
                connection.Open();
                command = new OracleCommand("HCI_SP_UPLOAD_INSERT");
                command.CommandType = CommandType.StoredProcedure;
                command.Connection = connection;
                command.Parameters.Add("V_ID", OracleDbType.Double).Value = obj.Id;
                command.Parameters.Add("V_AGT_CODE", OracleDbType.NVarchar2).Value = obj.AgtCode;
                command.Parameters.Add("V_DOC_TYPE_ID", OracleDbType.Double).Value = obj.DocTypeId;
                command.Parameters.Add("V_DOC_URL", OracleDbType.NVarchar2).Value = obj.DocUrl;
                command.Parameters.Add("V_CREATED_BY", OracleDbType.NVarchar2).Value = obj.CreatedBy;
                command.Parameters.Add("V_DOC_DESCRIPTION", OracleDbType.NVarchar2).Value = obj.DocDescription;
                //command.Parameters.Add("V_CREATED_DATE", OracleType.DateTime).Value = "";// obj.CreatedDate;
                //command.Parameters.Add("V_EFFECTIVE_END_DATE", OracleType.DateTime).Value = "";// obj.EffectiveEndDate;
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


        [MimeMultipart]
        [TDABasicAuthenticationFilter(false)]
        public async Task<FileUploadResult> UploadDocument(string vAGT_CODE)
        {
            try
            {

                var uploadPath = CommissionSystemREST.Properties.Settings.Default.Commission_Docs;


                string year = DateTime.Now.Year.ToString();

                var dbSavePath = "/" + year + "/" + vAGT_CODE;

                uploadPath = uploadPath + "/" + year + "/" + vAGT_CODE;
                if (!Directory.Exists(uploadPath))
                {
                    System.IO.Directory.CreateDirectory(uploadPath);
                }

                var multipartFormDataStreamProvider = new UploadMultipartFormProvider(uploadPath);



                // Read the MIME multipart asynchronously 
                await Request.Content.ReadAsMultipartAsync(multipartFormDataStreamProvider);

                string AgentCode = multipartFormDataStreamProvider.FormData["AgentCode"];

                string DocTypeID = multipartFormDataStreamProvider.FormData["DocTypeID"];

                string UserID = multipartFormDataStreamProvider.FormData["UserID"];

                string DocDescription = multipartFormDataStreamProvider.FormData["DocDescription"];

                string _localFileName = multipartFormDataStreamProvider
                    .FileData.Select(multiPartData => multiPartData.LocalFileName).FirstOrDefault();

                dbSavePath = dbSavePath + "/" + multipartFormDataStreamProvider
                    .FileData.Select(multiPartData => multiPartData.Headers.ContentDisposition.FileName).FirstOrDefault().Replace("\"", "");



                UploadDoc uploadedDoc = new UploadDoc();
                uploadedDoc.Id = 0;
                uploadedDoc.AgtCode = AgentCode;
                uploadedDoc.DocTypeId = Convert.ToInt32(DocTypeID);
                uploadedDoc.DocUrl = dbSavePath;
                uploadedDoc.CreatedBy = UserID;
                uploadedDoc.CreatedDate = null;
                uploadedDoc.EffectiveEndDate = null;
                uploadedDoc.DocDescription = DocDescription;


                SaveUploadDocl(uploadedDoc);




                DataTable Dt = ConvertExceltoDatatable(_localFileName, vAGT_CODE);
                //BulkCopy(Dt);
                if (vAGT_CODE == "DPTSManualUpload")
                {
                    BulkCopyDPTS(Dt);
                }
                else if (vAGT_CODE == "ReceiptManualUpload")
                {
                    BulkCopyRECEIPT(Dt);
                }


                // Create response
                return new FileUploadResult
                {
                    LocalFilePath = _localFileName,

                    FileName = Path.GetFileName(_localFileName),

                    FileLength = new FileInfo(_localFileName).Length
                };


            }
            catch (Exception ex)
            {

                throw;
            }
        }


        [MimeMultipart]
        [TDABasicAuthenticationFilter(false)]
        public async Task<FileUploadResult> UploadExcel(string vAGT_CODE)
        {
            try
            {


                var uploadPath = CommissionSystemREST.Properties.Settings.Default.Commission_Docs;


                string year = DateTime.Now.Year.ToString();

                var dbSavePath = "/" + year + "/" + vAGT_CODE;

                uploadPath = uploadPath + "/" + year + "/" + vAGT_CODE;
                if (!Directory.Exists(uploadPath))
                {
                    System.IO.Directory.CreateDirectory(uploadPath);
                }

                var multipartFormDataStreamProvider = new UploadMultipartFormProvider(uploadPath);



                // Read the MIME multipart asynchronously 
                await Request.Content.ReadAsMultipartAsync(multipartFormDataStreamProvider);

                string AgentCode = multipartFormDataStreamProvider.FormData["AgentCode"];

                string DocTypeID = multipartFormDataStreamProvider.FormData["DocTypeID"];

                string UserID = multipartFormDataStreamProvider.FormData["UserID"];

                string DocDescription = multipartFormDataStreamProvider.FormData["DocDescription"];

                string _localFileName = multipartFormDataStreamProvider
                    .FileData.Select(multiPartData => multiPartData.LocalFileName).FirstOrDefault();

                dbSavePath = dbSavePath + "/" + multipartFormDataStreamProvider
                    .FileData.Select(multiPartData => multiPartData.Headers.ContentDisposition.FileName).FirstOrDefault().Replace("\"", "");



                DataTable Dt = ConvertExceltoDatatable(_localFileName, vAGT_CODE);   // DPTSManualUpload   /   ReceiptManualUpload

                if (vAGT_CODE == "DPTSManualUpload")
                {
                    BulkCopyDPTS(Dt);
                }
                else if (vAGT_CODE == "ReceiptManualUpload")
                {
                    BulkCopyRECEIPT(Dt);
                }



                UploadDoc uploadedDoc = new UploadDoc();
                uploadedDoc.Id = 0;
                uploadedDoc.AgtCode = AgentCode;
                uploadedDoc.DocTypeId = Convert.ToInt32(DocTypeID);
                uploadedDoc.DocUrl = dbSavePath;
                uploadedDoc.CreatedBy = UserID;
                uploadedDoc.CreatedDate = null;
                uploadedDoc.EffectiveEndDate = null;
                uploadedDoc.DocDescription = vAGT_CODE;    // DPTSManualUpload   /   ReceiptManualUpload


                SaveUploadDocl(uploadedDoc);


                // Create response
                return new FileUploadResult
                {
                    LocalFilePath = _localFileName,

                    FileName = Path.GetFileName(_localFileName),

                    FileLength = new FileInfo(_localFileName).Length
                };



            }
            catch (Exception ex)
            {

                throw;
            }
        }



        public void BulkCopyDPTS(DataTable Dt)
        {
            OracleConnection Oracleconn = new OracleConnection(WebConfigurationManager.ConnectionStrings["OracleConString"].ConnectionString);
            Oracleconn.Open();

            using (OracleBulkCopy bulkcopy = new OracleBulkCopy(Oracleconn))
            {

                bulkcopy.ColumnMappings.Add(new OracleBulkCopyColumnMapping("DPTS_RECEIPT_NO", "DPTS_RECEIPT_NO"));
                bulkcopy.ColumnMappings.Add(new OracleBulkCopyColumnMapping("DPTS_POLICY_NO", "DPTS_POLICY_NO"));
                bulkcopy.ColumnMappings.Add(new OracleBulkCopyColumnMapping("DPTS_PREM_RECEIPT_NO", "DPTS_PREM_RECEIPT_NO"));
                bulkcopy.ColumnMappings.Add(new OracleBulkCopyColumnMapping("DPTS_RECEIPT_AMT", "DPTS_RECEIPT_AMT"));
                bulkcopy.ColumnMappings.Add(new OracleBulkCopyColumnMapping("DPTS_TIME_SLAB", "DPTS_TIME_SLAB"));
                bulkcopy.ColumnMappings.Add(new OracleBulkCopyColumnMapping("DPTS_AGENT", "DPTS_AGENT"));
                bulkcopy.ColumnMappings.Add(new OracleBulkCopyColumnMapping("DPTS_TABLE", "DPTS_TABLE"));
                bulkcopy.ColumnMappings.Add(new OracleBulkCopyColumnMapping("DPTS_TERM", "DPTS_TERM"));
                bulkcopy.ColumnMappings.Add(new OracleBulkCopyColumnMapping("DPTS_MODE", "DPTS_MODE"));
                bulkcopy.ColumnMappings.Add(new OracleBulkCopyColumnMapping("DPTS_TRANSFER_AMT", "DPTS_TRANSFER_AMT"));
                bulkcopy.ColumnMappings.Add(new OracleBulkCopyColumnMapping("DPTS_VARIANCE", "DPTS_VARIANCE"));
                bulkcopy.ColumnMappings.Add(new OracleBulkCopyColumnMapping("DPTS_COMM_MONTH", "DPTS_COMM_MONTH"));
                bulkcopy.ColumnMappings.Add(new OracleBulkCopyColumnMapping("DPTS_COMM_YEAR", "DPTS_COMM_YEAR"));
                //bulkcopy.ColumnMappings.Add(new OracleBulkCopyColumnMapping("DPTS_TIME_SLAB_INDEX", "TFR_AMT1"));
                bulkcopy.ColumnMappings.Add(new OracleBulkCopyColumnMapping("DPTS_PROCESS_IND", "DPTS_PROCESS_IND"));


                bulkcopy.DestinationTableName = "HCI_TBL_DPTS_TEMP";
                try
                {
                    bulkcopy.WriteToServer(Dt);
                    Oracleconn.Close();

                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        public void BulkCopyRECEIPT(DataTable Dt)
        {
            OracleConnection Oracleconn = new OracleConnection(WebConfigurationManager.ConnectionStrings["OracleConString"].ConnectionString);
            Oracleconn.Open();

            using (OracleBulkCopy bulkcopy = new OracleBulkCopy(Oracleconn))
            {


                bulkcopy.ColumnMappings.Add(new OracleBulkCopyColumnMapping("PID_RECEIPT_DATE", "PID_RECEIPT_DATE"));
                bulkcopy.ColumnMappings.Add(new OracleBulkCopyColumnMapping("PID_CUSTOMER_NAME", "PID_CUSTOMER_NAME"));
                bulkcopy.ColumnMappings.Add(new OracleBulkCopyColumnMapping("PID_RECEIPT_NO", "PID_RECEIPT_NO"));
                bulkcopy.ColumnMappings.Add(new OracleBulkCopyColumnMapping("PID_BRANCH", "PID_BRANCH"));
                bulkcopy.ColumnMappings.Add(new OracleBulkCopyColumnMapping("PID_PROPOSAL_NO", "PID_PROPOSAL_NO"));
                bulkcopy.ColumnMappings.Add(new OracleBulkCopyColumnMapping("PID_POLICY_NO", "PID_POLICY_NO"));
                bulkcopy.ColumnMappings.Add(new OracleBulkCopyColumnMapping("PID_RECEIPT_AMT", "PID_RECEIPT_AMT"));
                bulkcopy.ColumnMappings.Add(new OracleBulkCopyColumnMapping("PID_PAYMENT_MTD", "PID_PAYMENT_MTD"));
                bulkcopy.ColumnMappings.Add(new OracleBulkCopyColumnMapping("PID_AGT_CODE", "PID_AGT_CODE"));
                bulkcopy.ColumnMappings.Add(new OracleBulkCopyColumnMapping("PID_TABLE", "PID_TABLE"));


                bulkcopy.ColumnMappings.Add(new OracleBulkCopyColumnMapping("PID_TERM", "PID_TERM"));
                bulkcopy.ColumnMappings.Add(new OracleBulkCopyColumnMapping("PID_BANK_CODE", "PID_BANK_CODE"));
                bulkcopy.ColumnMappings.Add(new OracleBulkCopyColumnMapping("PID_MODE", "PID_PAY_FREQ"));

                bulkcopy.ColumnMappings.Add(new OracleBulkCopyColumnMapping("PID_TXN_CODE", "PID_TIME_SLAB"));//CHECK

                bulkcopy.ColumnMappings.Add(new OracleBulkCopyColumnMapping("PID_INSTALLMENT_AMT", "PID_INSTALLMENT_AMT"));
                bulkcopy.ColumnMappings.Add(new OracleBulkCopyColumnMapping("PID_RECEIPT_TYPE", "PID_RECEIPT_TYPE"));
                bulkcopy.ColumnMappings.Add(new OracleBulkCopyColumnMapping("PID_PRODUCT_CODE", "PID_PRODUCT_CODE"));
                bulkcopy.ColumnMappings.Add(new OracleBulkCopyColumnMapping("PID_POLICY_BANK", "PID_POLICY_BANK"));
                bulkcopy.ColumnMappings.Add(new OracleBulkCopyColumnMapping("PID_POLICY_YEAR", "PID_POLICY_YEAR"));

                bulkcopy.ColumnMappings.Add(new OracleBulkCopyColumnMapping("PID_TOT_AMT", "PID_TOTAL_AMT"));
                bulkcopy.ColumnMappings.Add(new OracleBulkCopyColumnMapping("PID_SYS_OR_MAN", "PID_SYS_MAN"));
                bulkcopy.ColumnMappings.Add(new OracleBulkCopyColumnMapping("PID_RV_NO", "PID_RV_NO"));
                //bulkcopy.ColumnMappings.Add(new OracleBulkCopyColumnMapping("PID_RECEIPT_BRANCH", "PID_BRANCH"));


                bulkcopy.DestinationTableName = "hci_tbl_pid_dm_19_02_2018_b";
                try
                {
                    bulkcopy.WriteToServer(Dt);
                    Oracleconn.Close();

                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }


        public DataTable ConvertExceltoDatatable(string location, string UploadType)
        {
            try
            {


                DateTime CurrentDate = DateTime.Now;

                DataTable ds = new DataTable();

                OleDbCommand excelCommand = new OleDbCommand();
                OleDbDataAdapter excelDataAdapter = new OleDbDataAdapter();


                string excelConnStr = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source='" + location + "';Extended Properties= 'Excel 8.0;HDR=Yes;IMEX=1'";

                OleDbConnection excelConn = new OleDbConnection(excelConnStr);

                excelConn.Open();

                DataTable dtPatterns = new DataTable();

                //string usrName = "deshapriya.sooriyaTest";//Remove Please
                //excelCommand = new OleDbCommand("SELECT `SERIAL`".ToString() + "as SERIAL,`TRANSACTION_CODE`".ToString() + " as TRANSACTION_CODE,`VOUCHER_DATE`".ToString() + " as VOUCHER_DATE,`PAYMENT_MODE`".ToString() + " as PAYMENT_MODE,`NARRATION`".ToString() + " as NARRATION,`DEBIT_NO`".ToString() + " as DEBIT_NO,`AMOUNT`".ToString() + " as AMOUNT,`CASH_ACCOUNT`".ToString() + " as CASH_ACCOUNT,`POLICY_BRANCH`".ToString() + " as POLICY_BRANCH,`PAYING_PARTY`".ToString() + " as PAYING_PARTY,`INSTRUMENT_NUMBER`".ToString() + " as INSTRUMENT_NUMBER,`INSTRUMENT_DATE` ".ToString() + " as INSTRUMENT_DATE,`STATUS`".ToString() + " as STATUS,'" + usrName + "' as CREATEDBY,'" + CurrentDate + "' as CREATED_DATE, '" + BatchNo + "' as BATCH_NO, 'UPLOADED' as BATCH_STATUS, 'MANUAL' as BATCH_TYPE FROM [UPLOAD$]", excelConn);

                //excelCommand = new OleDbCommand("SELECT `DEPOSIT`".ToString() + "as DEPOSIT,`POLICY_NO`".ToString() + " as POLICY_NO ,`REF_NO`".ToString() + " as REF_NO ,`AMOUNT_NEW`".ToString() + " as AMOUNT, `TYPE`".ToString() + " as TYPE ,`AGENT_CODE`".ToString() + " as AGENT_CODE ,`TABLE`".ToString() + " as TABLE ,`TERM`".ToString() + " as TERM ,`MODE`".ToString() + " as MODE ,`TFR_AMT`".ToString() + " as TFR_AMT ,`VARIANCE`".ToString() + " as VARIANCE  ,`MONTH`".ToString() + " as MONTH   ,`YEAR`".ToString() + " as YEAR   ,`RANK`".ToString() + " as RANK ,`PROC`".ToString() + " as PROC FROM [Sheet1$]", excelConn);

                if (UploadType == "DPTSManualUpload")
                {
                    excelCommand = new OleDbCommand("SELECT `DPTS_RECEIPT_NO`".ToString() + "as DPTS_RECEIPT_NO ,  `DPTS_POLICY_NO`".ToString() + " as DPTS_POLICY_NO, `DPTS_PREM_RECEIPT_NO`".ToString() + " as DPTS_PREM_RECEIPT_NO, `DPTS_RECEIPT_AMT`".ToString() + " as DPTS_RECEIPT_AMT ,`DPTS_TIME_SLAB`".ToString() + " as DPTS_TIME_SLAB,`DPTS_AGENT`".ToString() + " as DPTS_AGENT,`DPTS_TABLE`".ToString() + " as DPTS_TABLE,`DPTS_TERM`".ToString() + " as DPTS_TERM,`DPTS_MODE`".ToString() + " as DPTS_MODE,`DPTS_TRANSFER_AMT`".ToString() + " as DPTS_TRANSFER_AMT,`DPTS_VARIANCE`".ToString() + " as DPTS_VARIANCE,`DPTS_COMM_MONTH`".ToString() + " as DPTS_COMM_MONTH,`DPTS_COMM_YEAR`".ToString() + " as DPTS_COMM_YEAR,`DPTS_PROCESS_IND`".ToString() + " as DPTS_PROCESS_IND FROM [Sheet1$]", excelConn);
                }
                else if (UploadType == "ReceiptManualUpload")
                {
                    excelCommand = new OleDbCommand("SELECT `DATE`".ToString() + "as PID_RECEIPT_DATE ,  + `CUSTOMER_NAME`".ToString() + " as PID_CUSTOMER_NAME, `RECEIPT_NO`".ToString() + " as PID_RECEIPT_NO, `BRANCH_CODE`".ToString() + " as PID_BRANCH ,`PROPOSAL_NO`".ToString() + " as PID_PROPOSAL_NO, `POLICY_NO`".ToString() + " as PID_POLICY_NO, `RECEIPT_AMOUNT`".ToString() + " as PID_RECEIPT_AMT,`PAYMENT_MODE`".ToString() + " as PID_PAYMENT_MTD,`AGENT`".ToString() + " as PID_AGT_CODE,   `TABLE`".ToString() + " as PID_TABLE,`TERM`".ToString() + " as PID_TERM, `BANK_CODE`".ToString() + " as PID_BANK_CODE,`MODE`".ToString().Trim() + " as PID_MODE,`TXN_CODE`".ToString() + " as PID_TXN_CODE  ,`INSTALL_PREM`".ToString() + " as PID_INSTALLMENT_AMT ,`RECEIPT_TYPE`".ToString() + " as PID_RECEIPT_TYPE ,`PRODUCT_CODE`".ToString() + " as PID_PRODUCT_CODE ,`POLICY_BANK`".ToString() + " as PID_POLICY_BANK ,`POLICY_YEAR`".ToString() + " as PID_POLICY_YEAR , `TOT_AMT`".ToString() + " as PID_TOT_AMT , `SYS_OR_MAN`".ToString() + " as PID_SYS_OR_MAN, `RV_NO`".ToString() + " as PID_RV_NO, `PID_RECEIPT_BRANCH`".ToString() + " as PID_RECEIPT_BRANCH  FROM [RECEIPT$]", excelConn);

                    //excelCommand = new OleDbCommand(string.Format(@"SELECT [DATE] FROM [{0}]","RECEIPT$"), excelConn);
                }


                excelDataAdapter.SelectCommand = excelCommand;

                excelDataAdapter.Fill(dtPatterns);

                excelConn.Close();

                return dtPatterns;
            }
            catch (Exception)
            {

                throw;
            }
        }


        [HttpGet]
        public IEnumerable<UploadDoc> getUploadDocByAgentID(string id)
        {
            List<UploadDoc> uploadDocList = new List<UploadDoc>();
            DataTable dataTable = new DataTable();
            OracleDataReader dataReader = null;
            OracleConnection connection = new OracleConnection(ConnectionString);
            OracleCommand command;

            string sql = "SELECT " +
            "CASE WHEN t.ID IS NULL THEN 0  ELSE t.ID END, " +
            "CASE WHEN t.AGT_CODE IS NULL THEN ''  ELSE t.AGT_CODE END, " +
            "CASE WHEN t.DOC_TYPE_ID IS NULL THEN 0  ELSE t.DOC_TYPE_ID END, " +
            "CASE WHEN t.DOC_URL IS NULL THEN ''  ELSE t.DOC_URL END, " +
            "CASE WHEN t.CREATED_BY IS NULL THEN ''  ELSE t.CREATED_BY END, " +
            "CASE WHEN t.CREATED_DATE IS NULL THEN to_date('01/01/1900', 'DD/MM/RRRR')  ELSE t.CREATED_DATE END, " +
            "CASE WHEN t.EFFECTIVE_END_DATE IS NULL THEN to_date('01/01/1900', 'DD/MM/RRRR')  ELSE to_date(t.EFFECTIVE_END_DATE, 'DD/MM/RRRR') END, DT.DOC_TYPE_NAME,t.doc_description   " +
            "FROM HCI_TBL_UPLOADED_DOC t, Hci_Tbl_Uploaded_Doc_Type DT WHERE T.AGT_CODE=:V_AGT_CODE AND DT.doc_type_id = T.doc_type_id AND t.EFFECTIVE_END_DATE is null";

            command = new OracleCommand(sql, connection);
            command.Parameters.Add(new OracleParameter("V_AGT_CODE", id));

            try
            {
                connection.Open();
                dataReader = command.ExecuteReader();
                dataTable.Load(dataReader);
                dataReader.Close();
                connection.Close();
                uploadDocList = (from DataRow drow in dataTable.Rows
                                 select new UploadDoc()
                                 {
                                     Id = Convert.ToInt32(drow[0]),
                                     AgtCode = drow[1].ToString(),
                                     DocTypeId = Convert.ToInt32(drow[2]),
                                     DocUrl = drow[3].ToString(),
                                     CreatedBy = drow[4].ToString(),
                                     CreatedDate = drow[5].ToString(),
                                     EffectiveEndDate = drow[6].ToString(),
                                     DocTypeDesc = drow[7].ToString(),
                                     DocDescription = drow[8].ToString()
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
            return uploadDocList;

        }





        //Manual Upload
        [HttpGet]
        public IEnumerable<UploadDoc> getUploadDocByType(string Type)
        {
            List<UploadDoc> uploadDocList = new List<UploadDoc>();
            DataTable dataTable = new DataTable();
            OracleDataReader dataReader = null;
            OracleConnection connection = new OracleConnection(ConnectionString);
            OracleCommand command;

            string sql = "SELECT " +
            "CASE WHEN t.ID IS NULL THEN 0  ELSE t.ID END, " +
            "CASE WHEN t.AGT_CODE IS NULL THEN ''  ELSE t.AGT_CODE END, " +
            "CASE WHEN t.DOC_TYPE_ID IS NULL THEN 0  ELSE t.DOC_TYPE_ID END, " +
            "CASE WHEN t.DOC_URL IS NULL THEN ''  ELSE t.DOC_URL END, " +
            "CASE WHEN t.CREATED_BY IS NULL THEN ''  ELSE t.CREATED_BY END, " +
            "CASE WHEN t.CREATED_DATE IS NULL THEN to_date('01/01/1900', 'DD/MM/RRRR')  ELSE t.CREATED_DATE END, " +
            "CASE WHEN t.EFFECTIVE_END_DATE IS NULL THEN to_date('01/01/1900', 'DD/MM/RRRR')  ELSE to_date(t.EFFECTIVE_END_DATE, 'DD/MM/RRRR') END, DT.DOC_TYPE_NAME,t.doc_description   " +
            "FROM HCI_TBL_UPLOADED_DOC t, Hci_Tbl_Uploaded_Doc_Type DT WHERE T.DOC_DESCRIPTION=:V_TYPE AND DT.doc_type_id = T.doc_type_id AND t.EFFECTIVE_END_DATE is null order by t.CREATED_DATE desc";

            command = new OracleCommand(sql, connection);
            command.Parameters.Add(new OracleParameter("V_TYPE", Type));

            try
            {
                connection.Open();
                dataReader = command.ExecuteReader();
                dataTable.Load(dataReader);
                dataReader.Close();
                connection.Close();
                uploadDocList = (from DataRow drow in dataTable.Rows
                                 select new UploadDoc()
                                 {
                                     Id = Convert.ToInt32(drow[0]),
                                     AgtCode = drow[1].ToString(),
                                     DocTypeId = Convert.ToInt32(drow[2]),
                                     DocUrl = drow[3].ToString(),
                                     CreatedBy = drow[4].ToString(),
                                     CreatedDate = drow[5].ToString(),
                                     EffectiveEndDate = drow[6].ToString(),
                                     DocTypeDesc = drow[7].ToString(),
                                     DocDescription = drow[8].ToString()
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
            return uploadDocList;

        }







        [HttpGet]
        public IEnumerable<UploadDoc> getAttachedAgentsUploadDocByAgentID(string id)
        {
            List<UploadDoc> uploadDocList = new List<UploadDoc>();
            DataTable dataTable = new DataTable();
            OracleDataReader dataReader = null;
            OracleConnection connection = new OracleConnection(ConnectionString);
            OracleCommand command;

            string sql = "SELECT " +
            "CASE WHEN t.ID IS NULL THEN 0  ELSE t.ID END, " +
            "t.AGT_CODE || ' - ' || A.AGT_FULL_NAME, " +
            "CASE WHEN t.DOC_TYPE_ID IS NULL THEN 0  ELSE t.DOC_TYPE_ID END, " +
            "CASE WHEN t.DOC_URL IS NULL THEN ''  ELSE t.DOC_URL END, " +
            "CASE WHEN t.CREATED_BY IS NULL THEN ''  ELSE t.CREATED_BY END, " +
            "CASE WHEN t.CREATED_DATE IS NULL THEN to_date('01/01/1900', 'DD/MM/RRRR')  ELSE t.CREATED_DATE END, " +
            "CASE WHEN t.EFFECTIVE_END_DATE IS NULL THEN to_date('01/01/1900', 'DD/MM/RRRR')  ELSE to_date(t.EFFECTIVE_END_DATE, 'DD/MM/RRRR') END, DT.DOC_TYPE_NAME,t.doc_description   " +
            "FROM HCI_TBL_UPLOADED_DOC t, Hci_Tbl_Uploaded_Doc_Type DT , HCI_TBL_AGENT A  WHERE A.AGT_LEADER_AGENT_CODE_V=:V_AGT_CODE AND DT.doc_type_id = T.doc_type_id AND t.EFFECTIVE_END_DATE is null AND A.AGT_CODE = T.AGT_CODE AND A.AGT_EFFECTIVE_END_DATE IS NULL ";

            command = new OracleCommand(sql, connection);
            command.Parameters.Add(new OracleParameter("V_AGT_CODE", id));

            try
            {
                connection.Open();
                dataReader = command.ExecuteReader();
                dataTable.Load(dataReader);
                dataReader.Close();
                connection.Close();
                uploadDocList = (from DataRow drow in dataTable.Rows
                                 select new UploadDoc()
                                 {
                                     Id = Convert.ToInt32(drow[0]),
                                     AgtCode = drow[1].ToString(),
                                     DocTypeId = Convert.ToInt32(drow[2]),
                                     DocUrl = "http://192.168.10.172:8082/comm_docs" + drow[3].ToString(),
                                     CreatedBy = drow[4].ToString(),
                                     CreatedDate = drow[5].ToString(),
                                     EffectiveEndDate = drow[6].ToString(),
                                     DocTypeDesc = drow[7].ToString(),
                                     DocDescription = drow[8].ToString()
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
            return uploadDocList;

        }



        [HttpGet]
        public UploadDoc getProfilePicByAgentID(string vAGT_CODE)
        {
            UploadDoc UploadDocObj = new UploadDoc();
            OracleDataReader dataReader = null;
            OracleConnection connection = new OracleConnection(ConnectionString);
            OracleCommand command;

            string sql = "";

            sql = "SELECT " +
                   "CASE WHEN t.ID IS NULL THEN 0  ELSE t.ID END, " +
                   "CASE WHEN t.AGT_CODE IS NULL THEN ''  ELSE t.AGT_CODE END, " +
                   "CASE WHEN t.DOC_TYPE_ID IS NULL THEN 0  ELSE t.DOC_TYPE_ID END, " +
                   "CASE WHEN t.DOC_URL IS NULL THEN ''  ELSE t.DOC_URL END, " +
                   "CASE WHEN t.CREATED_BY IS NULL THEN ''  ELSE t.CREATED_BY END, " +
                   "CASE WHEN t.CREATED_DATE IS NULL THEN to_date('01/01/1900', 'DD/MM/RRRR')  ELSE t.CREATED_DATE END, " +
                   "CASE WHEN t.EFFECTIVE_END_DATE IS NULL THEN to_date('01/01/1900', 'DD/MM/RRRR')  ELSE to_date(t.EFFECTIVE_END_DATE, 'DD/MM/RRRR') END, DT.DOC_TYPE_NAME, t.doc_description   " +
                   "FROM HCI_TBL_UPLOADED_DOC t, Hci_Tbl_Uploaded_Doc_Type DT WHERE T.AGT_CODE=:vAGT_CODE AND DT.doc_type_id = T.doc_type_id AND t.EFFECTIVE_END_DATE is null AND t.DOC_TYPE_ID = 1";


            command = new OracleCommand(sql, connection);
            command.Parameters.Add(new OracleParameter("vAGT_CODE", vAGT_CODE));
            connection.Open();
            try
            {
                dataReader = command.ExecuteReader();
                if (dataReader.HasRows)
                {
                    dataReader.Read();

                    UploadDocObj.Id = Convert.ToInt32(dataReader[0]);
                    UploadDocObj.AgtCode = dataReader[1].ToString();
                    UploadDocObj.DocTypeId = Convert.ToInt32(dataReader[2].ToString());
                    UploadDocObj.DocUrl = dataReader[3].ToString();
                    UploadDocObj.CreatedBy = dataReader[4].ToString();
                    UploadDocObj.CreatedDate = dataReader[5].ToString();
                    UploadDocObj.EffectiveEndDate = dataReader[6].ToString();
                    UploadDocObj.DocTypeDesc = dataReader[7].ToString();
                    UploadDocObj.DocDescription = dataReader[8].ToString();

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
            return UploadDocObj;
        }


        [HttpGet]
        public UploadDoc getLeader_VProfilePicByAgentID(string vAGT_CODE)
        {
            UploadDoc UploadDocObj2 = new UploadDoc();
            OracleDataReader dataReader2 = null;
            OracleConnection connection2 = new OracleConnection(ConnectionString);
            OracleCommand command2;

            string sql = "";


            sql = "SELECT " +
                   "CASE WHEN t.ID IS NULL THEN 0  ELSE t.ID END, " +
                   "t.AGT_CODE || ' - ' || A.AGT_FULL_NAME, " +
                   "CASE WHEN t.DOC_TYPE_ID IS NULL THEN 0  ELSE t.DOC_TYPE_ID END, " +
                   "CASE WHEN t.DOC_URL IS NULL THEN ''  ELSE t.DOC_URL END, " +
                   "CASE WHEN t.CREATED_BY IS NULL THEN ''  ELSE t.CREATED_BY END, " +
                   "CASE WHEN t.CREATED_DATE IS NULL THEN to_date('01/01/1900', 'DD/MM/RRRR')  ELSE t.CREATED_DATE END, " +
                   "CASE WHEN t.EFFECTIVE_END_DATE IS NULL THEN to_date('01/01/1900', 'DD/MM/RRRR')  ELSE to_date(t.EFFECTIVE_END_DATE, 'DD/MM/RRRR') END, DT.DOC_TYPE_NAME, t.doc_description   " +
                   "FROM HCI_TBL_UPLOADED_DOC t, Hci_Tbl_Uploaded_Doc_Type DT, HCI_TBL_AGENT A " +
                   "WHERE A.AGT_CODE=:vAGT_CODE AND DT.doc_type_id = T.doc_type_id AND t.EFFECTIVE_END_DATE is null AND t.DOC_TYPE_ID = 1 AND A.AGT_CODE = T.AGT_CODE AND A.AGT_EFFECTIVE_END_DATE IS NULL ";



            command2 = new OracleCommand(sql, connection2);
            command2.Parameters.Add(new OracleParameter("vAGT_CODE", vAGT_CODE));
            connection2.Open();
            try
            {
                dataReader2 = command2.ExecuteReader();
                if (dataReader2.HasRows)
                {
                    dataReader2.Read();

                    UploadDocObj2.Id = Convert.ToInt32(dataReader2[0]);
                    UploadDocObj2.AgtCode = dataReader2[1].ToString();
                    UploadDocObj2.DocTypeId = Convert.ToInt32(dataReader2[2].ToString());
                    UploadDocObj2.DocUrl = dataReader2[3].ToString();
                    UploadDocObj2.CreatedBy = dataReader2[4].ToString();
                    UploadDocObj2.CreatedDate = dataReader2[5].ToString();
                    UploadDocObj2.EffectiveEndDate = dataReader2[6].ToString();
                    UploadDocObj2.DocTypeDesc = dataReader2[7].ToString();
                    UploadDocObj2.DocDescription = dataReader2[8].ToString();

                    dataReader2.Close();
                    connection2.Close();
                }
                else
                {
                    return null;
                }
            }
            catch (Exception exception)
            {
                if (dataReader2 != null)
                {
                    dataReader2.Close();
                    
                }
                if (connection2.State == ConnectionState.Open)
                {
                    connection2.Close();
                }
            }
            finally
            {
                connection2.Close();
            }
            return UploadDocObj2;
        }

        [HttpGet]
        public UploadDoc getLeader_HProfilePicByAgentID(string vAGT_CODE)
        {
            UploadDoc UploadDocObj1 = new UploadDoc();
            OracleDataReader dataReader1 = null;
            OracleConnection connection1 = new OracleConnection(ConnectionString);
            OracleCommand command1;

            string sql = "";


            sql = "SELECT " +
                               "CASE WHEN t.ID IS NULL THEN 0  ELSE t.ID END, " +
                               "t.AGT_CODE || ' - ' || A.AGT_FULL_NAME, " +
                               "CASE WHEN t.DOC_TYPE_ID IS NULL THEN 0  ELSE t.DOC_TYPE_ID END, " +
                               "CASE WHEN t.DOC_URL IS NULL THEN ''  ELSE t.DOC_URL END, " +
                               "CASE WHEN t.CREATED_BY IS NULL THEN ''  ELSE t.CREATED_BY END, " +
                               "CASE WHEN t.CREATED_DATE IS NULL THEN to_date('01/01/1900', 'DD/MM/RRRR')  ELSE t.CREATED_DATE END, " +
                               "CASE WHEN t.EFFECTIVE_END_DATE IS NULL THEN to_date('01/01/1900', 'DD/MM/RRRR')  ELSE to_date(t.EFFECTIVE_END_DATE, 'DD/MM/RRRR') END, DT.DOC_TYPE_NAME, t.doc_description   " +
                               "FROM HCI_TBL_UPLOADED_DOC t, Hci_Tbl_Uploaded_Doc_Type DT, HCI_TBL_AGENT A " +
                               "WHERE A.AGT_CODE=:vAGT_CODE AND DT.doc_type_id = T.doc_type_id AND t.EFFECTIVE_END_DATE is null AND t.DOC_TYPE_ID = 1 AND A.AGT_CODE = T.AGT_CODE AND A.AGT_EFFECTIVE_END_DATE IS NULL";



            command1 = new OracleCommand(sql, connection1);
            command1.Parameters.Add(new OracleParameter("vAGT_CODE", vAGT_CODE));
            connection1.Open();
            try
            {
                dataReader1 = command1.ExecuteReader();
                if (dataReader1.HasRows)
                {
                    dataReader1.Read();

                    UploadDocObj1.Id = Convert.ToInt32(dataReader1[0]);
                    UploadDocObj1.AgtCode = dataReader1[1].ToString();
                    UploadDocObj1.DocTypeId = Convert.ToInt32(dataReader1[2].ToString());
                    UploadDocObj1.DocUrl = dataReader1[3].ToString();
                    UploadDocObj1.CreatedBy = dataReader1[4].ToString();
                    UploadDocObj1.CreatedDate = dataReader1[5].ToString();
                    UploadDocObj1.EffectiveEndDate = dataReader1[6].ToString();
                    UploadDocObj1.DocTypeDesc = dataReader1[7].ToString();
                    UploadDocObj1.DocDescription = dataReader1[8].ToString();

                    dataReader1.Close();
                    connection1.Close();
                }
                else
                {
                    return null;
                }
            }
            catch (Exception exception)
            {
                if (dataReader1 != null || connection1.State == ConnectionState.Open)
                {
                    dataReader1.Close();
                    connection1.Close();
                }
            }
            finally
            {
                connection1.Close();
            }
            return UploadDocObj1;
        }
    }
}
