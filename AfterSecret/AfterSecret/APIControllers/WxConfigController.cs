using AfterSecret.Filter;
using AfterSecret.Lib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Security;

namespace AfterSecret.APIControllers
{
    public class WxConfigController : BaseApiController
    {
        public IHttpActionResult Get(string url)
        {
            try
            {
                var ticket = Common.get_jsapi_ticket(SubscribeConfig.APPID, SubscribeConfig.APPSECRET);
                var nonceStr = Guid.NewGuid().ToString();
                var timestamp = (long)(DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalMilliseconds / 1000;
                var signature = string.Empty;

                string string1 = "jsapi_ticket=" + ticket +
                    "&noncestr=" + nonceStr +
                    "&timestamp=" + timestamp +
                    "&url=" + url;
                try
                {
                    signature = FormsAuthentication.HashPasswordForStoringInConfigFile(string1, "SHA1");
                }
                catch (Exception e)
                {
                    return BadRequest(e.Message);
                }

                return Ok(new
                {
                    appId = SubscribeConfig.APPID,
                    nonceStr = nonceStr,
                    signature = signature,
                    timestamp = timestamp
                });
            }
            catch (Exception ex)
            {
                log.Warn(ex);
                return BadRequest();
            }
        }
    }
}