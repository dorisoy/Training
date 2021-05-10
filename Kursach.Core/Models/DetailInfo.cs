using Dapper.Contrib.Extensions;
using ISTraining_Part.Core.ViewModels;
using Newtonsoft.Json;
using PropertyChanged;
using System;
using System.ComponentModel.DataAnnotations;

namespace ISTraining_Part.Core.Models
{
    /// <summary>
    ///关于人的详细信息
    /// </summary>
    [Table("detailInfo")]
    public class DetailInfo : ValidateViewModel, ICloneable
    {
        /// <summary>
        /// Id.
        /// </summary>
        [DoNotNotify]
        [Dapper.Contrib.Extensions.Key]
        public int Id { get; set; }

        /// <summary>
        /// 电话
        /// </summary>
        [Display(Name = "电话")]
        [Required(ErrorMessage = "{0}不能为空", AllowEmptyStrings = false)]
        [StringLength(255, MinimumLength = 11, ErrorMessage = "{0}应包含至少11个数字")]
        [JsonProperty("p")]
        public string Phone { get; set; }

        /// <summary>
        /// 邮箱.
        /// </summary>
        [Display(Name = "邮箱")]
        [Required(ErrorMessage = "{0}不能空", AllowEmptyStrings = false)]
        [EmailAddress(ErrorMessage = "指定邮件")]
        [JsonProperty("e")]
        public string EMail { get; set; }

        /// <summary>
        /// 地址
        /// </summary>
        [Display(Name = "地址")]
        [Required(ErrorMessage = "{0}不能为空", AllowEmptyStrings = false)]
        [StringLength(255, MinimumLength = 5, ErrorMessage = "{0}应至少包含{2}字母")]
        [JsonProperty("a")]
        public string Address { get; set; }

        /// <summary>
        /// 工作Id
        /// </summary>
        [JsonProperty("si")]
        public int? Staff { get; set; }

        /// <summary>
        /// 学生Id
        /// </summary>
        [JsonProperty("sti")]
        public int? Student { get; set; }

        /// <summary>
        /// 副本.
        /// </summary>
        /// <returns></returns>
        public object Clone()
        {
            return MemberwiseClone();
        }

        public override string ToString()
        {
            return $"学生: {Student}, 工作人员: {Staff}";
        }
    }
}
