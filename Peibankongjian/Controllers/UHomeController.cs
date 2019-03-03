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
    public class UHomeController : UBaseController
    {
        private UnitOfWork unitOfWork = new UnitOfWork();
        public ActionResult Index(int? page)
        {
            //Pager pager = new Pager();
            //pager.table = "Product";
            //pager.strwhere = "1=1";
            //pager.PageSize = 12;
            //pager.PageNo = page ?? 1;
            //pager.FieldKey = "Id";
            //pager.FiledOrder = "Id desc";

            //pager = CommonDal.GetPager(pager);
            //IList<Product> dataList = DataConvertHelper<Product>.ConvertToModel(pager.EntityDataTable);
            //var PageList = new StaticPagedList<Product>(dataList, pager.PageNo, pager.PageSize, pager.Amount);
            //return View(PageList);
            int rid = int.Parse(Session["renid"].ToString());
            Pager pager = new Pager();
            pager.table = "RenkongList";
            pager.strwhere = "Shenqingren="+rid+" and Status='true'";
            pager.PageSize = 12;
            pager.PageNo = page ?? 1;
            pager.FieldKey = "Id";
            pager.FiledOrder = "Id desc";

            pager = CommonDal.GetPager(pager);
            IList<RenKongList> dataList = DataConvertHelper<RenKongList>.ConvertToModel(pager.EntityDataTable);
            var PageList = new StaticPagedList<RenKongList>(dataList, pager.PageNo, pager.PageSize, pager.Amount);
            return View(PageList);
        }

        public ActionResult AccountSet()
        {
            int rid=int.Parse(Session["renid"].ToString());
            Ren ren = unitOfWork.rensRepository.GetByID(rid);
            return View(ren);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult AccountSet(Ren ren)
        {
            if (ModelState.IsValid)
            {
                unitOfWork.rensRepository.Update(ren);
                unitOfWork.Save();
                ViewBag.msg = "个人资料已更新！";
                return View(ren);
            }
            ViewBag.msg = "发生错误，个人资料未更新！";

            return View(ren);
        }
        public ActionResult EditPassword()
        {
            int rid = int.Parse(Session["renid"].ToString());
            Ren ren = unitOfWork.rensRepository.GetByID(rid);
            ViewBag.rid = rid;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult EditPassword(FormCollection fc)
        {
            string old_password = fc["RenPassword"].ToString();
            string new_password = fc["NewPassword"].ToString();
            string re_password = fc["RePassword"].ToString();
            int rid = int.Parse(fc["Id"].ToString());
            Ren ren = unitOfWork.rensRepository.GetByID(rid);

            if (string.IsNullOrEmpty(old_password) || string.IsNullOrEmpty(new_password) || string.IsNullOrEmpty(re_password))
            {
                ViewBag.msg = "密码不能为空";
                ViewBag.rid = ren.Id;
                return View();
            }
            else
            {
                if (ren.RenPassword == CommonTools.ToMd5(old_password))
                {
                    if (new_password == re_password)
                    {
                        ren.RenPassword = CommonTools.ToMd5(new_password);
                        unitOfWork.rensRepository.Update(ren);
                        unitOfWork.Save();
                        ViewBag.msg = "个人资料已更新！";
                        ViewBag.rid = ren.Id;
                        return View();
                    }
                    else
                    {
                        ViewBag.msg = "两次输入密码不一致，请重新输入";
                        ViewBag.rid = ren.Id;
                        return View();
                    }

                }
                else
                {
                    ViewBag.msg = "旧密码输入错误";
                    ViewBag.rid = ren.Id;
                    return View();
                }
 
            }

          
            
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
           int rid = int.Parse(Session["renid"].ToString());
           Ren ren = unitOfWork.rensRepository.GetByID(rid);
           ViewData["userinfo"] = ren;
           return View("~/Views/UHome/_PartialUserInfo.cshtml");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult EntrySpace(int rid,int kid,int bid,int pid,bool status)
        {
            Message msg = new Message();

            var rks = unitOfWork.renKongListsRepository.Get(
                filter: u => u.Kongjian == kid && u.Shenqingren == rid && u.Peibanshi == pid && u.ProductBook == bid);

            if (rks.Count() > 0)
            {
                RenKongList _renKongList = rks.First();
                _renKongList.Status = status;
                unitOfWork.renKongListsRepository.Update(_renKongList);
                unitOfWork.Save();
                msg.MessageStatus = "true";
                msg.MessageInfo = "重新申请进入成功";
            }
            else
            {
                RenKongList rklist = new RenKongList();
                rklist.Kongjian = kid;
                rklist.Shenqingren = rid;
                rklist.Peibanshi = pid;
                rklist.ProductBook = bid;
                rklist.Status = status;


                unitOfWork.renKongListsRepository.Insert(rklist);
                unitOfWork.Save();

                msg.MessageStatus = "true";
                msg.MessageInfo = "申请进入成功";

            }

            //  ChanpinOrder _chanpinOrder = unitOfWork.chanpinOrdersRepository.Get();

            

            return Json(msg, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult QuitSpace(int rid, int kid, int bid, int pid, bool status)
        {
            Message msg = new Message();

            ////  ChanpinOrder _chanpinOrder = unitOfWork.chanpinOrdersRepository.Get();

            //RenKongList rklist = new RenKongList();
            //rklist.Kongjian = kid;
            //rklist.Shenqingren = rid;
            //rklist.Peibanshi = pid;
            //rklist.ProductBook = bid;
            //rklist.Status = status;

            var rks=unitOfWork.renKongListsRepository.Get(
                filter: u => u.Kongjian == kid && u.Shenqingren == rid && u.Peibanshi == pid && u.ProductBook == bid);
            RenKongList rklist = rks.First();
            rklist.Status = status;
            try
            {
                unitOfWork.renKongListsRepository.Update(rklist);
                unitOfWork.Save();
                msg.MessageStatus = "true";
                msg.MessageInfo = "退出成功";
            }
            catch (Exception)
            {
                msg.MessageStatus = "true";
                msg.MessageInfo = "退出失败";
                
                throw;
            }
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
                chanpinOrder.Status = "false";
                chanpinOrder.PayStatus = "false";
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
             Ren ren = unitOfWork.rensRepository.GetByID(daka.RenwuZhixingzhe);
             Renwu renwu = unitOfWork.renwusRepository.GetByID(daka.RenwuName);
             if (ModelState.IsValid)
             {

                 unitOfWork.renwuDakasRepository.Insert(daka);
                 unitOfWork.Save();

                 WechatTemplateMessage msgData = new WechatTemplateMessage
                 {
                     touser = ren.RenOpenid,
                     template_id = "YfnxngfPAXv5hgSkDGKS-3bd5aScpZgwlRr1Jn85fWc",
                     url = "http://peiban.zzd123.com/UHome/DakaContent?id="+daka.Id,
                     data = new
                     {

                         first = new
                         {
                             value = "你好，完成作业通知。",
                             color = "#173177"
                         },
                         keyword1 = new
                         {
                             value = ren.RenNickName,
                             color = "#173177"
                         },
                         keyword2 = new
                         {
                             value = renwu.Title,
                             color = "#173177"
                         },
                         keyword3 = new
                         {
                             value = DateTime.Now.ToString("yyyy-MM-dd hh:mm"),
                             color = "#173177"
                         },
                         remark = new
                         {
                             value = "每一次陪伴都是人生的美好。"
                         }
                     }
                 };

                 string access_token = AccessTokenService.GetAccessToken();
                 string postdata = JsonConvert.SerializeObject(msgData);

                 string result = WechatMessageServices.SendTempletMessge(access_token, postdata);

                 WechatResult wechatResult = JsonConvert.DeserializeObject<WechatResult>(result);
                 if (wechatResult.errcode == 0)
                 {
                     ViewBag.msg = "模板消息发送成功！操作代码如下：";
                     ViewBag.result = result;
                 }
                 else
                 {
                     ViewBag.msg = "模板消息发送失败！错误代码如下：";
                     ViewBag.result = result;

                 }
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