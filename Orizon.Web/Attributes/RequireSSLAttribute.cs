using System.Web.Mvc;

namespace Orizon.Web.Attributes
{
    class RequireSSLAttribute : RequireHttpsAttribute
    {
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            if (filterContext != null && filterContext.HttpContext.Request.IsLocal)
            {
                return;
            }
            base.OnAuthorization(filterContext);
        }
    }
}
