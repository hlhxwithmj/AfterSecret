using AfterSecret.Models.Constant;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AfterSecret.Models
{
    public class Ticket : BaseModel
    {
        public int InvitationId { get; set; }

        public virtual Invitation Invitation { get; set; }

        public int InviteeId { get; set; }

        public virtual RegisterMember Invitee { get; set; }

        public InvitationType InvitationType { get; set; }

        [MaxLength(4)]
        public string TableNo { get; set; }

        [Required]
        [MaxLength(128)]
        public string QRCodePath { get; set; }
    }
}