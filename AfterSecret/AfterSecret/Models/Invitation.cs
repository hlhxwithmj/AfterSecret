using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AfterSecret.Models
{
    public class Invitation : BaseMember
    {
        public int TicketId { get; set; }

        public virtual Ticket Ticket { get; set; }
    }
}