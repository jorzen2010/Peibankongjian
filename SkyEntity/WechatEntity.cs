using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace SkyEntity
{
    public class WechatReply
    {
        [Key]
        public int Id { get; set; }
        [Display(Name = "关键词")]
        [Required(ErrorMessage = "关键词不能为空")]
        public string ReplyKeyword { get; set; }
        [Display(Name = "匹配方式")]
        [Required(ErrorMessage = "匹配方式不能为空")]
        public int Category { get; set; }
        [Display(Name = "内容")]
        [Required(ErrorMessage = "关键词回复不能为空")]
        public string ReplyContent { get; set; }


    }

  
}
