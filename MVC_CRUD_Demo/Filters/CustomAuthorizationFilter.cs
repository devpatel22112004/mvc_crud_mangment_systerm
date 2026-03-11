using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace MVC_CRUD_Demo.Filters
{
    public class CustomAuthorizationFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            // Check if user is logged in via Session
            if (HttpContext.Current.Session["Username"] == null)
            {
                // User is not logged in, redirect to Login page
                filterContext.Result = new RedirectToRouteResult(
                    new RouteValueDictionary
                    {
                        { "controller", "Account" },
                        { "action", "Login" }
                    });
            }
            base.OnActionExecuting(filterContext);
        }
    }
}
