using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SkyCommon;
using SkyWechatService;

namespace Peibankongjian.Controllers
{
    public class UcenterController : Controller
    {
        //
        // GET: /Ucenter/
        public ActionResult Register()
        {
            string userAgent = Request.UserAgent;

            string CODE = Request["code"];

            string STATE = Request["state"];
            WebchatJsUserinfo userinfo =new WebchatJsUserinfo();

            if (string.IsNullOrEmpty(CODE))
            {
                return RedirectToAction("WechatLogin");
            }
            else
            {
                userinfo = WechatJsServices.GetUserInfo(userAgent, CODE);
            }
            ViewBag.code = CODE;
            ViewBag.userAgent = userAgent;
            return View(userinfo);
        }

        #region 微信登录
        public ActionResult WechatLogin()
        {
           // string sourceUrl = Request.UrlReferrer.ToString();

            string userAgent = Request.UserAgent;

            WechatConfig wechatconfig = AccessTokenService.GetWechatConfig();

            string REDIRECT_URI = System.Web.HttpUtility.UrlEncode("http://peiban.zzd123.com/Ucenter/Register");

            string SCOPE = "snsapi_userinfo";
          //string STATE = sourceUrl;
            string STATE = "statecanshu";

            string url = "https://open.weixin.qq.com/connect/oauth2/authorize?appid=" + wechatconfig.Appid + "&redirect_uri=" + REDIRECT_URI + "&response_type=code&scope=" + SCOPE + "&state=" + STATE + "#wechat_redirect";

            return Redirect(url);

        }
        #endregion

        public void SendYanzhengma(string touser)
        {
            Configuration config = System.Web.Configuration.WebConfigurationManager.OpenWebConfiguration("~");
            AppSettingsSection appsection = config.GetSection("appSettings") as AppSettingsSection;

            string WechatId = appsection.Settings["WechatId"].Value.ToString();
            string code ="注册验证码为："+ CommonTools.getRandomNumber(100000, 999999);
            WechatMessageServices.ResponseTextMessage(touser,WechatId,code);
        }
    }
}