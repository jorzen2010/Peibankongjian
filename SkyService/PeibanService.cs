using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SkyDal;
using SkyEntity;
using SkyCommon;

namespace SkyService
{
    public class PeibanService
    {
        public static Book GetBookById(int id)
        {
            UnitOfWork unitOfWork = new UnitOfWork();
            Book book = unitOfWork.booksRepository.GetByID(id);
            return book;
        }

        public static Ren GetRenById(int id)
        {
            UnitOfWork unitOfWork = new UnitOfWork();
            Ren ren = unitOfWork.rensRepository.GetByID(id);
            return ren;
        }

        public static Product GetProductById(int id)
        {
            UnitOfWork unitOfWork = new UnitOfWork();
            Product _product = unitOfWork.productsRepository.GetByID(id);
            return _product;
        }

        public static Renwu GetRenwuById(int id)
        {
            UnitOfWork unitOfWork = new UnitOfWork();
            Renwu renwu = unitOfWork.renwusRepository.GetByID(id);
            return renwu;
        }

        public static Product GetKongjianById(int id)
        {
            UnitOfWork unitOfWork = new UnitOfWork();
            Product _product = unitOfWork.productsRepository.GetByID(id);
            return _product;
        }

        public static bool GetVipByRen(int ptid,int cid,int rid)
        {
            //producttyep ：1表示 会员成品 2表示课程产品 
            //buychanpin：1表示大众会员 2表示陪伴式会员 其他数字表示产品ID
            UnitOfWork unitOfWork = new UnitOfWork();
            bool status = false;
            var orders = unitOfWork.chanpinOrdersRepository.Get(filter:u=>u.VipUser==rid&&u.ProductType==ptid&&u.BuyChanpin==cid&&u.Status=="true");
            if (orders.Count() > 0)
            {
                foreach (ChanpinOrder o in orders)
                {
                    if (DateTime.Parse(o.ExpiredTime) > DateTime.Now)
                    {
                        status = true;
                        return status;
                    }
                }
                return status; 
            }
            else
            {
                return status; 
            }
        }

        public static Message GetStatusBySpaceRen(int rid,int kid)
        {
            Message msg=new Message();
            UnitOfWork unitOfWork = new UnitOfWork();
            var rklist = unitOfWork.renKongListsRepository.Get(filter: u => u.Kongjian == kid && u.Shenqingren == rid && u.Status == true);
            if (rklist.Count() > 0)
            {
                msg.MessageStatus = "true";
                msg.MessageInfo = "进入陪伴空间";
                foreach (RenKongList rk in rklist)
                {
                    if (!rk.Status)
                    {
                        msg.MessageStatus = "true";
                        msg.MessageInfo = "";
                        return msg;
                    }
                }
                return msg;
            }
            else
            {
                msg.MessageStatus = "false";
                msg.MessageInfo = "申请进入陪伴空间";
                return msg;
            }
        }

        public static Message GetStatusByPreDaka(int id,int bid)
        {
            Message msg = new Message();
            UnitOfWork unitOfWork = new UnitOfWork();
            string sql = "select top 1 * from Renwu where Id<" + id + " and  RenwuBook="+bid+" order by Id DESC";
            var renwus = unitOfWork.renwusRepository.GetWithRawSql(sql);
            if (renwus.Count() > 0)
            {
                Renwu renwu = renwus.First();
                var dakas = unitOfWork.renwuDakasRepository.Get(filter: r => r.RenwuName == renwu.Id);
                if (dakas.Count() > 0)
                {
                    msg.MessageStatus = "true";
                    msg.MessageInfo = "打卡";
                }
                else
                {
                    msg.MessageStatus = "false";
                    msg.MessageInfo = "解锁";
                }
                return msg;
            }
            else
            {
                msg.MessageStatus = "true";
                msg.MessageInfo = "解锁";          
                return msg;
            }
        }

        public static List<PinglunReply> GetPinglunReplyByPinglunId(int pid)
        {
            UnitOfWork unitOfWork = new UnitOfWork();

            List<PinglunReply> pinglunReplys = unitOfWork._pinglunReplysRepository.Get(filter: u => u.ReplyPinlun == pid, orderBy: q => q.OrderByDescending(u => u.Id)) as List<PinglunReply>;

            return pinglunReplys;
        }

