using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SkyCommon;
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
            string jsonstr= XiaochengxuApi.GetOpenidByWxlogin(appid, secret, js_code, grant_type);
            return Content(jsonstr);
        }
        

        public ActionResult GetuserinfoByunionid(string unionid)
        {
            Ren ren=new Ren();
            var userlist = unitOfWork.rensRepository.Get(filter: u => u.RenUnionid == unionid);
            if (userlist.Count() > 0)
            {
                ren = userlist.First();
            }
            string json = JsonHelper.JsonSerializerBySingleData(ren);
            return Content(json);

        }

        public ActionResult GetBijiByPid(int? page,int pid)
        {
            Pager pager = new Pager();
            pager.table = "RenwuDaka";
            pager.strwhere = "RenwuZhixingzhe="+pid;
            pager.PageSize = 2;
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
            pager.PageSize = 2;
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

        public ActionResult GetDianzanByDakaId(int id,int count)
        {
            string sql = "select top  " + count + " * from BijiDianzan where DakaBiji=" + id;
            var dianzans = unitOfWork._bijiDianzansRepository.GetWithRawSql(sql);
            IList<BijiDianzan> List = dianzans as IList<BijiDianzan>;
            System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
            string json = js.Serialize(new { dianzans = List });//将对象序列化成JSON字符串。匿名类。向浏览器返回多个JSON对象。 

            return Content(json);
        }

        public ActionResult GetPinglunByDakaId(int id,int count)
        {
            string sql = "select top  " + count + " * from BijiPinglun where DakaBiji=" + id;
            var pingluns = unitOfWork._bijiPinglunsRepository.GetWithRawSql(sql);
            IList<BijiPinglun> List = pingluns as IList<BijiPinglun>;
            System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
            string json = js.Serialize(new { pingluns = List });//将对象序列化成JSON字符串。匿名类。向浏览器返回多个JSON对象。 

            return Content(json);
        }

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

        public ActionResult GetSpaceByPeibanshi(int? page ,int pid)
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

        public ActionResult GetBookById(int id)
        {
            Book _book = unitOfWork.booksRepository.GetByID(id);
            string json = JsonHelper.JsonSerializerBySingleData(_book);
            return Content(json);

        }

        public ActionResult GetRenwuListByBookId(int bid)
        {
            string sql = "select * from Renwu where RenwuBook=" + bid+" order by Paixu";
            var renwus = unitOfWork.renwusRepository.GetWithRawSql(sql);
            IList<Renwu> List = renwus as IList<Renwu>;
            System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
            string json = js.Serialize(new { renwus = List });//将对象序列化成JSON字符串。匿名类。向浏览器返回多个JSON对象。 

            return Content(json);
        }
    }
}