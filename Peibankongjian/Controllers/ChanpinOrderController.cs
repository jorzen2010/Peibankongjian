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
        public ActionResult Index(int? page, int cid)
        {

            Pager pager = new Pager();
            pager.table = "ChanpinOrder";
            pager.strwhere = "BuyChanpin=" + cid;
            pager.PageSize = 2;
            pager.PageNo = page ?? 1;
            pager.FieldKey = "Id";
            pager.FiledOrder = "Id desc";

            pager = CommonDal.GetPager(pager);
            IList<ChanpinOrder> dataList = DataConvertHelper<ChanpinOrder>.ConvertToModel(pager.EntityDataTable);
            var PageList = new StaticPagedList<ChanpinOrder>(dataList, pager.PageNo, pager.PageSize, pager.Amount);
            return View(PageList);
        }



        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public JsonResult UpdateStatus(int? id, bool status)
        //{
        //    Message msg = new Message();
        //    if (id == null)
        //    {
        //        msg.MessageStatus = "false";
        //        msg.MessageInfo = "找不到ID";
        //    }
        //    ChanpinOrder _order = unitOfWork.chanpinOrdersRepository.GetByID(id);
        //    _order.Status = !status;
        //    _order.PayTime = DateTime.Now;
        //    if (ModelState.IsValid)
        //    {

        //        unitOfWork.chanpinOrdersRepository.Update(_order);
        //        unitOfWork.Save();
        //        msg.MessageStatus = "true";
        //        msg.MessageInfo = "已经更改为" + _order.Status.ToString();
        //    }
        //    return Json(msg, JsonRequestBehavior.AllowGet);
        //}

        public ActionResult Edit(int id)
        {

            ChanpinOrder _order = unitOfWork.chanpinOrdersRepository.GetByID(id);

            if (_order == null)
            {
                return HttpNotFound();
            }

            if (_order.ProductType == 1)
            {
               
                ViewBag.productType = "会员产品";
                ViewBag.chanpin =
                    _order.BuyChanpin == 1 ? "普通会员" : "陪伴师会员";
               

            }
            if (_order.ProductType == 2)
            {
                ViewBag.productType = "课程产品";
            }
            ViewBag.yaoqingren =
                   PeibanService.GetRenById(_order.Yaoqingren) == null ? "邀请人编号有误" : PeibanService.GetRenById(_order.Yaoqingren).RenNickName;
            return View(_order);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Edit(ChanpinOrder _order)
        {
            if (ModelState.IsValid)
            {
                unitOfWork.chanpinOrdersRepository.Update(_order);
                unitOfWork.Save();
                return RedirectToAction("Index", "ChanpinOrder",new{cid=_order.BuyChanpin});
            }
            return View(_order);
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