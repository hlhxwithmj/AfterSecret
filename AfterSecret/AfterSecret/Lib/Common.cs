using AfterSecret.Models;
using AfterSecret.Models.DAL;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;

namespace AfterSecret.Lib
{
    public class Common
    {
        public static int ConvertToTimestamp(DateTime value)
        {
            TimeSpan span = (value - new DateTime(1970, 1, 1, 0, 0, 0, 0).ToLocalTime());
            return (int)span.TotalSeconds;
        }
        
        public static string get_AccessToken(string appID, string appsecret)
        {
            using (var uw = new UnitOfWork())
            {
                var token = uw.AccessTokenRepository.dbSet.Take(1).FirstOrDefault();
                if (token == null || token.IsValid == false)
                {
                    //获取access_token,每日限额2000
                    string url = "https://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential&appid=" + appID + "&secret=" + appsecret;
                    string access_token = string.Empty;
                    int expires_in = 0;
                    string errcode = string.Empty;
                    string errmsg = string.Empty;
                    using (var client = new WebClient())
                    {
                        string json = client.DownloadString(url);
                        JObject jObj = JObject.Parse(json);
                        access_token = (string)jObj["access_token"];
                        expires_in = (int)jObj["expires_in"];
                        errcode = (string)jObj["errcode"];
                        errmsg = (string)jObj["errmsg"];
                    }
                    //end
                    if (token == null)
                        uw.AccessTokenRepository.Insert(new AccessToken() { EditTime = DateTime.Now, ErrCode = errcode, ErrMsg = errmsg, ExpireTime = DateTime.Now.AddSeconds(expires_in - 60), Token = access_token });
                    else
                    {
                        token.Token = access_token;
                        token.ExpireTime = DateTime.Now.AddSeconds(expires_in - 60);
                        token.ErrMsg = errmsg;
                        token.ErrCode = errcode;
                        token.EditTime = DateTime.Now;
                    }
                    uw.AccessTokenRepository.context.SaveChanges();
                    return access_token;
                }
                else
                    return token.Token;
            }
        }
        public static string get_AccessToken()
        {
            var _appID = SubscribeConfig.APPID;
            var _appsecret = SubscribeConfig.APPSECRET;
            return get_AccessToken(_appID, _appsecret);
        }

        public static string DesEncrypt(string strText)
        {
            try
            {
                var token = SubscribeConfig.TOKEN;
                var strB = GetBytes(strText);
                var tokenB = GetBytes(token);
                for (int i = 0; i < strB.Length; i++)
                {
                    strB[i] = (byte)(strB[i] ^ tokenB[i % tokenB.Length]);
                }
                var enCyptStr = GetString(strB);
                var enCyptStr64 = Base64Encode(enCyptStr);
                var enCyptStr64Filter = base64_url_encode(enCyptStr64);
                return enCyptStr64Filter;
            }
            catch
            {
                return "";
            }
        }

        public static string DesDecrypt(string enCyptStr64Filter)
        {
            try
            {
                var token = SubscribeConfig.TOKEN;
                var tokenB = GetBytes(token);
                var deCyptStr64Filter = base64_url_decode(enCyptStr64Filter);
                var deCyptStr64 = Base64Decode(deCyptStr64Filter);
                var deCyptStr = GetBytes(deCyptStr64);
                for (int i = 0; i < deCyptStr.Length; i++)
                {
                    deCyptStr[i] = (byte)(deCyptStr[i] ^ tokenB[i % tokenB.Length]);
                }
                //decrypt
                return GetString(deCyptStr);
            }
            catch
            {
                return "";
            }
        }

        static string base64_url_encode(string input)
        {
            return input.Replace("+", "-").Replace("/", "_").Replace("=", ",");
        }

        static string base64_url_decode(string input)
        {
            return input.Replace("-", "+").Replace("_", "/").Replace(",", "=");
        }


        public static string Base64Encode(string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }
        public static string Base64Decode(string base64EncodedData)
        {
            var base64EncodedBytes = System.Convert.FromBase64String(base64EncodedData);
            return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
        }

        static byte[] GetBytes(string str)
        {
            byte[] bytes = new byte[str.Length * sizeof(char)];
            System.Buffer.BlockCopy(str.ToCharArray(), 0, bytes, 0, bytes.Length);
            return bytes;
        }

        static string GetString(byte[] bytes)
        {
            char[] chars = new char[bytes.Length / sizeof(char)];
            System.Buffer.BlockCopy(bytes, 0, chars, 0, bytes.Length);
            return new string(chars);
        }

        public static string GenerateCredential(string openId)
        {
            TimeSpan s = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0);
            long now = (long)s.TotalMilliseconds;

            return Common.DesEncrypt(now.ToString() + openId);
        }
    }
}