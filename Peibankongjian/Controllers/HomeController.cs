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
    public class HomeController : Controller
    {
        private UnitOfWork unitOfWork = new UnitOfWork();
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult ChengzhangList(int? page)
        {
            Pager pager = new Pager();
            pager.table = "RenwuDaka";
            pager.strwhere = "Status='false'";
            pager.PageSize = 12;
            pager.PageNo = page ?? 1;
            pager.FieldKey = "Id";
            pager.FiledOrder = "Id desc";

            pager = CommonDal.GetPager(pager);
            IList<RenwuDaka> dataList = DataConvertHelper<RenwuDaka>.ConvertToModel(pager.EntityDataTable);
            var PageList = new StaticPagedList<RenwuDaka>(dataList, pager.PageNo, pager.PageSize, pager.Amount);
            return View(PageList);
        }

      
      

        public ActionResult Shudan(int? page)
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

        public ActionResult Product(int? page)
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

        public ActionResult RenwuList(int id)
        { 
            Product product = unitOfWork.productsRepository.GetByID(id);
            Book book = unitOfWork.booksRepository.GetByID(product.ProductBook);
            var renwus = unitOfWork.renwusRepository.Get(filter: r => r.RenwuBook == book.Id);
            ViewData["probook"] = book;
            ViewData["proRenwus"] = renwus;

            return View(product);
        }


    }
}