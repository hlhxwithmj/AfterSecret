using AfterSecret.Filter;
using AfterSecret.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using Pingpp;
using AfterSecret.Lib;
using AfterSecret.Models;
using Pingpp.Models;
using System.Transactions;
using AfterSecret.Models.Constant;

namespace AfterSecret.APIControllers
{
    [ApiAuthorize]
    public class InviteGuestController : BaseApiController
    {
        public IHttpActionResult Get(InvitationType invitationType)
        {
            try
            {
                var invitation = UW.InvitationRepository.Get().Where(a => a.Inviter.OpenId == OpenId).SingleOrDefault();
                if (invitation != null)
                {
                    var result = new InvitationVM()
                    {
                        Invitees = invitation.Tickets.Where(a => a.InvitationType == invitationType).Select(a => new InviteeVM() { InviteeId = a.InviteeId, Name = a.Invitee.ToString() }).ToList(),
                        InviterId = invitation.InviterId,
                        Inviter = invitation.Inviter.ToString(),
                        InvitationCode = invitationType == InvitationType.Ticket ? invitation.TicketCode : invitation.TableCode,
                        Total = invitationType == InvitationType.Ticket ? invitation.TicketTotal : invitation.TableTotal
                    };
                    return Ok(result);
                }
                return BadRequest();
            }
            catch (Exception ex)
            {
                log.Warn(ex);
                return BadRequest();
            }
        }

        //my seat
        public IHttpActionResult Post([FromBody]InvitationType invitationType)
        {
            try
            {
                var invitation = UW.InvitationRepository.Get(false)
                    .Where(a => a.Inviter.OpenId == OpenId).SingleOrDefault();
                if (invitationType == InvitationType.Table && invitation.TableRemain < 1)
                {
                    return BadRequest();
                }
                else if (invitationType == InvitationType.Ticket && invitation.TicketRemain < 1)
                {
                    return BadRequest();
                }
                else if (invitation != null)//校验是否是自己的票
                {
                    //add ticket
                    var path = Common.GenerateQRImage(OpenId);
                    UW.TicketRepository.Insert(new Ticket()
                    {
                        QRCodePath = path,
                        InvitationId = invitation.Id,
                        InviteeId = invitation.InviterId,
                        InvitationType = invitationType
                    });
                    UW.context.SaveChanges();
                    return Ok();
                }
                return BadRequest();
            }
            catch (Exception ex)
            {
                log.Warn(ex);
                return BadRequest();
            }
        }

        //cancel
        public IHttpActionResult Get(int inviteeId)
        {
            try
            {
                var model = UW.TicketRepository.Get().Where(a => a.Invitation.Inviter.OpenId == OpenId)
                    .Where(a => a.InviteeId == inviteeId).SingleOrDefault();
                if (model != null)
                {
                    UW.TicketRepository.Delete(model);
                    UW.context.SaveChanges();
                    return Ok();
                }
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