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
                var order = UW.OrderRepository.Get().Where(a => a.Id == id)
                    .Where(a => a.OrderStatus == Models.Constant.OrderStatus.Unpaid)
                    .Where(a => a.OpenId == OpenId).SingleOrDefault();
                if (order != null)
                {
                    var purchases = order.Purchases.ToList();
                    foreach (var p in purchases)
                    {
                        UW.PurchaseRepository.Delete(p);
                    }
                    UW.context.SaveChanges();
                    UW.OrderRepository.Delete(order);
                    UW.context.SaveChanges();
                    return Ok();
                }
            }
            catch (Exception ex)
            {
                log.Warn(ex);

            }
            return BadRequest();
        }
    }
}