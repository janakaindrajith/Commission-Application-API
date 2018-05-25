using System.Net.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace MRPSystem.Filters
{
    public class ValidateModelAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            if (actionContext.ModelState.IsValid == false)
            {
                actionContext.Response = actionContext.Request.CreateErrorResponse(
                    System.Net.HttpStatusCode.BadRequest, actionContext.ModelState);
            }

        }
    }
}