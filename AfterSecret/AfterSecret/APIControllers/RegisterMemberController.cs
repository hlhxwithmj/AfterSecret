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
            try
            {
                var old = UW.RegisterMemberRepository.Get(false).Where(a => a.OpenId == OpenId).SingleOrDefault();
                if (old == null)
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
                }
                else
                {
                    old.AgentCode = model.AgentCode;
                    old.Email = model.Email;
                    old.FirstName = model.FirstName;
                    old.Gender = model.Gender;
                    old.LastName = model.LastName;
                    old.Mobile = model.Mobile;
                    old.Nationality = model.Nationality;
                    old.Occupation = model.Occupation;
                    old.WeChatID = model.WeChatID;
                }
                UW.context.SaveChanges();
                return Ok();
            }
            catch(Exception ex)
            {
                log.Warn(ex);
                return BadRequest();
            }
        }

        public IHttpActionResult Get()
        {
            try
            {
                var result = UW.RegisterMemberRepository.Get().Where(a => a.OpenId == OpenId).SingleOrDefault();
                return Ok(result);
            }
            catch (Exception ex)
            {
                log.Warn(ex);
                return BadRequest();
            }
        }
    }
}
