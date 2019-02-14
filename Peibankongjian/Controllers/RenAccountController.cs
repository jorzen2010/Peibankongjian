using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Security;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;
using System.Data;
using System.Collections;
using SkyDal;
using SkyEntity;
using SkyCommon;
using SkyService;

namespace Peibankongjian.Controllers
{
    public class RenAccountController : Controller
    {
        private UnitOfWork unitOfwork = new UnitOfWork();

        public ActionResult Login()
        {
            ViewBag.msg = "";

            return View();
        }


        [HttpPost]
        public ActionResult Login(FormCollection fc)
        {
            System.Web.HttpContext.Current.Session["renname"] = "";
            string username = fc["UserName"];
            string password = CommonTools.ToMd5(fc["Password"].ToString());

            var rens = unitOfwork.rensRepository.Get(filter: u => u.RenPhone == username && u.RenPassword == password);


            if (rens.Count() > 0)
            {
                if (rens.First().Status)
                {
                    HttpCookie cookie = new HttpCookie("renname");
                    cookie.Value = username;
                    System.Web.HttpContext.Current.Response.Cookies.Add(cookie);

                    HttpCookie cookieid = new HttpCookie("renid");
                    cookieid.Value = rens.First().Id.ToString();
                    System.Web.HttpContext.Current.Response.Cookies.Add(cookieid);

                    System.Web.HttpContext.Current.Session["renid"] = rens.First().Id.ToString();
                    System.Web.HttpContext.Current.Session["renname"] = username;
                    System.Web.HttpContext.Current.Session["Vip"] = "false";
                    if (PeibanService.GetVipByRen(rens.First().Id))
                    {
                        System.Web.HttpContext.Current.Session["Vip"] = "true";
                    }

                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ViewBag.msg = "此已经被禁用，不允许登陆";
                    return View();
                }
            }
            else
            {
                ViewBag.msg = "用户名或密码错误";
                return View();

            }
        }


        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            Response.Cookies["renname"].Expires = DateTime.Now.AddDays(-1);
            Response.Cookies["renid"].Expires = DateTime.Now.AddDays(-1);
            System.Web.HttpContext.Current.Session["renid"] = null;
            System.Web.HttpContext.Current.Session["renname"] = null;
            return View("Login");
        }
	}
}