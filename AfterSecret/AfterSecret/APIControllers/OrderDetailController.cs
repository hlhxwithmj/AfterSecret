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
    public class OrderDetailController : BaseApiController
    {
        public IHttpActionResult Get(int id)
        {
            var order = UW.OrderRepository.Get().Where(a => a.OpenId == OpenId)
                .Where(a => a.Id == id).SingleOrDefault();
            if (order != null)
            {

                var purchases = from a in order.Purchases
                                select new
                                {
                                    id = a.Item.Id,
                                    count = a.Quantity
                                };
                return Ok(purchases);
            }
            else
                return BadRequest();
        }
    }
}