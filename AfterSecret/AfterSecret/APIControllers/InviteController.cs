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
                var purchases = UW.PurchaseRepository.Get().Where(a => a.Order.OpenId == OpenId).ToList();
                var result = purchases.Select(a => new TicketVM()
                {
                    purchaseId = a.Id,
                    seats = a.Quantity,
                    ticketCode = a.TicketCode,
                    attendees = a.Tickets.Select(b => new AttendeeVM()
                    {
                        name = b.RegisterMember.FirstName + " " + b.RegisterMember.LastName,
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
    }
}