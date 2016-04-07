using System;
using System.Collections.Generic;
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

        public decimal Quantity { get; set; }

        //剩余
        public decimal Remain { get; set; }
        public virtual ICollection<Ticket> Tickets { get; set; }
    }
}