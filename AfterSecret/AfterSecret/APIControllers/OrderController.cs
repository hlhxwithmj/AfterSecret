﻿using AfterSecret.Filter;
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
    public class OrderController : BaseApiController
    {
        public IHttpActionResult Post([FromBody]List<CheckoutList> model)
        {
            var created = UW.OrderRepository.Get().Where(a => a.OpenId == OpenId)
                .Where(a => a.OrderStatus == Models.Constant.OrderStatus.Unpaid).Count();
            if (created > 0)
            {
                return BadRequest("needtopay");
            }
            var member = UW.RegisterMemberRepository.Get().Where(a => a.OpenId == OpenId).SingleOrDefault();
            var openIdForPay = Common.DesDecrypt(this.Request.Headers.GetValues("openIdForPay").SingleOrDefault());
            var items = UW.ItemRepository.Get().ToList();
            var total = items.Sum(a => a.UnitPrice * (model.Where(b => b.Id == a.Id).SingleOrDefault().Count));
            var charge = new Order(member.Id, total, Common.GetChargeBody(items, model), HttpContext.Current.Request.UserHostAddress, OpenId, openIdForPay);
            try
            {
                Charge c = Charge.Create(charge.Param);
                using (var scope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions() { IsolationLevel = IsolationLevel.Snapshot }))
                {
                    charge.ChargeId = c.Id;
                    UW.OrderRepository.Insert(charge);
                    UW.context.SaveChanges();

                    var random = new Random();
                    foreach (var m in model)
                    {
                        if (m.Count > 0)
                            UW.PurchaseRepository.Insert(new Purchase()
                            {
                                ItemId = m.Id,
                                OrderId = charge.Id,
                                Quantity = m.Count,
                            });
                    }
                    UW.context.SaveChanges();
                    if (items.Any(a => a.Remain < 0))
                    {
                        scope.Dispose();
                        return BadRequest();
                    }
                    scope.Complete();
                }
                return Ok(c);
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
                var result = UW.OrderRepository.Get()
                    .Where(a => a.OpenId == OpenId).Where(a => a.OrderStatus != Models.Constant.OrderStatus.Expired)
                    .Select(a => new OrderVM()
                    {
                        amount = a.Amount / 100,
                        id = a.Id,
                        order_no = a.Order_No,
                        orderStatus = a.OrderStatus,
                        expireTime = a.ExpireTime,
                        purchases = a.Purchases.Select(b => new PurchaseVM() { amount = b.Item.UnitPrice * b.Quantity / 100, name = b.Item.Name, quantity = b.Quantity, remark = b.Item.Remark }).ToList()
                    }).ToList();

                return Ok(result);
            }
            catch (Exception ex)
            {
                log.Warn(ex);
                return BadRequest();
            }
        }

        public IHttpActionResult Get(string status)
        {
            log.Warn(status);
            var result = UW.OrderRepository.Get().Where(a => a.OpenId == OpenId);
            int count = 0;
            if (status == "unpaid")
            {
                count = result.Where(a => a.OrderStatus == Models.Constant.OrderStatus.Unpaid).Count();
            }
            else if (status == "expired")
            {
                count = result.Where(a => a.OrderStatus == Models.Constant.OrderStatus.Expired).Count();
            }
            else
                count = result.Count();
            return Ok(count);
        }
    }
}