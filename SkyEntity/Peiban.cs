using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace SkyEntity
{
    public class Book
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "标题")]
        [Required(ErrorMessage = "请输入标题")]
        public string Title { get; set; }      

        [Display(Name = "类别")]
        [Required(ErrorMessage = "类别不能可为空")]
        public int Category { get; set; }

        [Display(Name = "作者")]
        public string Author { get; set; }

        [Display(Name = "标签")]
        public string Tags { get; set; }

        [Display(Name = "封面")]
        public string Cover { get; set; }

        [Display(Name = "创建时间")]
        public DateTime CreateTime { get; set; }
        [Required(ErrorMessage = "请输入内容")]
        [Display(Name = "内容")]
        public string Content { get; set; }

        [Display(Name = "排序")]
        public int Paixu { get; set; }

        [Display(Name = "上线")]
        public bool Shangxian { get; set; }

    }

    public class Renwu
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "任务描述")]
        [Required(ErrorMessage = "任务描述")]
        public string Title { get; set; }

        [Required(ErrorMessage = "请输入任务详情")]
        [Display(Name = "任务详情")]
        public string Content { get; set; }

        [Display(Name = "视频地址")]

        public string VideoUrl { get; set; }

        [Display(Name = "音频地址")]

        public string AudioUrl { get; set; }

        [Display(Name = "排序")]
        public int Paixu { get; set; }

        [Display(Name = "任务对象")]
        public int RenwuBook { get; set; }

        [Display(Name = "允许视频")]
        public bool IfVideoOn { get; set; }

        [Display(Name = "允许音频")]
        public bool IfAudioOn { get; set; }

    }


    public class Product
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "商品名称")]
        public string Title { get; set; }

        [Display(Name = "商品源")]
        public int ProductBook { get; set; }

        [Display(Name = "陪伴师")]
        public int Peibanshi { get; set; }

        [Display(Name = "价格")]
        public float Price { get; set; }

        [Display(Name = "上线")]
        public bool Shangxian { get; set; }

        [Display(Name = "体验课")]
        public bool Tiyan { get; set; }

    }


    public class RenwuDaka
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "任务名")]
        public int RenwuName { get; set; }

        [Display(Name = "商品源")]
        public int ProductBook { get; set; }

        [Display(Name = "商品源")]
        public int ProductName { get; set; }

        [Display(Name = "陪伴师")]
        public int Peibanshi { get; set; }

        [Display(Name = "任务执行人")]
        public int RenwuZhixingzhe { get; set; }

        [Display(Name = "执行状态")]
        public bool Status { get; set; }

        [Display(Name = "打卡内容")]
        public string DakaContent { get; set; }

        [Display(Name = "打卡时间")]
        public DateTime DakaTime { get; set; }

        [Display(Name = "最后修改时间")]
        public DateTime LastEditTime { get; set; }

    }

    public class Ren
    {
        [Key]
        public int Id { get; set; }
      
        [Display(Name = "姓名")]
        public string RenName { get; set; }
       
        [Display(Name = "生日")]
        public string RenBirthday { get; set; }
        [Display(Name = "一句话介绍")]
        public string RenYijuhua { get; set; }
        [Display(Name = "个人简介")]
        public string RenInfo { get; set; }


        [Display(Name = "注册邮箱")]
        public string RenUserEmail { get; set; }
       

        [Display(Name = "头像")]
        public string RenAvatar { get; set; }
        [Display(Name = "微信Openid")]
        public string RenOpenid { get; set; }
        [Display(Name = "微信Unionid")]
        public string RenUnionid { get; set; }
        [Display(Name = "性别")]
        public string RenSex { get; set; }
        [Display(Name = "省份")]
        public string Province { get; set; }
        [Display(Name = "城市")]
        public string City { get; set; }
        [Display(Name = "国家")]
        public string Country { get; set; }
        [Display(Name = "昵称")]
        public string RenNickName { get; set; }

            
        [Display(Name = "注册时间")]
        public DateTime CreateTime { get; set; }
        [Display(Name = "用户状态")]
        public bool Status { get; set; }
        [Display(Name = "发送陪伴信息")]
        public bool MsgStatus { get; set; }


       [Required(ErrorMessage = "请输入邀请码")]
        [Display(Name = "邀请码")]
        public int Yaoqingren { get; set; }
       [Required(ErrorMessage = "请输入密码")]
       [Display(Name = "密码")]
       public string RenPassword { get; set; }
       [Required(ErrorMessage = "请输入手机号")]
       [Display(Name = "手机号码")]
       public string RenPhone { get; set; }  
    }

    public class ChanpinOrder
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "订单号")]
        public string OrderNumber { get; set; }

        [Display(Name = "姓名")]
        public int VipUser { get; set; }

        [Display(Name = "邀请人")]
        public int Yaoqingren { get; set; }

        [Display(Name = "购买产品类型")]
        public int ProductType { get; set; }

        [Display(Name = "购买产品")]
        public int BuyChanpin { get; set; }

        [Display(Name = "订单备注")]
        public string Beizhu { get; set; }

        [Display(Name = "注册时间")]
        public DateTime CreateTime { get; set; }

        [Display(Name = "支付审核时间")]
        public DateTime PayTime { get; set; }
        [Display(Name = "审核状态")]
        public bool Status { get; set; }
        [Display(Name = "邀请人支付状态")]
        public bool PayStatus { get; set; }
    }
}
