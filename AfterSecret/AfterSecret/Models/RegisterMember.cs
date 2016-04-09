using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AfterSecret.Models
{
    public class RegisterMember : BaseMember
    {
        [Required]
        public string AgentCode { get; set; }

        [Required]
        public string OpenId { get; set; }
    }
}