using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AfterSecret.Models
{
    public class BaseMember : BaseModel
    {
        [Required]
        [MaxLength(100)]
        [JsonProperty("firstName")]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(100)]
        [JsonProperty("lastName")]
        public string LastName { get; set; }

        [Required]
        [MaxLength(20)]
        [JsonProperty("gender")]
        public string Gender { get; set; }

        [Required]
        [MaxLength(100)]
        [JsonProperty("nationality")]
        public string Nationality { get; set; }

        [Required]
        [MaxLength(25)]
        [JsonProperty("mobile")]
        public string Mobile { get; set; }

        [Required]
        [MaxLength(100)]
        [JsonProperty("email")]
        public string Email { get; set; }

        [MaxLength(100)]
        [JsonProperty("wechatID")]
        public string WeChatID { get; set; }

        [MaxLength(100)]
        [JsonProperty("occupation")]
        public string Occupation { get; set; }
    }
}