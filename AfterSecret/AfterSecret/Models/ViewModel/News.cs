using AfterSecret.Lib;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Web;

namespace AfterSecret.Models.ViewModel
{
    public class News
    {
        public JObject JObj { get; set; }

        public string ToUserName
        {
            get
            {
                return (string)JObj["xml"]["FromUserName"]["#cdata-section"];
            }
        }

        public string FromUserName
        {
            get
            {
                return (string)JObj["xml"]["ToUserName"]["#cdata-section"];
            }
        }

        public long CreateTime
        {
            get
            {
                return Common.ConvertToTimestamp(DateTime.Now);
            }
        }

        public string MsgType
        {
            get
            {
                return "news";
            }
        }

        [Required]
        public List<ArticalItem> ArticleItems { get; set; }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<xml>");
            sb.Append("<ToUserName><![CDATA[" + ToUserName + "]]></ToUserName>");
            sb.Append("<FromUserName><![CDATA[" + FromUserName + "]]></FromUserName>");
            sb.Append("<CreateTime>" + CreateTime + "</CreateTime>");
            sb.Append("<MsgType><![CDATA[news]]></MsgType>");
            sb.Append("<ArticleCount>" + ArticleItems.Count + "</ArticleCount>");
            sb.Append("<Articles>");
            foreach (var m in ArticleItems)
            {
                sb.Append("<item>");
                sb.Append("<Title><![CDATA[" + m.Title + "]]></Title>");
                sb.Append("<Description><![CDATA[" + m.Description + "]]></Description>");
                sb.Append("<PicUrl><![CDATA[" + m.PicUrl + "]]></PicUrl>");
                sb.Append("<Url><![CDATA[" + m.Url + "]]></Url>");
                sb.Append("</item>");
            }
            sb.Append("</Articles>");
            sb.Append("</xml>");
            return sb.ToString();
        }
    }
}