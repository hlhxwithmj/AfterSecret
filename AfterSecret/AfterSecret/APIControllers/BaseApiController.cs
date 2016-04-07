using AfterSecret.Models.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace AfterSecret.APIControllers
{
    public class BaseApiController : ApiController
    {
        protected UnitOfWork UW = new UnitOfWork();
        public BaseApiController()
        {

        }
    }
}