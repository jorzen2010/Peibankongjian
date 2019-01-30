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
    public class RenwuController : AdminBaseController
    {
        private UnitOfWork unitOfWork = new UnitOfWork();
        //
        // GET: /JkSucai/
        public ActionResult Index(int? page,int bid)
        {

            Pager pager = new Pager();
            pager.table = "Renwu";
            pager.strwhere = "RenwuBook="+bid;
            pager.PageSize = 2;
            pager.PageNo = page ?? 1;
            pager.FieldKey = "Id";
            pager.FiledOrder = "Id desc";

            pager = CommonDal.GetPager(pager);
            IList<Renwu> dataList = DataConvertHelper<Renwu>.ConvertToModel(pager.EntityDataTable);
            var PageList = new StaticPagedList<Renwu>(dataList, pager.PageNo, pager.PageSize, pager.Amount);
            ViewBag.bid = bid;
            return View(PageList);
        }

        public ActionResult Create(int bid)
        {
            ViewBag.bid = bid;
           return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Create(Renwu renwu)
        {
            if (ModelState.IsValid)
            {
                unitOfWork.renwusRepository.Insert(renwu);
                unitOfWork.Save();
                return RedirectToAction("Index", "Renwu", new { bid=renwu.RenwuBook});
            }
          //  return View("Create?bid=" + renwu.RenwuBook, renwu);
            return RedirectToAction("Create", "Renwu", new { bid = renwu.RenwuBook });
        }

        public ActionResult Edit(int id)
        {

            Renwu renwu = unitOfWork.renwusRepository.GetByID(id);

            if (renwu == null)
            {
                return HttpNotFound();
            }

           return View(renwu);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Edit(Renwu renwu)
        {
            if (ModelState.IsValid)
            {
                unitOfWork.renwusRepository.Update(renwu);
                unitOfWork.Save();
                return RedirectToAction("Index", "Renwu", new {bid=renwu.RenwuBook });
            }
            return View(renwu);
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

                unitOfWork.renwusRepository.Delete(id);
                unitOfWork.Save();

                msg.MessageStatus = "true";
                msg.MessageInfo = "删除成功";
            }

            return Json(msg, JsonRequestBehavior.AllowGet);
        }

	}
}