using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using PagedList.Mvc;
using SkyDal;
using SkyEntity;
using SkyService;
using SkyCommon;

namespace Peibankongjian.Controllers
{
    public class WechatReplyController : AdminBaseController
    {
        private UnitOfWork unitOfWork = new UnitOfWork();
        //
        // GET: /JkSucai/
        public ActionResult Index(int? page)
        {

            Pager pager = new Pager();
            pager.table = "WechatReply";
            pager.strwhere = "1=1";
            pager.PageSize = 2;
            pager.PageNo = page ?? 1;
            pager.FieldKey = "Id";
            pager.FiledOrder = "Id desc";

            pager = CommonDal.GetPager(pager);
            IList<WechatReply> dataList = DataConvertHelper<WechatReply>.ConvertToModel(pager.EntityDataTable);
            var PageList = new StaticPagedList<WechatReply>(dataList, pager.PageNo, pager.PageSize, pager.Amount);
            return View(PageList);
        }

        public ActionResult Create()
        {
            CategoryService cate = new CategoryService();
            ViewData["Categorylist"] = cate.GetCategorySelectList(2);
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Create(WechatReply wechatReply)
        {
            if (ModelState.IsValid)
            {
                unitOfWork.wechatReplysRepository.Insert(wechatReply);
                unitOfWork.Save();
                return RedirectToAction("Index", "WechatReply");
            }
            CategoryService cate = new CategoryService();
            ViewData["Categorylist"] = cate.GetCategorySelectList(2);
            return View(wechatReply);
        }

        public ActionResult Edit(int id)
        {

            WechatReply wechatReply = unitOfWork.wechatReplysRepository.GetByID(id);
            CategoryService cate = new CategoryService();
            ViewData["Categorylist"] = cate.GetCategorySelectList(2);
            if (wechatReply == null)
            {
                return HttpNotFound();
            }
            return View(wechatReply);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Edit(WechatReply wechatReply)
        {
            if (ModelState.IsValid)
            {
                unitOfWork.wechatReplysRepository.Update(wechatReply);
                unitOfWork.Save();
                return RedirectToAction("Index", "WechatReply");
            } 
            CategoryService cate = new CategoryService();
            ViewData["Categorylist"] = cate.GetCategorySelectList(2);
            return View(wechatReply);
        }

        //彻底删除
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult DeleteConfirmed(int? id)
        {
            Message msg = new Message();
            if (id == null)
            {
                msg.MessageStatus = "false";
                msg.MessageInfo = "找不到ID";
            }
            else
            {

                unitOfWork.wechatReplysRepository.Delete(id);
                unitOfWork.Save();

                msg.MessageStatus = "true";
                msg.MessageInfo = "删除成功";
            }

            return Json(msg, JsonRequestBehavior.AllowGet);
        }

	}
}