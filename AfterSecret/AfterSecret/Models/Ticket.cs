using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AfterSecret.Models
{
    public class Ticket : BaseModel
    {
        public int PurchaseId { get; set; }

        public virtual Purchase Purchase { get; set; }

        [Required]
        [MaxLength(50)]
        public string TicketCode { get; set; }
    }
}