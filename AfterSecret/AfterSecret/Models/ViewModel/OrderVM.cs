using AfterSecret.Models.Constant;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AfterSecret.Models.ViewModel
{
    public class OrderVM
    {
        public string order_no { get; set; }

        public decimal amount { get; set; }

        public OrderStatus orderStatus { get; set; }

        public List<PurchaseVM> purchases { get; set; }
    }
}