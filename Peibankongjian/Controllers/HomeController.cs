﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using SkyCommon;
using SkyEntity;

namespace Peibankongjian.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
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

        public ActionResult RenwuList(int? page,int bid,int pid)
        {
            Pager pager = new Pager();
            pager.table = "Renwu";
            pager.strwhere = "RenwuBook="+bid;
            pager.PageSize = 12;
            pager.PageNo = page ?? 1;
            pager.FieldKey = "Id";
            pager.FiledOrder = "Id asc";

            pager = CommonDal.GetPager(pager);
            IList<Renwu> dataList = DataConvertHelper<Renwu>.ConvertToModel(pager.EntityDataTable);
            var PageList = new StaticPagedList<Renwu>(dataList, pager.PageNo, pager.PageSize, pager.Amount);
            ViewBag.product = pid;
            return View(PageList);
        }
    }
}