using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AfterSecret.Models.ViewModel
{
    public class RegisterMemberVM
    {

        [Required]
        [MaxLength(100)]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(100)]
        public string LastName { get; set; }

        [Required]
        [MaxLength(20)]
        public string Gender { get; set; }

        [Required]
        [MaxLength(100)]
        public string Nationality { get; set; }

        [Required]
        [MaxLength(25)]
        public string Mobile { get; set; }

        [Required]
        [MaxLength(100)]
        public string Email { get; set; }

        [MaxLength(100)]
        public string WeChatID { get; set; }

        [MaxLength(100)]
        public string Occupation { get; set; }

        [Required]
        public string AgentCode { get; set; }
    }
}