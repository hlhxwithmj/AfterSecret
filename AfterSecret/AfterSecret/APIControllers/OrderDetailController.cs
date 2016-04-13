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
            var result = UW.OrderRepository.Get().Where(a => a.OpenId == OpenId)
                .Where(a => a.Id == id).SingleOrDefault();
            if (result != null)
            {
                var items = result.Purchases.Select(a => new { id = a.ItemId, count = a.Quantity });
                return Ok(items);
            }
            else
                return BadRequest();
        }
    }
}