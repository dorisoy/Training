using Dapper.Contrib.Extensions;
using Newtonsoft.Json;
using PropertyChanged;
using System;

namespace ISTraining_Part.Core.Models
{
    [Table("signInLogs")]
    public class SignInLog
    {
        /// <summary>
        /// Id.
        /// </summary>
        [DoNotNotify]
        [Key]
        public int Id { get; set; }


        [DoNotNotify]
        [JsonProperty("u")]
        public int UserId { get; set; }

        /// <summary>
        /// 登录时间和日期
        /// </summary>
        [DoNotNotify]
        [JsonProperty("d")]
        public DateTime Date { get; set; } = DateTime.Now;
    }
}
