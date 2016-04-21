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
    public class InviteController : BaseApiController
    {
        public IHttpActionResult Get()
        {
            try
            {
                var invitation = UW.InvitationRepository.Get().Where(a => a.Inviter.OpenId == OpenId).SingleOrDefault();
                if (invitation != null)
                    return Ok(
                        new InviteVM
                        {
                            HasTable = invitation.TableTotal > 0,
                            HasTicket = invitation.TicketTotal > 0,
                            FullTable = invitation.TableRemain == 0 && invitation.TableTotal > 0,
                            FullTicket = invitation.TicketRemain == 0 && invitation.TicketTotal > 0
                        });
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