﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SkyDal;

namespace Peibankongjian.Controllers
{
    public class WapBaseController : Controller
    {
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);
            if (Session["uname"] == null)
            {
                //filterContext.HttpContext.Response.Redirect("/User/Login");
                filterContext.Result = Redirect("/Wap/Login");
            }
            ViewBag.Title = "陪伴空间";
        }
    }
}