using Dapper.Contrib.Extensions;
using ISTraining_Part.Core.ViewModels;
using Newtonsoft.Json;
using PropertyChanged;
using System;
using System.ComponentModel.DataAnnotations;

namespace ISTraining_Part.Core.Models
{
    /// <summary>
    /// 组.
    /// </summary>
    [Table("groups")]
    public class Group : ValidateViewModel, ICloneable
    {
        /// <summary>
        /// Id.
        /// </summary>
        [DoNotNotify]
        [Dapper.Contrib.Extensions.Key]
        public int Id { get; set; }

        /// <summary>
        /// 组名称.
        /// </summary>
        [Display(Name = "组名称")]
        [Required(ErrorMessage = "{0}不能为空", AllowEmptyStrings = false)]
        [RegularExpression("^[а-z-Z]{1,3}-[0-9]{1,3}$", ErrorMessage = "{0} 应以 'АА-11'")]
        [JsonProperty("n")]
        public string Name { get; set; }

        /// <summary>
        /// 负责人.
        /// </summary>
        [Display(Name = "负责人")]
        [Range(0, double.MaxValue, ErrorMessage = "{0}应在正数范围内")]
        [JsonProperty("c")]
        public int CuratorId { get; set; } = -1;

        /// <summary>
        /// 专业
        /// </summary>
        [Display(Name = "专业")]
        [Required(ErrorMessage = "{0}不能空", AllowEmptyStrings = false)]
        [JsonProperty("s")]
        public string Specialty { get; set; }

        /// <summary>
        /// 开始时间
        /// </summary>
        [Display(Name = "开始时间")]
        [Required(ErrorMessage = "{0} 不能为空", AllowEmptyStrings = false)]
        [JsonProperty("st")]
        public DateTime? Start { get; set; }

        /// <summary>
        /// 结束时间
        /// </summary>
        [Display(Name = "结束时间")]
        [Required(ErrorMessage = "{0} 不能为空", AllowEmptyStrings = false)]
        [JsonProperty("en")]
        public DateTime? End { get; set; }

        /// <summary>
        /// 是否预算
        /// </summary>
        [JsonProperty("b")]
        public bool IsBudget { get; set; } = true;

        /// <summary>
        /// 分部.
        /// </summary>
        [Display(Name = "分部")]
        [Range(0, 2, ErrorMessage = "{0} 应该在0到2之间")]
        [JsonProperty("d")]
        public int Division { get; set; }

        [Display(Name = "SpoNpo")]
        [Range(0, 2, ErrorMessage = "{0}应在0至2之间")]
        [JsonProperty("sp")]
        public int SpoNpo { get; set; }

        /// <summary>
        /// IsIntramural
        /// </summary>
        [JsonProperty("i")]
        public bool IsIntramural { get; set; } = true;

        /// <summary>
        /// Clone
        /// </summary>
        /// <returns></returns>
        public object Clone()
        {
            return MemberwiseClone();
        }

        public override bool Equals(object obj)
        {
            return (obj is Group group) && group.Id == Id;
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
