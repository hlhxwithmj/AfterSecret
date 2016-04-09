using AfterSecret.Filter;
using Pingpp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace AfterSecret.APIControllers
{
    //[ApiAuthorize]
    public class RegisterController : BaseApiController
    {
        public IHttpActionResult Get(string code)
        {
            var model = UW.AgentCodeListRepository.Get().Where(a => a.AgentCode == code).FirstOrDefault();

            if (model != null)
                return Ok();
            else
            {
                var ticket = UW.TicketRepository.Get().Where(a => a.Code == code).FirstOrDefault();
                if (ticket != null)
                    return Ok();
            }
            return BadRequest();
        }
    }
}