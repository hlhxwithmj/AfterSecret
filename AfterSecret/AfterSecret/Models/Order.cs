using AfterSecret.Lib;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using AfterSecret.Models.Constant;
using AfterSecret.Models.ViewModel;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace AfterSecret.Models
{
    public class Order : BaseModel
    {
        private Dictionary<string, string> _app = new Dictionary<string, string>();
        private Dictionary<string, Object> _param = new Dictionary<String, Object>();
        private Dictionary<String, Object> _extra = new Dictionary<String, Object>();

        public int RegisterMemberId { get; set; }

        public virtual RegisterMember RegisterMember { get; set; }

        [Required]
        [MaxLength(200)]
        public string OpenId { get; set; }

        [Required]
        [MaxLength(200)]
        public string OpenIdForPay { get; set; }

        [Required]
        [MaxLength(50)]
        [JsonProperty("order_no")]
        //订单号
        public string Order_No { get; set; }

        [Required]
        [MaxLength(100)]
        public string ChargeId { get; set; }

        [Required]
        [MaxLength(100)]
        public string AppId { get; set; }

        [JsonProperty("amount")]
        public decimal Amount { get; set; }

        [Required]
        [MaxLength(50)]
        public string Client_Ip { get; set; }

        [Required]
        [MaxLength(20)]
        public string Currency { get; set; }

        [Required]
        [MaxLength(32)]
        public string Subject { get; set; }

        [Required]
        [MaxLength(128)]
        public string Body { get; set; }

        [Required]
        [MaxLength(20)]
        public string Channel { get; set; }

        [Column(TypeName = "DateTime2")]
        public DateTime ExpireTime { get; set; }


        [Column(TypeName = "DateTime2")]
        public DateTime? PaidTime { get; set; }

        [Column(TypeName = "DateTime2")]
        public DateTime CreatedTime { get; set; }

        [Required]
        [JsonProperty("orderStatus")]
        public OrderStatus OrderStatus { get; set; }

        public string FailureCode { get; set; }

        public string FailureMsg { get; set; }

        public virtual ICollection<Purchase> Purchases { get; set; }

        public Dictionary<string, Object> Param
        {
            get
            {
                return _param;
            }
        }

        public Order()
        {

        }

        public Order(int registerMemberId, decimal amount, string body, string ip, string openId, string openIdForPay)
        {
            RegisterMemberId = registerMemberId;
            OrderStatus = OrderStatus.Unpaid;
            OpenIdForPay = openIdForPay;
            OpenId = openId;
            Order_No = Common.GenerateOrderNo(new Random());
            Amount = amount;
            Body = body;
            Subject = PingConfig.SUBJECT;
            Client_Ip = ip;
            ExpireTime = DateTime.Now.AddMinutes(5);
            Channel = "wx_pub";
            Currency = "cny";
            AppId = PingConfig.APPID;
            CreatedTime = DateTime.Now;

            Pingpp.Pingpp.ApiKey = PingConfig.LIVESECRETKEY;
            _app.Add("id", AppId);
            _extra.Add("open_id", OpenIdForPay);
            _param.Add("amount", Amount);
            _param.Add("currency", Currency);
            _param.Add("subject", Subject);
            _param.Add("body", Body);
            _param.Add("order_no", Order_No);
            _param.Add("channel", Channel);
            _param.Add("client_ip", ip);
            _param.Add("time_expire", Common.ConvertToTimestamp(ExpireTime).ToString());
            _param.Add("app", _app);
            _param.Add("extra", _extra);
        }
    }
}