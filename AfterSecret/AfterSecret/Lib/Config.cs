﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AfterSecret.Lib
{
    public class SubscribeConfig
    {
        public const string APPID = "wx7328e16406a940eb";
        public const string APPSECRET = "0f8beaabd159526a0bc344f003d45614";
        public const string TOKEN = "SecretDinner";
        public const string ENCODINGAESKEY = "YlzZWOu7qS0giJmT29ogJopufCZZp9mxcXxiHYTOOlv";
        public const string DOMAIN = "http://sap.cwchros.com";
        public const string mREGISTER = "REGISTER";
        public const string mPURCHASE = "PURCHASE";
        public const string myPURCHASE = "MYPURCHASE";
        public const string myTICKET = "MYTICKET";
        public const string INVITATION = "INVITATION";
        public const string _seedUser_Prefix = "8";
        public const string _Ticket_Invitee_Prefix = "6";
        public const string _Table_Invitee_Prefix = "7";
        public const string _shareUser_Prefix = "9";
    }

    public class PayConfig
    {
        public const string APPID = "wxda37cd9d7e836425";
        public const string MCHID = "1309299701";
        public const string KEY = "hjSxH5qgUAHZGfDOtmERKTKsbF5WV5ox";
        public const string APPSECRET = "32190d3a0a5bf98a388a175dc8b5ef4e";
    }

    public class PingConfig
    {
        public const string APPID = "app_4G0GS0eH4uPSDqv9";
        public const string TESTSECRETKEY = "sk_test_eDWfj5T88yT4jvz5m9yLSyDG";
        public const string LIVESECRETKEY = "sk_live_TyTOyLWHyPK8jHSWb9nDOSWH";
        public const string SUBJECT = "The Secret After Party";
    }

    public class SystemConfig
    {
        public const string QRPATH = "Content/QR/";
    }

    public class CheckIn
    {
        public static readonly string[] Members = {
            "otb_8vti-ldLRin7ljtViTlAFEX4"
        };
    }
}

 