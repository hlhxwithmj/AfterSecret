using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AfterSecret.Models
{
    public class Order : BaseModel
    {
        [Required]
        public string OpenId { get; set; }

        [Required]
        //订单号
        public string Order_No { get; set; }

        public int ChargeId { get; set; }
        public virtual Charge Charge { get; set; }
    }
}