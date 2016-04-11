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
        [MaxLength(50)]
        public string AgentCode { get; set; }

        [Required]
        [MaxLength(200)]
        public string OpenId { get; set; }
    }
}