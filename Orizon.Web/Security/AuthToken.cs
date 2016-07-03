using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Orizon.Web.Security
{
    public sealed class AuthToken
    {

        public static Guid FromRequest(string name)
        {
            var request = HttpContext.Current.Request;

            try
            {
                var header = request.Headers[name];
                if (header != null) { return FromString(header); }

                var cookie = request.Cookies[name];
                if (cookie != null) { return FromString(cookie.Value); }

                return Guid.Empty;
            }
            catch
            {
                return Guid.Empty;
            }
        }

        private static Guid FromString(string token)
        {
            try
            {
                return Guid.Parse(AesDataProtector.Unprotect(token));
            }
            catch
            {
                return Guid.Empty;
            }
        }



        public static void SaveCookie(string name, Guid token)
        {
            var cookie = new HttpCookie(name);
            cookie.HttpOnly = true;
            cookie.Expires = DateTime.Now.AddHours(2);
            cookie.Value = AesDataProtector.Protect(token.ToString());
            HttpContext.Current.Response.SetCookie(cookie);
        }

        public static void ClearCookie(string name)
        {
            var cookie = HttpContext.Current.Request.Cookies[name];
            HttpContext.Current.Response.Cookies.Remove(name);
            cookie.Expires = DateTime.Now.AddDays(-1);
            cookie.Value = null;
            HttpContext.Current.Response.SetCookie(cookie);
        }

    }
}
