using AfterSecret.Filter;
using AfterSecret.Models;
using AfterSecret.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace AfterSecret.APIControllers
{
    //[ApiAuthorize]
    public class RegisterMemberController : BaseApiController
    {
        public IHttpActionResult Post([FromBody]RegisterMemberVM model)
        {
            var result = new RegisterMember()
            {
                AgentCode = model.AgentCode,
                Email = model.Email,
                FirstName = model.FirstName,
                Gender = model.Gender,
                LastName = model.LastName,
                Mobile = model.Mobile,
                Nationality = model.Nationality,
                Occupation = model.Occupation,
                OpenId = OpenId,
                WeChatID = model.WeChatID
            };
            UW.RegisterMemberRepository.Insert(result);
            try
            {
                UW.context.SaveChanges();
                return Ok();
            }
            catch
            {
                return BadRequest();
            }
        }
    }
}
