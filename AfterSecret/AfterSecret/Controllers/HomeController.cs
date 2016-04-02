using AfterSecret.Lib;
using AfterSecret.Models.ViewModel;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Mvc;
using System.Xml;
using Tencent;

namespace AfterSecret.Controllers
{
    public class HomeController : Controller
    {
        public void Index()
        {
            //判断是否来自于微信                       
            if (!String.IsNullOrEmpty(Request.QueryString["echostr"]))
            {
                var echostr = Request.QueryString["echostr"];
                Response.Write(echostr);
                return;
            }
            else
            {
                #region 处理数据

                #region 通过配置文件获取Token, EncodingAESKey, AppID
                string sToken = SubscribeConfig.TOKEN;
                string sEncodingAESKey = SubscribeConfig.ENCODINGAESKEY;
                string sAppID = SubscribeConfig.APPID;
                #endregion

                #region 通过url获取msg_signature, timestamp, nonce
                string sReqMsgSig = Request.QueryString["msg_signature"];
                string sReqTimeStamp = Request.QueryString["timestamp"];
                string sReqNonce = Request.QueryString["nonce"];
                #endregion
                // 微信服务器返回的密文，对应POST请求的数据
                string sReqData = System.Text.Encoding.Default.GetString(Request.BinaryRead(Request.TotalBytes));

                WXBizMsgCrypt wxcpt = new WXBizMsgCrypt(sToken, sEncodingAESKey, sAppID);
                string sMsg = "";  //解析之后的明文
                int ret = 0;
                //验证消息真实性，并获取解密后的明文
                ret = wxcpt.DecryptMsg(sReqMsgSig, sReqTimeStamp, sReqNonce, sReqData, ref sMsg);
                if (ret == (int)Tencent.WXBizMsgCrypt.WXBizMsgCryptErrorCode.WXBizMsgCrypt_OK)
                {
                    var jObj = TransferToJObject(sMsg);
                    //企业号待回复用户的消息，xml格式的字符串
                    string str = ResponseMsg(jObj);
                    //加密后的可以直接回复用户的密文
                    string msg = string.Empty;
                    //将企业号回复用户的消息加密
                    wxcpt.EncryptMsg(str, sReqTimeStamp, sReqNonce, ref msg);
                    Response.Write(msg);
                }
                else
                {
                    Response.Write("请通过微信访问");
                    return;
                }

                #endregion
            }
        }

        /// <summary>
        /// 处理数据
        /// </summary>
        /// <param name="str">明文数据字符串</param>
        /// <returns>需要给用户返回的数据</returns>
        private JObject TransferToJObject(string str)
        {
            var xml = new XmlDocument();
            xml.LoadXml(str);
            JObject jObj = JObject.Parse(JsonConvert.SerializeObject(xml));
            return jObj;
        }

        private string ResponseMsg(JObject jObj)
        {
            string resultStr = "";

            //处理用户发送的不同数据类型
            switch ((string)jObj["xml"]["MsgType"]["#cdata-section"])
            {
                //用户触发事件
                case "event":
                    resultStr = ReceiveEvent(jObj);
                    break;
                //其他
                default:
                    resultStr = "";
                    break;
            }

            return resultStr;
        }

        /// <summary>
        /// 处理用户发送的事件型数据
        /// </summary>
        /// <param name="jObj"></param>
        /// <param name="employee">发送者</param>
        /// <returns></returns>
        private string ReceiveEvent(JObject jObj)
        {
            var result = "";
            switch ((string)jObj["xml"]["Event"]["#cdata-section"])
            {
                case "CLICK":

                    break;
                case "subscribe":
                    break;
                case "unsubscribe":
                    break;
                case "SCAN":
                    break;
                case "LOCATION":
                    break;
                case "VIEW":
                    break;
                default:
                    break;
            }
            return result;
        }

        private string HandleClickEvent(JObject jObj)
        {
            var result = string.Empty;
            var description = string.Empty;
            var picUrl = string.Empty;
            var title = string.Empty;
            var url = string.Empty;
            switch ((string)jObj["xml"]["EventKey"]["#cdata-section"])
            {
                case SubscribeConfig.mREGISTER:

                    break;
                case SubscribeConfig.mPURCHASE:

                    break;
                default:
                    break;
            }
            result = new News()
            {
                JObj = jObj,
                ArticleItems = new List<ArticalItem>() { 
                            new ArticalItem() {  Description = description, PicUrl = picUrl, Title = title, Url = url } }
            }.ToString();
            return result;
        }
    }
}
