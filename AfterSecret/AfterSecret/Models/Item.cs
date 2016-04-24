using AfterSecret.Models.Constant;
using AfterSecret.Models.DAL;
using Newtonsoft.Json;
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
        [JsonProperty("name")]
        public string Name { get; set; }

        [MaxLength(200)]
        [JsonProperty("remark")]
        public string Remark { get; set; }

        //系数
        [DefaultValue(1)]
        [JsonProperty("seats")]
        public int Seats { get; set; }

        [JsonProperty("unitPrice")]
        public decimal UnitPrice { get; set; }

        [JsonProperty("total")]
        public int Total { get; set; }

        [JsonProperty("remain")]
        public int Remain
        {
            get
            {
                using (var uw = new UnitOfWork())
                {
                    return Total - uw.PurchaseRepository.Get()
                        .Where(a => a.Order.OrderStatus == OrderStatus.Unpaid
                            || a.Order.OrderStatus == OrderStatus.Paid
                            || a.Order.OrderStatus == OrderStatus.Processing).Where(a => a.ItemId == Id)
                            .Select(a => a.Quantity).ToList().Sum();
                }
            }
        }

        [JsonProperty("order")]
        public int Order { get; set; }

        [JsonProperty("invitationType")]
        public InvitationType InvitationType { get; set; }

        [Required]
        [MaxLength(200)]
        [JsonProperty("imgSrc")]
        public string ImgSrc { get; set; }
    }
}