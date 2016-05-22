using AfterSecret.Filter;
using AfterSecret.Lib;
using AfterSecret.Models.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace AfterSecret.APIControllers
{
    public class CheckInController : ApiController
    {
        public IHttpActionResult Get(string key)
        {
            if (string.IsNullOrEmpty(key))
                return BadRequest("二维码不合法");
            var openId = Common.DesDecrypt(key);
            using (var uw = new UnitOfWork())
            {
                var ticket = uw.TicketRepository.dbSet.Where(a => a.Invitee.OpenId == openId).SingleOrDefault();
                if (ticket != null)
                {
                    if (ticket.IsValidate == true)
                    {
                        ticket.IsValidate = false;
                        uw.context.SaveChanges();
                        return Ok(
                            new
                            {
                                id = ticket.Id,
                                type = ticket.InvitationType,
                                guest = ticket.Invitee.ToString(),
                                mobile = ticket.Invitee.Mobile,
                                host = ticket.Invitation.Inviter.ToString(),
                                table = ticket.TableNo,
                                success = 1
                            });
                    }
                    else
                    {
                        return Ok(
                            new
                            {
                                id = ticket.Id,
                                type = ticket.InvitationType,
                                guest = ticket.Invitee.ToString(),
                                mobile = ticket.Invitee.Mobile,
                                host = ticket.Invitation.Inviter.ToString(),
                                table = ticket.TableNo,
                                success = 0
                            });
                    }
                }
                else
                    return BadRequest("找不到该票据信息");
            }
        }
    }
}