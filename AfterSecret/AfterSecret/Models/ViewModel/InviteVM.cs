using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AfterSecret.Models.ViewModel
{
    public class InviteVM
    {
        [JsonProperty("hasTicket")]
        public bool HasTicket { get; set; }

        [JsonProperty("hasTable")]
        public bool HasTable { get; set; }

        [JsonProperty("fullTicket")]
        public bool FullTicket { get; set; }

        [JsonProperty("fullTable")]
        public bool FullTable { get; set; }
    }
}