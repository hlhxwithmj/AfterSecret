using AfterSecret.Filter;
using Pingpp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace AfterSecret.APIControllers
{
    [ApiAuthorize]
    public class RegisterController : BaseApiController
    {
        public IHttpActionResult Get(string code)
        {
            var model = UW.AgentCodeListRepository.Get().Where(a => a.AgentCode == code).FirstOrDefault();
            var count = UW.RegisterMemberRepository.Get().Where(a => a.AgentCode == code).Count();
            if (count >= 150 && code == "963903")
                return BadRequest();
            else if (count >= 50 && code != "963903")
                return BadRequest();
            if (model != null)
                return Ok();
            else
            {
                var ticket = UW.InvitationRepository.Get()
                    .Where(a => a.TicketCode == code || a.TableCode == code).FirstOrDefault();
                if (ticket != null)
                    return Ok();
            }
            return BadRequest();
        }
    }
}