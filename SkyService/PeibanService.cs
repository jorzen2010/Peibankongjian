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

        public static bool GetVipByRen(int rid)
        {
            UnitOfWork unitOfWork = new UnitOfWork();
            bool status = false;
            var orders = unitOfWork.chanpinOrdersRepository.Get(filter:u=>u.VipUser==rid&&u.ProductType==1&&u.BuyChanpin==2&&u.Status==true);
            if (orders.Count() > 0)
            {
                foreach (ChanpinOrder o in orders)
                {
                    if (o.PayTime.AddYears(1) > DateTime.Now)
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
            bool status = false;
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
            bool status = false;
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

    }
}
