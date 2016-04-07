using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AfterSecret.Models
{
    public class Item : BaseModel
    {
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        [Required]
        [MaxLength(200)]
        public string Remark { get; set; }

        //系数
        [DefaultValue(1.00)]
        public decimal Factor { get; set; }

        public decimal UnitPrice { get; set; }

        public int Total { get; set; }
        public int Remain { get; set; }

        public int Order { get; set; }
    }
}