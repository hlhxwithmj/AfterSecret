using AfterSecret.Filter;
using AfterSecret.Lib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace AfterSecret.APIControllers
{
    //[ApiAuthorize]
    public class TicketController : BaseApiController
    {
        public IHttpActionResult Get()
        {
            try
            {
                var ticket = UW.TicketRepository.Get().Where(a => a.RegisterMember.OpenId == OpenId).SingleOrDefault();

                if (ticket != null)
                    return Ok(new
                    {
                        src = ticket.QRCodePath,
                        name = ticket.RegisterMember.FirstName + " " + ticket.RegisterMember.LastName,
                        ticketId = ticket.Id
                    });
                else
                    return BadRequest();
            }
            catch (Exception ex)
            {
                log.Warn(ex);
                return BadRequest();
            }
        }
    }
}