using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace AfterSecret.Models
{
    public class AccessToken:BaseModel
    {
        [MaxLength(1024)]
        public string Token { get; set; }

        [Column(TypeName = "DateTime2")]
        public DateTime ExpireTime { get; set; }

        [MaxLength(20)]
        public string ErrCode { get; set; }

        [MaxLength(200)]
        public string ErrMsg { get; set; }

        [Column(TypeName = "DateTime2")]
        public DateTime EditTime { get; set; }

        //ErrCode与ErrMsg同时为null，且失效时间大于当前时间加上60秒，则返回true
        public bool IsValid
        {
            get
            {
                return string.IsNullOrEmpty(ErrCode) && string.IsNullOrEmpty(ErrMsg) && ExpireTime > DateTime.Now.AddSeconds(60);
            }
        }

        public AccessToken()
        {
            EditTime = DateTime.Now;
        }
    }
}