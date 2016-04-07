using AfterSecret.Lib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace AfterSecret.Filter
{
    public class ApiAuthorize : AuthorizeAttribute
    {
        public override void OnAuthorization(System.Web.Http.Controllers.HttpActionContext actionContext)
        {
            if (IsAuthorized(actionContext))
            {

            }
            else
            {
                HandleUnauthorizedRequest(actionContext);
            }
        }

        protected override void HandleUnauthorizedRequest(System.Web.Http.Controllers.HttpActionContext actionContext)
        {
            actionContext.Response = new System.Net.Http.HttpResponseMessage(System.Net.HttpStatusCode.Unauthorized);
        }
        protected override bool IsAuthorized(System.Web.Http.Controllers.HttpActionContext actionContext)
        {
            IEnumerable<string> tokens;
            IEnumerable<string> openIds;
            actionContext.Request.Headers.TryGetValues("token", out tokens);
            actionContext.Request.Headers.TryGetValues("openId", out openIds);
            if (tokens == null || tokens.SingleOrDefault() == null || openIds == null || openIds.SingleOrDefault() == null)
                return false;
            return IsTokenValidated(tokens.SingleOrDefault(), openIds.SingleOrDefault());
        }

        public bool IsTokenValidated(string token, string openId)
        {
            var desToken = Common.DesDecrypt(token);
            var desOpenId = Common.DesDecrypt(openId);
            if (desToken == "" || desOpenId == "")
                return false;
            long time;
            TimeSpan s = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0);
            long now = (long)s.TotalMilliseconds;
            if (long.TryParse(desToken.Replace(desOpenId, ""), out time) && now - time < TimeSpan.TicksPerDay / 120000)
                return true;
            else return false;
        }
    }
}