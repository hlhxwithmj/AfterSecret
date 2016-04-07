using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AfterSecret.Models
{
    public class RegisterMember : BaseMember
    {
        public string AgentCode { get; set; }
        public string openId { get; set; }
    }
}