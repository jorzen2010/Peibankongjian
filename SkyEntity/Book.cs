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

    }


    public class RenwuList
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

    }
}
