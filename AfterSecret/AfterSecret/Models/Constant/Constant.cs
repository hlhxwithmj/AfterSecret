using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AfterSecret.Models.Constant
{
    public enum OrderStatus
    {
        Unpaid = 10,
        Processing = 15,
        Paid = 20,
        Failed = 30,
        Expired = 40
    }

    public enum InvitationType
    {
        Ticket = 10,
        Table = 20
    }
}