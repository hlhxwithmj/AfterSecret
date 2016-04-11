﻿using AfterSecret.Models.DAL;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AfterSecret.Models
{
    public class Purchase : BaseModel
    {
        public int OrderId { get; set; }
        public virtual Order Order { get; set; }

        public int ItemId { get; set; }
        public virtual Item Item { get; set; }

        [JsonProperty("quantity")]
        public int Quantity { get; set; }

        //剩余
        [JsonProperty("remain")]
        public int Remain
        {
            get
            {
                using (var uw = new UnitOfWork())
                {
                    var tickets = uw.TicketRepository.Get().Where(a => a.PurchaseId == Id).Count();
                    return Quantity - tickets;
                }
            }
        }

        [MaxLength(50)]
        [JsonProperty("ticketCode")]
        public string TicketCode { get; set; }

        public virtual ICollection<Ticket> Tickets { get; set; }
    }
}