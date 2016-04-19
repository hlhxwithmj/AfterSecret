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
    public class OrderProcessingController : BaseApiController
    {
        public IHttpActionResult Get(string chargeId)
        {
            var order = UW.OrderRepository.Get(false).Where(a => a.OpenId == OpenId)
                .Where(a => a.ChargeId == chargeId).Where(a => a.OrderStatus == Models.Constant.OrderStatus.Unpaid).SingleOrDefault();
            if (order != null)
            {
                order.OrderStatus = Models.Constant.OrderStatus.Processing;
                UW.context.SaveChanges();
                return Ok();
            }
            else
                return BadRequest();
        }
    }
}