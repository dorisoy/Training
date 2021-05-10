using Dapper.Contrib.Extensions;
using Newtonsoft.Json;
using PropertyChanged;
using System;
using System.ComponentModel.DataAnnotations;

namespace ISTraining_Part.Core.Models
{

    [Table("users")]
    public class User : LoginUser, ICloneable
    {
        /// <summary>
        /// ID.
        /// </summary>
        [DoNotNotify]
        [Dapper.Contrib.Extensions.Key]
        public int Id { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        [Display(Name = "名称")]
        [Required(ErrorMessage = "{0} 不能为空", AllowEmptyStrings = false)]
        [JsonProperty("n")]
        public string Name { get; set; }


        [JsonProperty("m")]
        public UserMode Mode { get; set; }


        public object Clone()
        {
            return MemberwiseClone();
        }

        public override bool Equals(object obj)
        {
            return (obj is User user) && user.Id == Id;
        }
    }
}
