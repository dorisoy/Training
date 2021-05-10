using Dapper.Contrib.Extensions;
using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;

namespace ISTraining_Part.Core.Models
{

    [Table("staff")]
    public class Staff : People, ICloneable
    {
        /// <summary>
        /// 位置.
        /// </summary>
        [Display(Name = "位置")]
        [Required(ErrorMessage = "{0} 不能为空", AllowEmptyStrings = false)]
        [JsonProperty("p")]
        public string 位置 { get; set; }


        public object Clone()
        {
            return MemberwiseClone();
        }
    }
}