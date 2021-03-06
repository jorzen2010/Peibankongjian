﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SkyCommon;
using SkyService;
using SkyDal;
using SkyEntity;
using SkyWechatService;
using WechatXiaochengxu;

namespace Peibankongjian.Controllers
{
    public class XiaochengxuController : Controller
    {
        private UnitOfWork unitOfWork = new UnitOfWork();

        public ActionResult xcxLogin(string js_code)
        {
            string appid = XcxConfig.APPID;
            string secret = XcxConfig.SECRET;
            string grant_type = "authorization_code";
            string jsonstr = XiaochengxuApi.GetOpenidByWxlogin(appid, secret, js_code, grant_type);
            return Content(jsonstr);
        }


        public ActionResult GetuserinfoByunionid(string unionid)
        {
            string json = string.Empty;
            Ren ren = new Ren();

            var userlist = unitOfWork.rensRepository.Get(filter: u => u.RenUnionid == unionid);
            if (userlist.Count() > 0)
            {
                ren = userlist.First();
                json = JsonHelper.JsonSerializerBySingleData(ren);
            }
            else
            {
                Message msg = new Message();
                msg.MessageInfo = "此用户尚未注册";
                msg.MessageStatus = "false";
                msg.MessageUrl = "";
                json = JsonHelper.JsonSerializerBySingleData(msg);
            }
            return Content(json);

        }


      
        public ActionResult GetBijiByPid(int? page, int pid)
        {
            Pager pager = new Pager();
            pager.table = "RenwuDaka";
            pager.strwhere = "RenwuZhixingzhe=" + pid;
            pager.PageSize = 10;
            pager.PageNo = page ?? 1;
            pager.FieldKey = "Id";
            pager.FiledOrder = "Id desc";


            pager = CommonDal.GetPager(pager);
            IList<RenwuDaka> List = DataConvertHelper<RenwuDaka>.ConvertToModel(pager.EntityDataTable);

            System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
            string json = js.Serialize(new { pagecount = pager.PageCount, dakas = List });//将对象序列化成JSON字符串。匿名类。向浏览器返回多个JSON对象。 
            return Content(json);

        }

        public ActionResult GetBijiBySid(int? page, int sid)
        {
            Pager pager = new Pager();
            pager.table = "RenwuDaka";
            pager.strwhere = "Kongjian=" + sid+" and Status='false'";
            pager.PageSize = 10;
            pager.PageNo = page ?? 1;
            pager.FieldKey = "Id";
            pager.FiledOrder = "Id desc";


            pager = CommonDal.GetPager(pager);
            IList<RenwuDaka> List = DataConvertHelper<RenwuDaka>.ConvertToModel(pager.EntityDataTable);

            System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
            string json = js.Serialize(new { pagecount = pager.PageCount, dakas = List });//将对象序列化成JSON字符串。匿名类。向浏览器返回多个JSON对象。 
            return Content(json);

        }

        public ActionResult GetBijiByRenwuId(int? page, int rid)
        {
            Pager pager = new Pager();
            pager.table = "RenwuDaka";
            pager.strwhere = "RenwuName=" + rid + " and Status='false'";
            pager.PageSize = 10;
            pager.PageNo = page ?? 1;
            pager.FieldKey = "Id";
            pager.FiledOrder = "Id desc";
            pager = CommonDal.GetPager(pager);
            IList<RenwuDaka> List = DataConvertHelper<RenwuDaka>.ConvertToModel(pager.EntityDataTable);
            System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
            string json = js.Serialize(new { pagecount = pager.PageCount, dakas = List });//将对象序列化成JSON字符串。匿名类。向浏览器返回多个JSON对象。 
            return Content(json);

        }

        public ActionResult GetAllBiji(int? page)
        {
            Pager pager = new Pager();
            pager.table = "RenwuDaka";
            pager.strwhere = "1=1";
            pager.PageSize = 10;
            pager.PageNo = page ?? 1;
            pager.FieldKey = "Id";
            pager.FiledOrder = "Id desc";


            pager = CommonDal.GetPager(pager);
            IList<RenwuDaka> List = DataConvertHelper<RenwuDaka>.ConvertToModel(pager.EntityDataTable);

            System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
            string json = js.Serialize(new { pagecount = pager.PageCount, dakas = List });//将对象序列化成JSON字符串。匿名类。向浏览器返回多个JSON对象。 
            return Content(json);

        }

        public ActionResult GetBijis(int count)
        {
            string sql = "select top " + count + " * from RenwuDaka where status=0";
            var bijis = unitOfWork.renwuDakasRepository.GetWithRawSql(sql);
            IList<RenwuDaka> List = bijis as IList<RenwuDaka>;
            System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
            string json = js.Serialize(new { dakas = List });//将对象序列化成JSON字符串。匿名类。向浏览器返回多个JSON对象。 

            return Content(json);

        }


        public ActionResult GetBijiById(int id)
        {
            RenwuDaka biji = unitOfWork.renwuDakasRepository.GetByID(id);
            string json = JsonHelper.JsonSerializerBySingleData(biji);
            return Content(json);
        }

