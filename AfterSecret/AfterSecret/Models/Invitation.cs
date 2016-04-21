using AfterSecret.Models.DAL;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace AfterSecret.Models
{
    public class Invitation : BaseModel
    {
        public int InviterId { get; set; }

        public virtual RegisterMember Inviter { get; set; }

        [MaxLength(50)]
        [JsonProperty("ticketCode")]
        public string TicketCode { get; set; }

        [MaxLength(50)]
        [JsonProperty("tableCode")]
        public string TableCode { get; set; }

        public virtual ICollection<Ticket> Tickets { get; set; }

        //Table剩余
        [JsonProperty("tableRemain")]
        public int TableRemain
        {
            get
            {
                using (var uw = new UnitOfWork())
                {
                    var tickets = Tickets.Where(a => a.InvitationType == Constant.InvitationType.Table).Count();
                    return TableTotal - tickets;
                }
            }
        }

        //Ticket剩余
        [JsonProperty("ticketRemain")]
        public int TicketRemain
        {
            get
            {
                using (var uw = new UnitOfWork())
                {
                    var tickets = Tickets.Where(a => a.InvitationType == Constant.InvitationType.Ticket).Count();
                    return TicketTotal - tickets;
                }
            }
        }

        [JsonProperty("ticketTotal")]
        public int TicketTotal
        {
            get
            {
                using (var uw = new UnitOfWork())
                {
                    var orders = uw.OrderRepository.Get().Where(a => a.RegisterMemberId == InviterId)
                        .Where(a => a.OrderStatus == Constant.OrderStatus.Paid).ToList();
                    if (orders.Count > 0)
                    {
                        int count = 0;
                        foreach (var order in orders)
                        {
                            var purchases = order.Purchases.Where(a => a.Item.InvitationType == Constant.InvitationType.Ticket).ToList();
                            count = count + purchases.Sum(a => a.Quantity * a.Item.Seats);
                        }

                        return count;
                    }
                    return 0;
                }
            }
        }

        [JsonProperty("tableTotal")]
        public int TableTotal
        {
            get
            {
                using (var uw = new UnitOfWork())
                {
                    var orders = uw.OrderRepository.Get().Where(a => a.RegisterMemberId == InviterId)
                        .Where(a => a.OrderStatus == Constant.OrderStatus.Paid).ToList();
                    if (orders.Count > 0)
                    {
                        int count = 0;
                        foreach (var order in orders)
                        {
                            var purchases = order.Purchases.Where(a => a.Item.InvitationType == Constant.InvitationType.Table).ToList();
                            count = count + purchases.Sum(a => a.Quantity * a.Item.Seats);
                        }

                        return count;
                    }
                    return 0;
                }
            }

        }
    }
}