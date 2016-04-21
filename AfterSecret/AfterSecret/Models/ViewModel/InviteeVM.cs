using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AfterSecret.Models.ViewModel
{
    public class InviteeVM
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("inviteeId")]
        public int InviteeId { get; set; }
    }
}