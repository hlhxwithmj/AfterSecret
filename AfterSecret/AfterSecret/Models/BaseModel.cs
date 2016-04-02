using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AfterSecret.Models
{
    public class BaseModel
    {
        public int Id { get; set; }

        public bool IsValidate { get; set; }
    }
}