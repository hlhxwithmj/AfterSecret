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
    [ApiAuthorize]
    public class TicketController : BaseApiController
    {
        public IHttpActionResult Get()
        {
            try
            {
                var ticket = UW.TicketRepository.Get().Where(a => a.Invitee.OpenId == OpenId).SingleOrDefault();
                if (ticket != null)
                    return Ok(new
                    {
                        src = SubscribeConfig.DOMAIN + "/Content/QR/" + ticket.QRCodePath,
                        inviter = ticket.Invitee.ToString(),
                        invitee = ticket.Invitation.Inviter.ToString(),
                        ticketId = ticket.Id,
                        invitationType = ticket.InvitationType,
                        inviteeId = ticket.InviteeId
                    });
                else
                    return BadRequest("invite");
            }
            catch (Exception ex)
            {
                log.Warn(ex);
                return BadRequest();
            }
        }
    }
}