using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AfterSecret.Models.ViewModel
{
    public class PurchaseVM
    {
        public string name { get; set; }

        public string remark { get; set; }

        public int quantity { get; set; }

        public decimal amount { get; set; }
    }
}