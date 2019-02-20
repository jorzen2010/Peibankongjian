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
        public JsonResult EntrySpace(int rid,int kid,bool status)
        {
            Message msg = new Message();

            RenKongList rklist=new RenKongList();
            rklist.Kongjian = kid;
            rklist.Shenqingren = rid;
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

        public ActionResult ShenqingPeiban()
        {
            
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


             return View(daka);
         }

    }
}