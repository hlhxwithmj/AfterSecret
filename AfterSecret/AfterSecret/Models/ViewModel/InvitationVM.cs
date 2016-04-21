using AfterSecret.Models.Constant;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AfterSecret.Models.ViewModel
{
    public class InvitationVM
    {
        //[JsonProperty("invitationType")]
        //public InvitationType InvitationType { get; set; }

        [JsonProperty("inviterId")]
        public int InviterId { get; set; }

        [JsonProperty("inviter")]
        public string Inviter { get; set; }

        [JsonProperty("invitationCode")]
        public string InvitationCode { get; set; }

        [JsonProperty("total")]
        public int Total { get; set; }

        [JsonProperty("invitees")]
        public List<InviteeVM> Invitees { get; set; }
    }
}