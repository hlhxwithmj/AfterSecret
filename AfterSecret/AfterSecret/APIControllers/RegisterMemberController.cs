using AfterSecret.Filter;
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
                        AgentCode = model.AgentCode.Trim(),
                        Email = model.Email.Trim(),
                        FirstName = model.FirstName.Trim(),
                        Gender = model.Gender.Trim(),
                        LastName = model.LastName.Trim(),
                        Mobile = model.Mobile.Trim(),
                        Nationality = model.Nationality.Trim(),
                        Occupation = model.Occupation,
                        OpenId = OpenId,
                        WeChatID = model.WeChatID
                    };
                    UW.RegisterMemberRepository.Insert(result);
                    UW.context.SaveChanges();
                    Random r = new Random();
                    UW.InvitationRepository.Insert(new Invitation() { InviterId = result.Id, TableCode = Common.GenerateTableCode(r), TicketCode = Common.GenerateTicketCode(r) });
                    UW.context.SaveChanges();
                }
                else
                {
                    result.AgentCode = model.AgentCode.Trim();
                    result.Email = model.Email.Trim();
                    result.FirstName = model.FirstName.Trim();
                    result.Gender = model.Gender.Trim();
                    result.LastName = model.LastName.Trim();
                    result.Mobile = model.Mobile.Trim();
                    result.Nationality = model.Nationality.Trim();
                    result.Occupation = model.Occupation;
                    result.WeChatID = model.WeChatID;
                    result.EditTime = DateTime.Now;
                    UW.context.SaveChanges();
                }

                if (model.AgentCode.StartsWith(SubscribeConfig._Table_Invitee_Prefix) || model.AgentCode.StartsWith(SubscribeConfig._Ticket_Invitee_Prefix))
                {
                    using (var scope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions() { IsolationLevel = IsolationLevel.Snapshot }))
                    {
                        var ticket = UW.TicketRepository.Get().Where(a => a.InviteeId == result.Id).ToList();
                        if (ticket.Count > 0)
                        {
                            return BadRequest("ticket");
                        }
                        var invitation = UW.InvitationRepository.Get().Where(a => a.TicketCode == model.AgentCode || a.TableCode == model.AgentCode).SingleOrDefault();
                        log.Warn(invitation.TableRemain);
                        log.Warn(invitation.TicketRemain);
                        if (model.AgentCode.StartsWith(SubscribeConfig._Table_Invitee_Prefix) && invitation.TableRemain > 0)
                        {
                            UW.TicketRepository.Insert(new Ticket()
                            {
                                InvitationType = Models.Constant.InvitationType.Table,
                                InvitationId = invitation.Id,
                                QRCodePath = Common.GenerateQRImage(OpenId),
                                InviteeId = result.Id
                            });
                            UW.context.SaveChanges();
                            scope.Complete();
                        }
                        else if (model.AgentCode.StartsWith(SubscribeConfig._Ticket_Invitee_Prefix) && invitation.TicketRemain > 0)
                        {
                            UW.TicketRepository.Insert(new Ticket()
                            {
                                InvitationType = Models.Constant.InvitationType.Ticket,
                                InvitationId = invitation.Id,

                                QRCodePath = Common.GenerateQRImage(OpenId),
                                InviteeId = result.Id
                            });
                            UW.context.SaveChanges();
                            scope.Complete();
                        }
                        else
                        {
                            scope.Dispose();
                            return BadRequest("fail");
                        }
                    }
                    return Ok("success");
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
                if (result != null)
                    return Ok(result);
                else
                    return BadRequest();
            }
            catch (Exception ex)
            {
                log.Warn(ex);
                return BadRequest();
            }
        }
    }
}
