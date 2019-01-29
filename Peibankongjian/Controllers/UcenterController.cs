using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SkyCommon;
using SkyWechatService;
using SkyDal;
using SkyEntity;

namespace Peibankongjian.Controllers
{
    public class UcenterController : Controller
    {
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

        private UnitOfWork unitOfWork = new UnitOfWork();
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
                var wxusers = unitOfWork.rensRepository.Get(filter: u => u.RenOpenid == userinfo.openid);
                if (wxusers.Count() > 0)
                {
                    return RedirectToAction("ShengjiVIP");
                }
            }
            ViewData["userInfo"] = userinfo;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Register(Ren ren)
        {
            if (ModelState.IsValid)
            {
                unitOfWork.rensRepository.Insert(ren);
                unitOfWork.Save();
                return RedirectToAction("Index", "Home");
            }

            return RedirectToAction("Register", "Ucenter");
        }


        public ActionResult ShengjiVIP()
        {
            return View();
        }

        

      
    }
}