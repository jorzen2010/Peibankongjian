using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SkyDal;
using SkyEntity;

namespace SkyService
{
    public class WechatReplyService
    {
        public static WechatReply  GetWechatReplyByKey(string  keyword)
        {
            UnitOfWork unitOfWork = new UnitOfWork();
            WechatReply wechatReply=new WechatReply();
            var wechatReplys = unitOfWork.wechatReplysRepository.Get(filter: u => u.ReplyKeyword == keyword);
            if (wechatReplys.Count() > 0)
            {
                wechatReply = wechatReplys.First();
            }
            else
            {
                keyword = "默认回复";
                var defaultReplys = unitOfWork.wechatReplysRepository.Get(filter: u => u.ReplyKeyword == keyword);
                wechatReply = defaultReplys.First();
            }

            return wechatReply;
        }
    }
}
