using AfterSecret.Lib;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AfterSecret.Models
{
    public class Charge : BaseModel
    {
        private Dictionary<string, string> _app = new Dictionary<string, string>();
        private Dictionary<string, Object> _param = new Dictionary<String, Object>();
        private Dictionary<String, Object> _extra = new Dictionary<String, Object>();

        [Required]
        public string OpenId { get; set; }

        [Required]
        //订单号
        public string Order_No { get; set; }

        [Required]
        public string AppId { get; set; }

        public decimal Amount { get; set; }

        [Required]
        public string Client_Ip { get; set; }

        [Required]
        public string Currency { get; set; }

        [Required]
        public string Subject { get; set; }

        [Required]
        public string Body { get; set; }

        [Required]
        public string Channel { get; set; }

        public DateTime ExpireTime { get; set; }

        public Dictionary<string, Object> Param
        {
            get
            {
                return _param;
            }
        }

        public Charge()
        {

        }

        public Charge(decimal Amount, string Subject, string Body, string Ip, string openId)
        {
            _app.Add("id", PingConfig.APPID);
            _extra.Add("open_id", openId);
            _param.Add("amount", Amount);
            _param.Add("currency", "cny");
            _param.Add("subject", Subject);
            _param.Add("body", Body);
            _param.Add("order_no", "123456789");
            _param.Add("channel", "wx_pub");
            _param.Add("client_ip", Ip);
            _param.Add("time_expire", Common.ConvertToTimestamp(DateTime.Now.AddMinutes(5)).ToString());
            _param.Add("app", _param);
            _param.Add("extra", _extra);
        }
    }
}