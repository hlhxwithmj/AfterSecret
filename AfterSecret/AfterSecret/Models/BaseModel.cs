using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace AfterSecret.Models
{
    public class BaseModel
    {
        public int Id { get; set; }

        public bool IsValidate { get; set; }

        [Column(TypeName = "DateTime2")]
        public DateTime EditTime { get; set; }

        public BaseModel()
        {
            IsValidate = true;
            EditTime = DateTime.Now;
        }
    }
}