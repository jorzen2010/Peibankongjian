﻿using System;
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

namespace Peibankongjian.Controllers
{
    public class UHomeController : UBaseController
    {
        private UnitOfWork unitOfWork = new UnitOfWork();
        public ActionResult Index(int? page)
        {
            Pager pager = new Pager();
            pager.table = "Product";
            pager.strwhere = "1=1";
            pager.PageSize = 12;
            pager.PageNo = page ?? 1;
            pager.FieldKey = "Id";
            pager.FiledOrder = "Id desc";

            pager = CommonDal.GetPager(pager);
            IList<Product> dataList = DataConvertHelper<Product>.ConvertToModel(pager.EntityDataTable);
            var PageList = new StaticPagedList<Product>(dataList, pager.PageNo, pager.PageSize, pager.Amount);
            return View(PageList);
        }

        public ActionResult DakaList(int? page)
        {
            Pager pager = new Pager();
            pager.table = "RenwuDaka";
            pager.strwhere = "RenwuZhixingzhe="+int.Parse(Session["renid"].ToString());
            pager.PageSize = 12;
            pager.PageNo = page ?? 1;
            pager.FieldKey = "Id";
            pager.FiledOrder = "Id desc";

            pager = CommonDal.GetPager(pager);
            IList<RenwuDaka> dataList = DataConvertHelper<RenwuDaka>.ConvertToModel(pager.EntityDataTable);
            var PageList = new StaticPagedList<RenwuDaka>(dataList, pager.PageNo, pager.PageSize, pager.Amount);
            return View(PageList);
        }

        public ActionResult CreateProductList(int? page)
        {
            Pager pager = new Pager();
            pager.table = "Book";
            pager.strwhere = "1=1";
            pager.PageSize = 12;
            pager.PageNo = page ?? 1;
            pager.FieldKey = "Id";
            pager.FiledOrder = "Id desc";

            pager = CommonDal.GetPager(pager);
            IList<Book> dataList = DataConvertHelper<Book>.ConvertToModel(pager.EntityDataTable);
            var PageList = new StaticPagedList<Book>(dataList, pager.PageNo, pager.PageSize, pager.Amount);
            return View(PageList);
        }

