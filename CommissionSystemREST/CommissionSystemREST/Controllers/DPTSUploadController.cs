using System.Configuration;
using System.Web.Http;


namespace ComissionWebAPI.Controllers
{
    public class DPTSUploadController : ApiController
    {
        static string ConnectionString = ConfigurationManager.ConnectionStrings["OracleConString"].ToString();


    }
}
