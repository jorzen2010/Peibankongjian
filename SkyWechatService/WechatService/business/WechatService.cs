using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.Xml;
using Newtonsoft.Json;
using System.Configuration;
using SkyCommon;
using SkyService;
using SkyEntity;

namespace SkyWechatService
{
    public class WechatService
    {
        public static void Auth()
        {
            Configuration config = System.Web.Configuration.WebConfigurationManager.OpenWebConfiguration("~");
            AppSettingsSection appsection = config.GetSection("appSettings") as AppSettingsSection;

            string token = appsection.Settings["WechatToken"].Value.ToString();
            string echoString = HttpContext.Current.Request.QueryString["echoStr"];
            string signature = HttpContext.Current.Request.QueryString["signature"];
            string timestamp = HttpContext.Current.Request.QueryString["timestamp"];
            string nonce = HttpContext.Current.Request.QueryString["nonce"];

            string[] ArrTmp = { token, timestamp, nonce };

            Array.Sort(ArrTmp);
            string tmpStr = string.Join("", ArrTmp);

            tmpStr = SkyEncrypt.SHA1(tmpStr);

            tmpStr = tmpStr.ToLower();

            if (tmpStr == signature)
            {
                System.Web.HttpContext.Current.Response.Write(echoString);
                System.Web.HttpContext.Current.Response.End();
            }
            else
            {
                System.Web.HttpContext.Current.Response.Write("验证不通过");
                System.Web.HttpContext.Current.Response.End();
            }

        }