        public static bool GetDianzanById(int pid, int uid)
        {
            UnitOfWork unitOfWork = new UnitOfWork();
            bool status = false;

            var dzPingluns = unitOfWork._dianzanPinglunsRepository.Get(filter: u => u.DzPinglun == pid && u.DianzanRen == uid && u.Dianzan == true);
            if (dzPingluns.Count() > 0)
            {
                status = true;
            }
            return status;

        }


        public static Message GetStatusByOrder(int ptid,int cid, int rid)
        {
            Message msg = new Message();
            UnitOfWork unitOfWork = new UnitOfWork();
            var olist = unitOfWork.chanpinOrdersRepository.Get(filter: u => u.VipUser == rid && u.BuyChanpin == cid &&u.ProductType==ptid);
            if (olist.Count() > 0)
            {
                if (olist.First().Status=="ture")
                {
                    if (DateTime.Parse(olist.First().ExpiredTime) > DateTime.Now)
                    {
                        msg.MessageStatus = "true";
                        msg.MessageInfo = "有权限进入";
                        return msg;
                    }
                    else
                    {
                        if (olist.First().Status == "expired")
                        {
                            msg.MessageStatus = "expired";
                            msg.MessageInfo = "会员已经过期";
                            return msg;
                        }
                        else
                        {
                            ChanpinOrder _order = olist.First();
                            _order.Status = "expired";
                            unitOfWork.chanpinOrdersRepository.Update(_order);
                            unitOfWork.Save();
                            msg.MessageStatus = "expired";
                            msg.MessageInfo = "会员已经过期";
                            return msg;
                        }
                        
                    }
                }
                else
                {
                    msg.MessageStatus = "nopay";
                    msg.MessageInfo = "已申请，未付款";
                    return msg;
                }
            }
            else
            {
                msg.MessageStatus = "false";
                msg.MessageInfo = "未申请会员，无权进入";
                return msg;
            }
        }


        public static int GetViewCountByDakaId(int did)
        {
            UnitOfWork unitOfWork = new UnitOfWork();
            var _viewTimes = unitOfWork._viewHistorysRepository.Get(filter: u => u.DakaBiji == did);

            return _viewTimes.Count();

        }
        public static int GetSpaceCountByRenId(int rid)
        {
            UnitOfWork unitOfWork = new UnitOfWork();
            var _renkongList = unitOfWork.renKongListsRepository.Get(filter: u => u.Shenqingren == rid && u.Status == true);

            return _renkongList.Count();

        }

        public static int GetDakaCountByRenId(int rid)
        {
            UnitOfWork unitOfWork = new UnitOfWork();
            var _daka = unitOfWork.renwuDakasRepository.Get(filter: u => u.RenwuZhixingzhe == rid);

            return _daka.Count();

        }

        public static int GetHudongCountByRenId(int rid,string type)
        {
            UnitOfWork unitOfWork = new UnitOfWork();
            var _pingluns = unitOfWork._bijiPinglunsRepository.Get(filter: u => u.PinglunRen == rid);
            int bijipinglun_count = _pingluns.Count();

            var _pinglunReplys = unitOfWork._pinglunReplysRepository.Get(filter: u => u.PinglunReplyren == rid);
            int pinglunReply_count = _pinglunReplys.Count();

            var _dianzanBijis = unitOfWork._bijiDianzansRepository.Get(filter: u => u.DianzanRen == rid);
            int bijidianzan_count = _dianzanBijis.Count();

            var _dianzanPingluns = unitOfWork._dianzanPinglunsRepository.Get(filter: u => u.DianzanRen == rid);
            int pinglundianzan_count = _dianzanPingluns.Count();


            if (type == "bijipinglun")
            {
                return bijipinglun_count;
            }else if (type == "pinglunreply")
            {
                return pinglunReply_count;
            }else if (type == "bijidianzan")
            {
                return bijidianzan_count;
            }else if (type == "pinglundianzan")
            {
                return pinglundianzan_count;
            }else if (type == "all")
            {
                return bijipinglun_count + pinglunReply_count + bijidianzan_count + pinglundianzan_count;
            }
            else
            {

                return bijipinglun_count + pinglunReply_count + bijidianzan_count + pinglundianzan_count;
            }

        }
    }
}
