using Dapper.Contrib.Extensions;
using ISTraining_Part.Core.ViewModels;
using Newtonsoft.Json;
using PropertyChanged;
using System.ComponentModel.DataAnnotations;

namespace ISTraining_Part.Core.Models
{

    public class People : ValidateViewModel
    {
        /// <summary>
        /// Id.
        /// </summary>
        [Dapper.Contrib.Extensions.Key]
        public int Id { get; set; }

        /// <summary>
        /// 全名.
        /// </summary>
        [Display(Name = "全名")]
        [Required(ErrorMessage = "{0} 不能为空", AllowEmptyStrings = false)]
        [AlsoNotifyFor(nameof(FullName))]
        [JsonProperty("fn")]
        public string FirstName { get; set; }

        /// <summary>
        /// 曾用名.
        /// </summary>
        [Display(Name = "曾用名")]
        [Required(ErrorMessage = "{0} 不能为空", AllowEmptyStrings = false)]
        [AlsoNotifyFor(nameof(FullName))]
        [JsonProperty("mn")]
        public string MiddleName { get; set; }

        /// <summary>
        /// 姓.
        /// </summary>
        [Display(Name = "姓")]
        [Required(ErrorMessage = "{0} 不能为空", AllowEmptyStrings = false)]
        [AlsoNotifyFor(nameof(FullName))]
        [JsonProperty("ln")]
        public string LastName { get; set; }


        
        /// <summary>
        /// 位置.
        /// </summary>
        [Display(Name = "位置")]
        [Required(ErrorMessage = "{0} 不能为空", AllowEmptyStrings = false)]
        [AlsoNotifyFor(nameof(FullName))]
        [JsonProperty("poi")]
        public string Position { get; set; }


        [Write(false)]
        [JsonIgnore]
        [ChangeIgnore]
        public string FullName => $"{LastName} {FirstName} {MiddleName}";

        public override bool Equals(object obj)
        {
            return (obj is People people) && people.Id == Id;
        }
    }
}
