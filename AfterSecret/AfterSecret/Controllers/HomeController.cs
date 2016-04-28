using AfterSecret.Lib;
using AfterSecret.Models.DAL;
using AfterSecret.Models.ViewModel;
using log4net;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Pingpp;
using Pingpp.Models;
using Pingpp.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Xml;
using Tencent;

namespace AfterSecret.Controllers
{
    public class HomeController : Controller
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(HomeController).FullName);

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

        public ActionResult WebHooks()
        {
            if (Request.HttpMethod == "POST")
            {
                //获取 post 的 event 对象
                string inputData = ReadStream(Request.InputStream);

                //获取 header 中的签名
                string sig = Request.Headers.Get("x-pingplusplus-signature");

                //公钥路径（请检查你的公钥 .pem 文件存放路径）
                string pem = @"-----BEGIN PUBLIC KEY-----
MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEAsX1iZl5uQXbIMxugTaED
9fqZNpiJ4ggAyMApQmlr0ZDh7/VfEvsosx5BJtnGRukTTQwybTv9FhMzgYrH6x4r
HQICh46s3VVsZa9VDugFQCP49z4pt+zXvA7QtEo44ElT3e/WLHj0yU9LIcJ0cP+r
5mrpTRz+sQUMNiEuh8S9Em8Yw8GzwOY9fVcDcWLAMU7/6nrJI6KwrrhroTlMU5YO
QVom8p1XLNdcWh0sLbQTNlBh5RzsvFVIQrFMxuReY7ma/mJIKN3xkSAIAV5sBNbG
lAynO+E3hCXvcdt0PqzS1DH9hq1fmP4hBxs9x6+ufeflg+qs/cXo49zeyr1Cv28u
5wIDAQAB
-----END PUBLIC KEY-----
";

                //验证签名
                string result = VerifySignedHash(inputData, sig, pem);

                var jObject = JObject.Parse(inputData);
                var type = jObject.SelectToken("type");
                if (type.ToString() == "charge.succeeded")
                {
                    // TODO what you need do    
                    var chargeId = jObject["data"]["object"]["id"].Value<string>();
                    Common.OrderSucceeded(chargeId);
                    Response.StatusCode = 200;
                }
                else
                {
                    // TODO what you need do
                    Response.StatusCode = 500;
                }
            }
            return Content("");
        }

        private static string ReadStream(Stream stream)
        {
            using (var reader = new StreamReader(stream, Encoding.UTF8))
            {
                return reader.ReadToEnd();
            }
        }


        public static string ReadFileToString(string path)
        {
            using (StreamReader sr = new StreamReader(path))
            {
                return sr.ReadToEnd();
            }
        }

        public static string VerifySignedHash(string str_DataToVerify, string str_SignedData, string pem)
        {
            byte[] SignedData = Convert.FromBase64String(str_SignedData);

            ASCIIEncoding ByteConverter = new ASCIIEncoding();
            byte[] DataToVerify = ByteConverter.GetBytes(str_DataToVerify);
            try
            {
                string sPublicKeyPEM = pem;
                RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();

                rsa.PersistKeyInCsp = false;
                rsa.LoadPublicKeyPEM(pem);

                if (rsa.VerifyData(DataToVerify, "SHA256", SignedData))
                {
                    return "verify success";
                }
                else
                {
                    return "verify fail";
                }

            }
            catch (CryptographicException e)
            {
                log.Warn(e);
                return "verify error";
            }
        }


        public ActionResult Oauth2(string path = null, string expire = null, string code = null, string state = null)
        {
            TimeSpan now = (DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0));
            long fromtime = 0;
            long.TryParse(Common.DesDecrypt(expire), out fromtime);
            var timediff = ((long)now.TotalMilliseconds - fromtime);
            if (TimeSpan.FromMilliseconds((double)timediff).TotalMinutes > 30)
            {
                return RedirectToAction("Expire");
            }
            if (!string.IsNullOrEmpty(code) && !string.IsNullOrEmpty(state))
            {
                Session["openIdForPay"] = Common.DesEncrypt(WxPubUtils.GetOpenId(PayConfig.APPID, PayConfig.APPSECRET, code));//服务号openId
                Session["openId"] = Common.DesEncrypt(state);//订阅号openId
                Session["token"] = Common.GenerateCredential(state);//token
                Session["path"] = path;

                return RedirectToAction("Default");
            }
            else
            {
                return Content("没有参数Code, 非法操作！");
            }
        }

        public ActionResult Expire()
        {
            return View();
        }

        public ActionResult Default()
        {
            ViewBag.openIdForPay = Session["openIdForPay"];
            ViewBag.openId = Session["openId"];//订阅号openId
            ViewBag.token = Session["token"];//订阅号openId
            ViewBag.path = Session["path"];
            Session.Abandon();

            return View();
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
                    result = HandleClickEvent(jObj);
                    break;
                case "subscribe":
                    result = HandleClickEvent(jObj, true);
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

        private string HandleClickEvent(JObject jObj, bool isSubscribe = false)
        {
            var result = string.Empty;
            var description = string.Empty;
            var picUrl = SubscribeConfig.DOMAIN + "/Static/image/";
            var title = string.Empty;
            var url = string.Empty;
            var openId = (string)jObj["xml"]["FromUserName"]["#cdata-section"];
            var path = string.Empty;
            switch ((string)jObj["xml"]["EventKey"]["#cdata-section"])
            {
                case SubscribeConfig.mREGISTER:
                    path = "register";
                    description = "Enter your agent code, register and join the Secret After Party!";
                    picUrl = picUrl + "registration1.jpg";
                    title = "Registration";
                    break;
                case SubscribeConfig.mPURCHASE:
                    path = "items";
                    description = "Would like to purchase more tickets and invite your guests? Click here!";
                    picUrl = picUrl + "shop.jpg";
                    title = "Shop";
                    break;
                case SubscribeConfig.myPURCHASE:
                    path = "invite";
                    description = "Manage your invites and send tickets to your guests!";
                    picUrl = picUrl + "my-invitees.jpg";
                    title = "My Invites";
                    break;
                case SubscribeConfig.myTICKET:
                    path = "ticket";
                    description = "Review your ticket, save it on your phone and see you at the event!";
                    picUrl = picUrl + "my-pass.jpg";
                    title = "My Ticket";
                    break;
                default:
                    break;
            }
            if (isSubscribe == true)
            {
                path = "register";
                description = "Enter your agent code, register and join the Secret After Party!";
                picUrl = picUrl + "registration1.jpg";
                title = "Registration";
            }
            var expire = Common.GenerateCredential("");
            url = @"https://open.weixin.qq.com/connect/oauth2/authorize?appid=" + PayConfig.APPID
                + "&redirect_uri=" + HttpUtility.UrlEncode(SubscribeConfig.DOMAIN + "/Home/Oauth2?path=" + path + "&expire=" + expire)
                + "&response_type=code&scope=snsapi_base&state=" + openId + "#wechat_redirect";
            log.Warn(url);
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
