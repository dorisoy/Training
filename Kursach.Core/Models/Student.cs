using Dapper.Contrib.Extensions;
using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;

namespace ISTraining_Part.Core.Models
{

    [Table("students")]
    public class Student : People, ICloneable
    {

        [Display(Name = "组Id")]
        [Range(0, double.MaxValue, ErrorMessage = "{0} 必须在正数范围内")]
        [JsonProperty("g")]
        public int GroupId { get; set; } = -1;

        /// <summary>
        /// PoPk号码
        /// </summary>
        [Display(Name = "PoPk号码")]
        [Required(ErrorMessage = "{0} 不能为空", AllowEmptyStrings = false)]
        [JsonProperty("po")]
        public string PoPkNumber { get; set; }

        /// <summary>
        /// 生日
        /// </summary>
        [Display(Name = "生日")]
        [Required(ErrorMessage = "{0} 不能为空", AllowEmptyStrings = false)]
        [JsonProperty("bd")]
        public DateTime? Birthdate { get; set; }

        /// <summary>
        /// 登记号
        /// </summary>
        [Display(Name = "登记号")]
        [Required(ErrorMessage = "{0} 不能为空", AllowEmptyStrings = false)]
        [JsonProperty("decr")]
        public string DecreeOfEnrollment { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [JsonProperty("n")]
        public string Notice { get; set; }

        /// <summary>
        /// 是否开除
        /// </summary>
        [JsonProperty("e")]
        public bool Expelled { get; set; }

        /// <summary>
        /// 是否休学
        /// </summary>
        [JsonProperty("sab")]
        public bool OnSabbatical { get; set; }


        public object Clone()
        {
            return MemberwiseClone();
        }
    }
}