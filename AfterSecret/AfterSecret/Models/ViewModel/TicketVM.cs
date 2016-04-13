using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AfterSecret.Models.ViewModel
{
    public class TicketVM
    {
        public int purchaseId { get; set; }

        public int seats { get; set; }

        public string ticketCode { get; set; }

        public string inviter { get; set; }

        public List<AttendeeVM> attendees { get; set; }
    }
}