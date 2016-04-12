using AfterSecret.Lib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace AfterSecret.APIControllers
{
    public class ShareController : BaseApiController
    {
        public IHttpActionResult Get()
        {
            var member = UW.RegisterMemberRepository.Get().Where(a => a.OpenId == OpenId).SingleOrDefault();
            if (member != null && member.AgentCode.StartsWith(SubscribeConfig._seedUser_Prefix))
            {
                return Ok();
            }
            else
                return BadRequest();
        }

        public IHttpActionResult Post()
        {
            var code = Common.GenerateShareCode();
            return Ok(code);
        }
    }
}