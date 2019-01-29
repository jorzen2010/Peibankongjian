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
        public int Shangxiang { get; set; }

    }

    public class Renwu
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "任务描述")]
        [Required(ErrorMessage = "任务描述")]
        public string Title { get; set; }

        [Display(Name = "任务对象")]
        public int RenwuDuixiang { get; set; }

        [Display(Name = "视频地址")]

        public string VideoUrl { get; set; }

        [Display(Name = "音频地址")]

        public string AudioUrl { get; set; }



    }


    public class RenwuDaka
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "任务名")]
        public int RenwuName { get; set; }

        [Display(Name = "发布任务者")]
        public int RenwuFabuzhe { get; set; }

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
        [Display(Name = "密码")]
        public string RenPassword { get; set; }

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


        [Display(Name = "手机号码")]
        public string RenPhone { get; set; }       
        [Display(Name = "注册时间")]
        public DateTime CreateTime { get; set; }
        [Display(Name = "用户状态")]
        public bool Status { get; set; }
       
        [Display(Name = "邀请人")]
        public string Yaoqingren { get; set; }
    }
}
