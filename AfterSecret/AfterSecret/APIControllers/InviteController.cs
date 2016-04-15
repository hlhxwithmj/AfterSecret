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

namespace AfterSecret.APIControllers
{
    //[ApiAuthorize]
    public class InviteController : BaseApiController
    {       
        public IHttpActionResult Get()
        {
            try
            {
                var purchases = UW.PurchaseRepository.Get().Where(a => a.Order.OpenId == OpenId)
                    .Where(a => a.Item.NeedInvite == true).ToList();
                var result = purchases.Select(a => new TicketVM()
                {
                    purchaseId = a.Id,
                    seats = a.Quantity,
                    ticketCode = a.TicketCode,
                    inviter = a.Order.RegisterMember.ToString(),
                    attendees = a.Tickets.Select(b => new AttendeeVM()
                    {
                        name = b.RegisterMember.ToString(),
                        registerMemberId = b.RegisterMemberId
                    }).ToList()
                });
                return Ok(result);
            }
            catch (Exception ex)
            {
                log.Warn(ex);
                return BadRequest();
            }
        }

        //my seat
        public IHttpActionResult Post([FromBody]int purchaseId)
        {
            try
            {
                var purchase = UW.PurchaseRepository.Get(false).Where(a => a.Order.OpenId == OpenId)
                    .Where(a => a.Id == purchaseId).SingleOrDefault();
                if (purchase.Remain < 1)
                {
                    return BadRequest();
                }
                else if (purchase != null)//校验是否是自己的票
                {

                    var registerMember = UW.RegisterMemberRepository.Get().Where(a => a.OpenId == OpenId).SingleOrDefault();
                    //delete ticket
                    var oldtickets = UW.TicketRepository.Get().Where(a => a.RegisterMemberId == registerMember.Id).ToList();
                    foreach (var old in oldtickets)
                        UW.TicketRepository.Delete(old);
                    //add ticket
                    var path = Common.GenerateQRImage(OpenId);
                    UW.TicketRepository.Insert(new Ticket() { PurchaseId = purchase.Id, RegisterMemberId = registerMember.Id, QRCodePath = path });
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
        public IHttpActionResult Get(int registerMemberId)
        {
            try
            {
                var model = UW.TicketRepository.Get().Where(a => a.Purchase.Order.OpenId == OpenId)
                    .Where(a => a.RegisterMemberId == registerMemberId).SingleOrDefault();
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