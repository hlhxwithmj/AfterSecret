using AfterSecret.Filter;
using AfterSecret.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using Pingpp;
using AfterSecret.Lib;
using AfterSecret.Models;
using Pingpp.Models;
using System.Transactions;

namespace AfterSecret.APIControllers
{
    [ApiAuthorize]
    public class OrderDeleteController : BaseApiController
    {
        public IHttpActionResult Get(int id)
        {
            try
            {
                var order = UW.OrderRepository.Get(false).Where(a => a.Id == id)
                    .Where(a => a.OpenId == OpenId).SingleOrDefault();
                if (order != null && order.OrderStatus == Models.Constant.OrderStatus.Unpaid)
                {
                    var purchases = order.Purchases.ToList();
                    foreach (var p in purchases)
                    {
                        p.IsValidate = false;
                    }
                    order.IsValidate = false;
                    UW.context.SaveChanges();
                    return Ok();
                }
                else if(order != null && order.OrderStatus == Models.Constant.OrderStatus.Paid)
                    return BadRequest("paid");
            }
            catch (Exception ex)
            {
                log.Warn(ex);

            }
            return BadRequest();
        }
    }
}