using AfterSecret.Lib;
using AfterSecret.Models.DAL;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace AfterSecret.APIControllers
{
    public class BaseApiController : ApiController
    {
        protected static readonly ILog log = LogManager.GetLogger(typeof(BaseApiController).FullName);
        protected UnitOfWork UW = new UnitOfWork();
        private string _openId { get; set; }
        public string OpenId
        {
            get
            {
                return "x";
                if (string.IsNullOrEmpty(_openId))
                    _openId = Common.DesDecrypt(this.Request.Headers.GetValues("openId").SingleOrDefault());
                return _openId;
            }
        }

        public BaseApiController()
        {

        }
    }
}