        //处理微信信息
        public static void  Excute(string postStr)
        {

            Configuration config = System.Web.Configuration.WebConfigurationManager.OpenWebConfiguration("~");
            AppSettingsSection appsection = config.GetSection("appSettings") as AppSettingsSection;

            string WechatId = appsection.Settings["WechatId"].Value.ToString();

         //   LogHelper.Info("构建xml");

            XmlDocument xmldoc = new XmlDocument();
          //  LogHelper.Info("加载字符串");
            xmldoc.Load(new System.IO.MemoryStream(System.Text.Encoding.GetEncoding("GB2312").GetBytes(postStr)));
         //   LogHelper.Info("查找字节");
            XmlNode MsgType = xmldoc.SelectSingleNode("/xml/MsgType");
            XmlNode FromUserName=xmldoc.SelectSingleNode("/xml/FromUserName");
            LogHelper.Info(MsgType.InnerText);
            LogHelper.Info(FromUserName.InnerText);
            if (MsgType != null)
            {
                if (MsgType.InnerText == "event")
                {
                    XmlNode EventContent = xmldoc.SelectSingleNode("/xml/Event");
                    switch (EventContent.InnerText)
                    {
                        case "subscribe":
                            string subscribereplycontent = WechatReplyService.GetWechatReplyByKey("关注回复").ReplyContent;
                            WechatMessageServices.ResponseTextMessage(FromUserName.InnerText, WechatId, subscribereplycontent);
                            break;
                        case "unsubscribe":
                            WechatMessageServices.ResponseSuccessMessage(FromUserName.InnerText, WechatId);
                            //此处应该给数据库中添加一个字段帮助其判断是否关注了。
                          //  WechatMessageServices.ResponseTextMessage(FromUserName.InnerText, WechatId, "我哪里做的不好了，你居然敢离开我。");
                            break;
                        case "CLICK":

                           //string token = AccessTokenService.GetAccessToken();
                           //string PreMsg= "感谢您的认可,正在为您生成专属邀请卡。";
                           //string cardMediaId="slv-URo_tcvYdETXRXCkYC9LsbMh2atzB72d0NznT0o";
                           //string LastMsg="你可以把这张图片发送给需要的人，让更多星星的孩子能够健康成长。";
                           //MyWechatServices.invitationCard(token, FromUserName.InnerText, PreMsg, cardMediaId, LastMsg);
                           string CLICKreplycontent = WechatReplyService.GetWechatReplyByKey("点击回复").ReplyContent;
                           WechatMessageServices.ResponseTextMessage(FromUserName.InnerText, WechatId, CLICKreplycontent);
                            break;
                        case "LOCATION":
                          // WechatMessageServices.ResponseSuccessMessage(FromUserName.InnerText, WechatId);
                           string LOCATIONreplycontent = WechatReplyService.GetWechatReplyByKey("地理位置回复").ReplyContent;
                           WechatMessageServices.ResponseTextMessage(FromUserName.InnerText, WechatId, LOCATIONreplycontent);
                            break;
                        default:
                            //这是非常好用的一个地方，打开公众号，我就会和你打招呼。
                           string defaultreplycontent = WechatReplyService.GetWechatReplyByKey("默认回复").ReplyContent;
                           WechatMessageServices.ResponseTextMessage(FromUserName.InnerText, WechatId, defaultreplycontent);
                           break;
                    }                
                }
                else
                {
                    switch (MsgType.InnerText)
                    {
                        case "image":
                            string imgreplycontent = WechatReplyService.GetWechatReplyByKey("图片回复").ReplyContent;
                            WechatMessageServices.ResponseTextMessage(FromUserName.InnerText, WechatId, imgreplycontent);
                         break;
                        case "text":

                            XmlNode TextContent = xmldoc.SelectSingleNode("/xml/Content");
                            string keywords = TextContent.InnerText;
                            LogHelper.Info("获取关键词:" + keywords);
                            string replycontent= WechatReplyService.GetWechatReplyByKey(keywords).ReplyContent;
                            LogHelper.Info("得到回复:" + keywords);
                            WechatMessageServices.ResponseTextMessage(FromUserName.InnerText, WechatId, replycontent);
                            //if (string.IsNullOrEmpty(keywords))
                            //{
                            //    WechatMessageServices.ResponseSuccessMessage(FromUserName.InnerText, WechatId);
                            //    WechatMessageServices.ResponseTextMessage(FromUserName.InnerText, WechatId, "您好，这是自闭症儿童家庭训练和评估系统，请点击下方菜单开始使用。\n\n 如有疑问，您可以拨打13945016428联系瑞夕老师进行咨询。");
                           
                            //}
                            //else
                            //{
                            //    switch (keywords)
                            //    {
                            //        case "自闭症资料":
                            //            WechatMessageServices.ResponseTextMessage(FromUserName.InnerText, WechatId, "您好，请先分享图片到朋友圈，帮助更多的自闭症家庭。\n\n然后请使用百度网盘下载该资料。\n\n 链接地址：https://pan.baidu.com/s/1pLdwA3D \n 密码：zeju \n\n 同时建议您点击下方开始训练按钮，加入自闭症家庭训练计划，让孩子提升更快。\n\n <a href=\"http://zbz.zuyanquxian.cn/wechat/login\">点击获取更多资料</a>");
                           
                            //            break;
                            //        default:
                            //            LogHelper.Info("默认的东西");
                            //            WechatMessageServices.ResponseTextMessage(FromUserName.InnerText, WechatId, "您好，这是自闭症儿童家庭训练和评估系统，请点击下方菜单开始使用。\n\n 如有疑问，您可以拨打13945016428联系瑞夕老师进行咨询。");
                            //            break;
                            //    }

                            //}

                            break;

                        default:
                            WechatMessageServices.ResponseTextMessage(FromUserName.InnerText, WechatId, "祝星星宝贝健康快乐成长。");
                            break;
                        //这几个一般用不到，我就没做判断。
                        case "voice":
                            string voicereplycontent = WechatReplyService.GetWechatReplyByKey("语音回复").ReplyContent;
                            WechatMessageServices.ResponseTextMessage(FromUserName.InnerText, WechatId, voicereplycontent);
                            break;
                        case "video":
                            string videoreplycontent = WechatReplyService.GetWechatReplyByKey("视频回复").ReplyContent;
                            WechatMessageServices.ResponseTextMessage(FromUserName.InnerText, WechatId, videoreplycontent);
                            break;
                        case "shortvideo":
                            string shortvideoreplycontent = WechatReplyService.GetWechatReplyByKey("小视频回复").ReplyContent;
                            WechatMessageServices.ResponseTextMessage(FromUserName.InnerText, WechatId, shortvideoreplycontent);
                            break;
                        case "location":
                            string locationreplycontent = WechatReplyService.GetWechatReplyByKey("地理位置回复").ReplyContent;
                            WechatMessageServices.ResponseTextMessage(FromUserName.InnerText, WechatId, locationreplycontent);
                            break;
                        case "link":
                            string linkreplycontent = WechatReplyService.GetWechatReplyByKey("链接回复").ReplyContent;
                            WechatMessageServices.ResponseTextMessage(FromUserName.InnerText, WechatId, linkreplycontent);
                            break;
                    }
                }


            }
            else
            {

                WechatMessageServices.ResponseTextMessage(FromUserName.InnerText, WechatId, "success");
                
            }

           


        }



        public static string wechatApi(string operate, string access_token, string postdata)
        {
            string result = "";
            string url = "";
            switch (operate)
            {
                case "SendTemplateMessage":
                url = string.Format("https://api.weixin.qq.com/cgi-bin/message/template/send?access_token={0}", access_token);               
                break;

                case "GetUserListInfo":
                url = string.Format("https://api.weixin.qq.com/cgi-bin/user/info/batchget?access_token={0}", access_token);              
                break;

                case "CreateMenu":
                url = string.Format("https://api.weixin.qq.com/cgi-bin/menu/create?access_token={0}", access_token);
                break;


                    
            }
            result = HttpWebResponseUtility.PostJsonData(url, postdata);
            return result;
        }
    }
}