        public ActionResult CreateProduct(int pid)
        {
            ViewBag.pid = pid;
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult CreateProduct(Product product)
        {
            if (ModelState.IsValid)
            {
                unitOfWork.productsRepository.Insert(product);
                unitOfWork.Save();
                return RedirectToAction("Index", "UHome");
            }
            ViewBag.pid = product.ProductBook;
            return View(product);
        }
        public ActionResult UserInfo()
        {
           return View("~/Views/UHome/_PartialUserInfo.cshtml");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult EntrySpace(int rid,int kid,int bid,int pid,bool status)
        {
            Message msg = new Message();

            RenKongList rklist=new RenKongList();
            rklist.Kongjian = kid;
            rklist.Shenqingren = rid;
            rklist.Peibanshi = pid;
            rklist.ProductBook = bid;
            rklist.Status = status;


            unitOfWork.renKongListsRepository.Insert(rklist);
            unitOfWork.Save();

            msg.MessageStatus = "true";
            msg.MessageInfo = "申请进入成功";

            return Json(msg, JsonRequestBehavior.AllowGet);
        }

        public ActionResult RenwuList(int id)
        {
            Product product = unitOfWork.productsRepository.GetByID(id);
            Book book = unitOfWork.booksRepository.GetByID(product.ProductBook);
            var renwus = unitOfWork.renwusRepository.Get(filter:r=>r.RenwuBook==book.Id);
            ViewData["probook"] = book;
            ViewData["proRenwus"] = renwus;

            return View(product);
        }

        public ActionResult ShenqingPeiban(int cid)
        {
            int ptid = 1;
            int rid=int.Parse(Session["renid"].ToString());
            Message msg = PeibanService.GetStatusByOrder(ptid,cid, rid);
            ViewBag.status = msg.MessageStatus;
            ViewBag.cid = cid;
           
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]

        public ActionResult ShenqingPeiban(ChanpinOrder chanpinOrder)
        {
            if (ModelState.IsValid)
            {
                chanpinOrder.Status = false;
                chanpinOrder.PayStatus = false;
                chanpinOrder.OrderNumber = CommonTools.ToUnixTime(DateTime.Now) +
                                           CommonTools.getRandomNumber(10000, 99999);
                unitOfWork.chanpinOrdersRepository.Insert(chanpinOrder);
                unitOfWork.Save();
                return RedirectToAction("Index", "UHome");
            }
            return View(chanpinOrder);
        }

        public ActionResult MyOrder(int? page)
        {
            Pager pager = new Pager();
            pager.table = "ChanpinOrder";
            pager.strwhere = "1=1 and VipUser="+int.Parse(Session["renid"].ToString());
            pager.PageSize = 12;
            pager.PageNo = page ?? 1;
            pager.FieldKey = "Id";
            pager.FiledOrder = "Id desc";

            pager = CommonDal.GetPager(pager);
            IList<ChanpinOrder> dataList = DataConvertHelper<ChanpinOrder>.ConvertToModel(pager.EntityDataTable);
            var PageList = new StaticPagedList<ChanpinOrder>(dataList, pager.PageNo, pager.PageSize, pager.Amount);
            return View(PageList);
        }


         public ActionResult Daka(int renwu,int book,int peibanshi ,int kongjian,int dakaren)
         {
             ViewBag.peibanshi = peibanshi;
             ViewBag.renwu = renwu;
             ViewBag.book = book;
             ViewBag.dakaren = dakaren;
             ViewBag.kongjian = kongjian;

             return View();
         }

         [HttpPost]
         [ValidateAntiForgeryToken]
         [ValidateInput(false)]

         public ActionResult Daka(RenwuDaka daka)
         {
             if (ModelState.IsValid)
             {

                 unitOfWork.renwuDakasRepository.Insert(daka);
                 unitOfWork.Save();
                 return RedirectToAction("DakaList", "UHome");
             }
             return View(daka);
         }

         public ActionResult DakaContent(int id)
         {
             RenwuDaka daka = unitOfWork.renwuDakasRepository.GetByID(id);

             int dianzanren = int.Parse(Session["renid"].ToString());

             var dianzan = unitOfWork._bijiDianzansRepository.Get(filter: u => u.DakaBiji == id && u.DianzanRen == dianzanren && u.Dianzan == true);
             ViewBag.dianzan = false;
             if (dianzan.Count() > 0)
             {
                 ViewBag.dianzan = true;
             }
             return View(daka);
         }

         public ActionResult PinglunList(int bid)
         {
             var Pingluns = unitOfWork._bijiPinglunsRepository.Get(filter: u => u.DakaBiji == bid, orderBy: q => q.OrderByDescending(u => u.Id));
             ViewData["Pingluns"] = Pingluns;

             return View("~/Views/UHome/_PartialPinglun.cshtml");
         }

         //评论笔记
         [HttpPost]
         [ValidateAntiForgeryToken]
         public JsonResult PinglunBiji(int DakaRen, int PinglunRen, int Kongjian, int Peibanshi, int ProductBook, int DakaBiji, string Pinglun)
         {
             BijiPinglun pinglun = new BijiPinglun();
             pinglun.DakaRen = DakaRen;
             pinglun.PinglunContent = Pinglun;
             pinglun.PinglunRen = PinglunRen;
             pinglun.DakaBiji = DakaBiji;
             pinglun.Kongjian = Kongjian;
             pinglun.Peibanshi = Peibanshi;
             pinglun.ProductBook = ProductBook;
             pinglun.CreateTime = DateTime.Now;
             Message msg = new Message();

             try
             {

                 unitOfWork._bijiPinglunsRepository.Insert(pinglun);
                 unitOfWork.Save();

                 msg.MessageStatus = "true";
                 msg.MessageInfo = "评论成功";
             }
             catch
             {
                 msg.MessageStatus = "false";
                 msg.MessageInfo = "评论失败";
             }

             return Json(msg, JsonRequestBehavior.AllowGet);
         }

         //喜欢笔记
         [HttpPost]
         [ValidateAntiForgeryToken]
         public JsonResult XihuanBiji(int DakaRen, int PinglunRen, int Kongjian, int Peibanshi, int ProductBook, int DakaBiji, bool status)
         {
             BijiDianzan xihuan = new BijiDianzan();
             xihuan.DakaRen = DakaRen;
             xihuan.Dianzan = status;
             xihuan.DakaBiji = DakaBiji;
             xihuan.DianzanRen = PinglunRen;
             xihuan.Kongjian = Kongjian;
             xihuan.Peibanshi = Peibanshi;
             xihuan.ProductBook = ProductBook;
             Message msg = new Message();
             try
             {
                 var dianzan = unitOfWork._bijiDianzansRepository.Get(filter: u => u.DakaBiji == DakaBiji && u.DianzanRen == PinglunRen);

                 if (dianzan.Count() > 0)
                 {
                     BijiDianzan _xihuan = dianzan.First();
                     _xihuan.Dianzan = xihuan.Dianzan;
                     _xihuan.CreateTime = System.DateTime.Now;
                     unitOfWork._bijiDianzansRepository.Update(_xihuan);
                 }
                 else
                 {
                     xihuan.CreateTime = System.DateTime.Now;
                     unitOfWork._bijiDianzansRepository.Insert(xihuan);
                 }
                 unitOfWork.Save();

                 msg.MessageStatus = "true";
                 msg.MessageInfo = "点赞成功";
             }
             catch
             {
                 msg.MessageStatus = "false";
                 msg.MessageInfo = "点赞失败";
             }

             return Json(msg, JsonRequestBehavior.AllowGet);
         }

         //喜欢评论
         [HttpPost]
         [ValidateAntiForgeryToken]
         public JsonResult XihuanPinglun(int DianzanRen,int DakaBiji, int DakaRen, int Kongjian, int ProductBook, int Peibanshi, int DzPinglun, int Pinglunren, bool status)
         {
             DianzanPinglun dianzan = new DianzanPinglun();
             dianzan.Dianzan = status;
             dianzan.DakaRen = DakaRen;
             dianzan.Kongjian = Kongjian;
             dianzan.ProductBook = ProductBook;
             dianzan.Peibanshi = Peibanshi;
             dianzan.DzPinglun = DzPinglun;
             dianzan.Pinglunren = Pinglunren;
             dianzan.DianzanRen = DianzanRen;
             dianzan.DakaBiji = DakaBiji;
             dianzan.CreateTime = System.DateTime.Now;

             Message msg = new Message();
             try
             {
                 var dianzan_pinglun = unitOfWork._dianzanPinglunsRepository.Get(filter: u => u.DzPinglun == DzPinglun && u.DianzanRen == DianzanRen);

                 if (dianzan_pinglun.Count() > 0)
                 {
                     DianzanPinglun _dianzan = dianzan_pinglun.First();
                     _dianzan.Dianzan = dianzan.Dianzan;
                     _dianzan.CreateTime = System.DateTime.Now;
                     unitOfWork._dianzanPinglunsRepository.Update(_dianzan);
                 }
                 else
                 {
                     dianzan.CreateTime = System.DateTime.Now;
                     unitOfWork._dianzanPinglunsRepository.Insert(dianzan);
                 }
                 unitOfWork.Save();

                 msg.MessageStatus = "true";
                 msg.MessageInfo = "点赞成功";
             }
             catch
             {
                 msg.MessageStatus = "false";
                 msg.MessageInfo = "点赞失败";
             }

             return Json(msg, JsonRequestBehavior.AllowGet);
         }

         //评论笔记
         [HttpPost]
         [ValidateAntiForgeryToken]
         public JsonResult HuifuPinglun(string PinglunContent, int PinglunRen, int PinglunReplyren, int ReplyPinlun, int DakaRen, int Peibanshi, int ProductBook, int DakaBiji, int Kongjian)
         {
             PinglunReply huifupinglun = new PinglunReply();
             huifupinglun.DakaRen = DakaRen;
             huifupinglun.PinglunContent = PinglunContent;
             huifupinglun.PinglunRen = PinglunRen;
             huifupinglun.DakaBiji = DakaBiji;
             huifupinglun.Kongjian = Kongjian;
             huifupinglun.Peibanshi = Peibanshi;
             huifupinglun.ProductBook = ProductBook;
             huifupinglun.PinglunReplyren = PinglunReplyren;
             huifupinglun.ReplyPinlun = ReplyPinlun;
             huifupinglun.CreateTime = DateTime.Now;
             Message msg = new Message();

             try
             {

                 unitOfWork._pinglunReplysRepository.Insert(huifupinglun);
                 unitOfWork.Save();

                 msg.MessageStatus = "true";
                 msg.MessageInfo = "回复评论成功";
             }
             catch
             {
                 msg.MessageStatus = "false";
                 msg.MessageInfo = "回复评论失败";
             }

             return Json(msg, JsonRequestBehavior.AllowGet);
         }


    }
}