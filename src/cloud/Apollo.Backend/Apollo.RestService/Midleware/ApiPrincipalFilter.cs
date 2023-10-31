
using Apollo.Api;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Linq;

namespace Apollo.Service.Middleware
{
    /// <summary>
    /// This filters looks for instances of CpdmApi and VioApi on the conteoller instance and sets
    /// the context principal to them. After execution of this filter, the API instance holds the
    /// principal of the ASP.NET HttpContext.
    /// </summary>
    public class ApiPrincipalFilter : ActionFilterAttribute
    {
        /// <summary>
        /// Sets the principal to API instances members of the controller.
        /// </summary>
        /// <param name="filterContext"></param>
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (filterContext.HttpContext.User?.Identity.IsAuthenticated == true)
            {
                var cpdmApiProp = filterContext.Controller.GetType().GetFields(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic).FirstOrDefault(p => p.FieldType == typeof(ApolloApi));
                if (cpdmApiProp != null)
                {
                    ApolloApi? api = cpdmApiProp.GetValue(filterContext.Controller) as ApolloApi;
                    if (api != null)
                        api.Principal = filterContext.HttpContext.User;
                    else
                        throw new ApplicationException("The instance of ApolloApi could have not been resolved!");
                }
            }
        }

    }
}
