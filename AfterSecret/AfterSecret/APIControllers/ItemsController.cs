using AfterSecret.Filter;
using Pingpp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace AfterSecret.APIControllers
{
    [ApiAuthorize]
    public class ItemsController : BaseApiController
    {
        public IHttpActionResult Get()
        {
            var list = UW.ItemRepository.Get().ToList();
            return Ok(list);
        }
    }
}