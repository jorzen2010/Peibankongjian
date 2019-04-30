using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using PagedList;
using SkyCommon;
using SkyDal;
using SkyEntity;
using SkyService;
using SkyWechatService;
using Newtonsoft.Json;

namespace Peibankongjian.Controllers
{
    public class WapController : Controller
    {
        private UnitOfWork unitOfWork = new UnitOfWork();
        // GET: /Wap/
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Test()
        {
            return View();
        }
        public ActionResult ArticleContent(int id)
        {

            RenwuDaka daka = unitOfWork.renwuDakasRepository.GetByID(id);
            return View(daka);
        }
	}
}