using Pingpp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace AfterSecret.APIControllers
{
    public class RegisterController : BaseApiController
    {
        public IHttpActionResult Post([FromBody]string code)
        {
            
            return Ok();
        }
    }
}