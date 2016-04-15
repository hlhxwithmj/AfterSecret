using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AfterSecret.Models
{
    public class AgentCodeList : BaseModel
    {
        [Required]
        [MaxLength(50)]
        public string AgentCode { get; set; }

        [MaxLength(200)]
        public string OpenId { get; set; }
    }
}