        public ActionResult GetDianzanByDakaId(int id, int count)
        {
            string sql = string.Empty;
            if (count == 0)
            {
                sql = "select  * from BijiDianzan where DakaBiji=" + id;
            }
            else
            {
               sql = "select top  " + count + " * from BijiDianzan where DakaBiji=" + id;
            }
            var dianzans = unitOfWork._bijiDianzansRepository.GetWithRawSql(sql);
            IList<BijiDianzan> List = dianzans as IList<BijiDianzan>;
            System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();

            string json = js.Serialize(new { dianzans = List,dianzansCount=List.Count });//将对象序列化成JSON字符串。匿名类。向浏览器返回多个JSON对象。 

            return Content(json);
        }

        public ActionResult GetPinglunByDakaId(int id, int count)
        {
            string sql = "select top  " + count + " * from BijiPinglun where DakaBiji=" + id;
            var pingluns = unitOfWork._bijiPinglunsRepository.GetWithRawSql(sql);
            IList<BijiPinglun> List = pingluns as IList<BijiPinglun>;
            System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
            string json = js.Serialize(new { pingluns = List,pinglunsCount=List.Count });//将对象序列化成JSON字符串。匿名类。向浏览器返回多个JSON对象。 

            return Content(json);
        }

        //public ActionResult GetPinglunBy(int peibanshi, int pinglunren,int dakaren,int kongjian,int book,int biji,int dakaren ,int count)
        //{
        //    string sql = string.Empty;
        //    if (count == 0)
        //    {
        //        sql = "select  * from BijiDianzan where 1=1";
        //        if (pid != 0)
        //        {
        //            sql = sql + " and PinglunRen=" + pid;
 
        //        }
        //        if (bid != 0)
        //        {
        //            sql = sql + " and ProductBook=" + bid;

        //        }
        //        if (kid != 0)
        //        {
        //            sql = sql + " and Kongjian=" + kid;

        //        }
        //        if (did != 0)
        //        {
        //            sql = sql + " and pid=" + did;

        //        }
        //    }
        //    else
        //    {
        //        sql = "select top  " + count + " * from BijiDianzan where DakaBiji=" + did;
        //    }

        //    string sql = "select top  " + count + " * from BijiPinglun where DakaBiji=" + did;
        //    var pingluns = unitOfWork._bijiPinglunsRepository.GetWithRawSql(sql);
        //    IList<BijiPinglun> List = pingluns as IList<BijiPinglun>;
        //    System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
        //    string json = js.Serialize(new { pingluns = List, pinglunsCount = List.Count });//将对象序列化成JSON字符串。匿名类。向浏览器返回多个JSON对象。 

        //    return Content(json);
        //}





        public ActionResult GetAllSpace(int? page)
        {
            Pager pager = new Pager();
            pager.table = "Product";
            pager.strwhere = "1=1";
            pager.PageSize = 2;
            pager.PageNo = page ?? 1;
            pager.FieldKey = "Id";
            pager.FiledOrder = "Id desc";


            pager = CommonDal.GetPager(pager);
            IList<Product> List = DataConvertHelper<Product>.ConvertToModel(pager.EntityDataTable);
            System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
            string json = js.Serialize(new { pagecount = pager.PageCount, dakas = List });//将对象序列化成JSON字符串。匿名类。向浏览器返回多个JSON对象。 
            return Content(json);

        }

        public ActionResult GetSpaceByPeibanshi(int? page, int pid)
        {
            Pager pager = new Pager();
            pager.table = "Product";
            pager.strwhere = "1=1";
            pager.PageSize = 10;
            pager.PageNo = page ?? 1;
            pager.FieldKey = "Id";
            pager.FiledOrder = "Id desc";

            pager = CommonDal.GetPager(pager);
            IList<Product> List = DataConvertHelper<Product>.ConvertToModel(pager.EntityDataTable);
            System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
            string json = js.Serialize(new { pagecount = pager.PageCount, dakas = List });//将对象序列化成JSON字符串。匿名类。向浏览器返回多个JSON对象。 
            return Content(json);

        }


        public ActionResult GetSpaceListByCount(int count)
        {
            string sql = "select top  " + count + " * from Product where Shangxian='true'";
            var spaces = unitOfWork.productsRepository.GetWithRawSql(sql);
            IList<Product> List = spaces as IList<Product>;
            System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
            string json = js.Serialize(new { spaces = List });//将对象序列化成JSON字符串。匿名类。向浏览器返回多个JSON对象。 

            return Content(json);
        }
        public ActionResult GetSpaceByUserId(int uid, int count)
        {
            string sql = string.Empty;
            if (count == 0)
            {
                sql = "select * from RenKongList where Shenqingren=" + uid + " and status='true'";
            }
            else
            {
                sql = "select top  " + count + " * from RenKongList where Shenqingren=" + uid + " and status='true'";
            }

            var spaces = unitOfWork.renKongListsRepository.GetWithRawSql(sql);
            IList<RenKongList> List = spaces as IList<RenKongList>;
            System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
            string json = js.Serialize(new { spaces = List });//将对象序列化成JSON字符串。匿名类。向浏览器返回多个JSON对象。 

            return Content(json);
        }

