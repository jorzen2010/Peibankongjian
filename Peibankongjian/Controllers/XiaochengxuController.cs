using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SkyCommon;
using SkyDal;
using SkyEntity;

namespace Peibankongjian.Controllers
{
    public class XiaochengxuController : Controller
    {
        private UnitOfWork unitOfWork = new UnitOfWork();


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

    }
}