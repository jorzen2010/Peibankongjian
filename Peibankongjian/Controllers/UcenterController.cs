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
        public ActionResult WechatLogin(string redirectUrl)
        {
            // string sourceUrl = Request.UrlReferrer.ToString();

            string userAgent = Request.UserAgent;

            WechatConfig wechatconfig = AccessTokenService.GetWechatConfig();

           // string REDIRECT_URI = System.Web.HttpUtility.UrlEncode("http://peiban.zzd123.com/Ucenter/Register");
            string REDIRECT_URI = System.Web.HttpUtility.UrlEncode(redirectUrl);

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
                return RedirectToAction("WechatLogin", new { redirectUrl = "http://peiban.zzd123.com/Ucenter/Register" });
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
            //购买产品类型：课程和会员两类
            //购买产品：11代表注册会员升级，12代表升级为陪伴师，2+产品ID代表课程产品

            string userAgent = Request.UserAgent;

            string CODE = Request["code"];

            string STATE = Request["state"];
            if (string.IsNullOrEmpty(CODE))
            {
                return RedirectToAction("WechatLogin", new { redirectUrl = "http://peiban.zzd123.com/Ucenter/ShengjiVIP" });
            }
            else
            {
                WebchatJsUserinfo userinfo = WechatJsServices.GetUserInfo(userAgent, CODE);
                var wxusers = unitOfWork.rensRepository.Get(filter: u => u.RenOpenid == userinfo.openid);
                if (wxusers.Count() > 0)
                {
                    Ren ren = wxusers.First();
                    ViewData["renuser"] = ren;
                    return View();
                }
                else
                {
                    //你尚未注册
                    return RedirectToAction("Register", "Ucenter");
                }
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult ShengjiVIP(ChanpinOrder corder)
        {
            Random random = new Random();
            if (ModelState.IsValid)
            {
                corder.OrderNumber = System.DateTime.Now.ToUniversalTime().Ticks.ToString() + random.Next(1000, 9999).ToString().ToString();
                unitOfWork.chanpinOrdersRepository.Insert(corder);
                unitOfWork.Save();
                return RedirectToAction("Index", "Home");
            }

            return RedirectToAction("ShengjiVIP", "Ucenter");
        }

        public ActionResult ShengjiPeibanshi()
        {
            string userAgent = Request.UserAgent;

            string CODE = Request["code"];

            string STATE = Request["state"];
            if (string.IsNullOrEmpty(CODE))
            {
                return RedirectToAction("WechatLogin", new { redirectUrl = "http://peiban.zzd123.com/Ucenter/ShengjiPeibanshi" });
            }
            else
            {
                WebchatJsUserinfo userinfo = WechatJsServices.GetUserInfo(userAgent, CODE);

                var wxusers = unitOfWork.rensRepository.Get(filter: u => u.RenOpenid == userinfo.openid);

                if (wxusers.Count() > 0)
                {
                    Ren ren = wxusers.First();
                    ViewData["renuser"] = ren;
                    return View();

                }

                else
                {
                    //你尚未注册
                    return RedirectToAction("Register", "Ucenter");
                }
            }



        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult ShengjiPeibanshi(ChanpinOrder corder)
        {
            Random random = new Random();
            if (ModelState.IsValid)
            {
                corder.OrderNumber = System.DateTime.Now.ToUniversalTime().Ticks.ToString() + random.Next(1000, 9999).ToString().ToString();
                unitOfWork.chanpinOrdersRepository.Insert(corder);
                unitOfWork.Save();
                return RedirectToAction("Index", "Home");
            }

            return RedirectToAction("ShengjiVIP", "Ucenter");
        }



        

      
    }
}