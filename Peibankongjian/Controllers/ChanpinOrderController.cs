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
    public class ChanpinOrderController : AdminBaseController
    {
        private UnitOfWork unitOfWork = new UnitOfWork();
        //
        // GET: /JkSucai/
        public ActionResult Index(int? page,int tid)
        {

            Pager pager = new Pager();
            pager.table = "ChanpinOrder";
            pager.strwhere = "BuyChanpin="+tid;
            pager.PageSize = 2;
            pager.PageNo = page ?? 1;
            pager.FieldKey = "Id";
            pager.FiledOrder = "Id desc";

            pager = CommonDal.GetPager(pager);
            IList<ChanpinOrder> dataList = DataConvertHelper<ChanpinOrder>.ConvertToModel(pager.EntityDataTable);
            var PageList = new StaticPagedList<ChanpinOrder>(dataList, pager.PageNo, pager.PageSize, pager.Amount);
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
        public ActionResult Create(ChanpinOrder chanpinOrder)
        {
            if (ModelState.IsValid)
            {
                unitOfWork.chanpinOrdersRepository.Insert(chanpinOrder);
                unitOfWork.Save();
                return RedirectToAction("Index", "ChanpinOrder");
            }

            return View(chanpinOrder);
        }

        public ActionResult Edit(int id)
        {

            ChanpinOrder chanpinOrder = unitOfWork.chanpinOrdersRepository.GetByID(id);

            if (chanpinOrder == null)
            {
                return HttpNotFound();
            }
            return View(chanpinOrder);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Edit(ChanpinOrder chanpinOrder)
        {
            if (ModelState.IsValid)
            {
                unitOfWork.chanpinOrdersRepository.Update(chanpinOrder);
                unitOfWork.Save();
                return RedirectToAction("Index", "ChanpinOrder");
            }
            return View(chanpinOrder);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult UpdateStatus(int? id, bool status)
        {
            Message msg = new Message();
            if (id == null)
            {
                msg.MessageStatus = "false";
                msg.MessageInfo = "找不到ID";
            }
            ChanpinOrder _order = unitOfWork.chanpinOrdersRepository.GetByID(id);
            _order.Status = !status;
            _order.PayTime = DateTime.Now;
            if (ModelState.IsValid)
            {

                unitOfWork.chanpinOrdersRepository.Update(_order);
                unitOfWork.Save();
                msg.MessageStatus = "true";
                msg.MessageInfo = "已经更改为" + _order.Status.ToString();
            }
            return Json(msg, JsonRequestBehavior.AllowGet);
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

                unitOfWork.chanpinOrdersRepository.Delete(id);
                unitOfWork.Save();

                msg.MessageStatus = "true";
                msg.MessageInfo = "删除成功";
            }

            return Json(msg, JsonRequestBehavior.AllowGet);
        }

	}
}