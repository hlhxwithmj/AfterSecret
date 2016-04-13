﻿using AfterSecret.Filter;
using AfterSecret.Lib;
using AfterSecret.Models;
using AfterSecret.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Transactions;
using System.Web.Http;

namespace AfterSecret.APIControllers
{
    [ApiAuthorize]
    public class RegisterMemberController : BaseApiController
    {
        public IHttpActionResult Post([FromBody]RegisterMemberVM model)
        {
            try
            {
                var result = UW.RegisterMemberRepository.Get(false).Where(a => a.OpenId == OpenId).SingleOrDefault();
                if (result == null)
                {
                    result = new RegisterMember()
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
                    result.AgentCode = model.AgentCode;
                    result.Email = model.Email;
                    result.FirstName = model.FirstName;
                    result.Gender = model.Gender;
                    result.LastName = model.LastName;
                    result.Mobile = model.Mobile;
                    result.Nationality = model.Nationality;
                    result.Occupation = model.Occupation;
                    result.WeChatID = model.WeChatID;
                }
                UW.context.SaveChanges();
                if (model.AgentCode.StartsWith(SubscribeConfig._invitedUser_Prefix))
                {
                    using (var scope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions() { IsolationLevel = IsolationLevel.Snapshot }))
                    {
                        var purchase = UW.PurchaseRepository.Get().Where(a => a.TicketCode == model.AgentCode).SingleOrDefault();
                        log.Warn(purchase.Remain);
                        if (purchase.Remain > 0)
                        {
                            UW.TicketRepository.Insert(new Ticket()
                            {
                                PurchaseId = purchase.Id,
                                QRCodePath = Common.GenerateQRImage(OpenId),
                                RegisterMemberId = result.Id
                            });
                            UW.context.SaveChanges();
                            scope.Complete();
                        }
                        else
                        {
                            scope.Dispose();
                            return BadRequest();
                        }
                            
                    }
                }
                return Ok();
            }
            catch (Exception ex)
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