        public ActionResult GetSpaceById(int id)
        {
            Product _product = unitOfWork.productsRepository.GetByID(id);
            string json = JsonHelper.JsonSerializerBySingleData(_product);
            return Content(json);

        }

        public ActionResult GetUserInfoById(int id)
        {
            Ren _ren = unitOfWork.rensRepository.GetByID(id);
            string json = JsonHelper.JsonSerializerBySingleData(_ren);
            return Content(json);

        }

        public ActionResult GetBookById(int id)
        {
            Book _book = unitOfWork.booksRepository.GetByID(id);
            string json = JsonHelper.JsonSerializerBySingleData(_book);
            return Content(json);

        }

        public ActionResult GetRenwuById(int id)
        {
            Renwu _renwu = unitOfWork.renwusRepository.GetByID(id);
            string json = JsonHelper.JsonSerializerBySingleData(_renwu);
            return Content(json);

        }

        public ActionResult GetRenwuListByBookId(int bid)
        {
            string sql = "select * from Renwu where RenwuBook=" + bid + " order by Paixu";
            var renwus = unitOfWork.renwusRepository.GetWithRawSql(sql);
            IList<Renwu> List = renwus as IList<Renwu>;
            System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
            string json = js.Serialize(new { renwus = List });//将对象序列化成JSON字符串。匿名类。向浏览器返回多个JSON对象。 

            return Content(json);
        }

        public ActionResult GetBVipByPid(int id)
        {
            bool status = PeibanService.GetVipByRen(1, 1, id);
            string json = JsonHelper.JsonSerializerBySingleData(status);
            return Content(json);

        }

        public ActionResult GetSpaceByDakaId(int rid,int kid)
        {
            int next_id = 0;
            string sql = "select * from RenwuDaka where RenwuZhixingzhe=" + rid + " and Kongjian=" + kid+" order by Id desc";
            var renwudakas = unitOfWork.renwuDakasRepository.GetWithRawSql(sql);
            if (renwudakas.Count() == 0)
            {
                next_id=0;
            }
            else
            {
                RenwuDaka daka = renwudakas.First();
                int last_id = daka.RenwuName;
                string next_sql = "select top 1 * from Renwu where Id>"+last_id +" order by Paixu";
                var renwus = unitOfWork.renwusRepository.GetWithRawSql(next_sql);
                if (renwus.Count() == 0)
                {
                    next_id= last_id;
                }
                else
                {
                    Renwu renwu = renwus.First();
                    next_id = renwu.Id;
                }
            }
            string json = JsonHelper.JsonSerializerBySingleData(next_id);
            return Content(json);
        }

        public ActionResult MakeDaka(int rid,int bid,int kid,int peibanshi,int zhixingzhe,string title,string  dakacontent)
        {
            Message msg = new Message();
            RenwuDaka daka = new RenwuDaka();
            daka.RenwuName = rid;
            daka.ProductBook = bid;
            daka.Kongjian = kid;
            daka.Peibanshi = peibanshi;
            daka.RenwuZhixingzhe = zhixingzhe;
            daka.Status = false;
            daka.DakaTitle = title;
            daka.DakaContent = dakacontent;
            daka.DakaTime = DateTime.Now;
            daka.LastEditTime = DateTime.Now;
            try
            {
                unitOfWork.renwuDakasRepository.Insert(daka);
                unitOfWork.Save();
                msg.MessageStatus = "true";
                msg.MessageInfo = "打卡成功";
            }
            catch (Exception)
            {
                msg.MessageStatus = "false";
                msg.MessageInfo = "打卡失败";
                throw;
            }
           
            string json = JsonHelper.JsonSerializerBySingleData(msg);
            return Content(json);
        }

        public ActionResult DakaPinglun(int bijiid, int dakarenid, int peibanshi, int kongjianid, int bookid, int pinglunren, string pingluncontent)
        {
            Message msg = new Message();
            BijiPinglun pinglun = new BijiPinglun();
            pinglun.DakaBiji = bijiid;
            pinglun.DakaRen = dakarenid;
            pinglun.Peibanshi = peibanshi;
            pinglun.Kongjian = kongjianid;
            pinglun.ProductBook = bookid;
            pinglun.PinglunRen = pinglunren;
            pinglun.PinglunContent = pingluncontent;
            pinglun.CreateTime = DateTime.Now;
            try
            {
                unitOfWork._bijiPinglunsRepository.Insert(pinglun);
                unitOfWork.Save();
                msg.MessageStatus = "true";
                msg.MessageInfo = "评论成功";
            }
            catch (Exception)
            {
                msg.MessageStatus = "false";
                msg.MessageInfo = "评论失败";
                throw;
            }

            string json = JsonHelper.JsonSerializerBySingleData(msg);
            return Content(json);
        }

    }
}