using System.Web.Http.Controllers;


namespace MRPSystem.Controllers.User
{
    public class TDABasicAuthenticationFilter : BasicAuthenticationFilter
    {

        public TDABasicAuthenticationFilter()
        { }

        public TDABasicAuthenticationFilter(bool active) : base(active)
        { }


        protected override bool OnAuthorizeUser(string username, string password, HttpActionContext actionContext)
        {
            var userAcc = new UserAccountController();

            var user = userAcc.AuthenticateAndLoad(username, password);
            if (user == null)
                return false;

            return true;
        }
    }

}