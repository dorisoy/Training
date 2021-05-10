using ISTraining_Part.Core.ViewModels;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace ISTraining_Part.Core.Models
{

    public class LoginUser : ValidateViewModel
    {
        /// <summary>
        /// 账户
        /// </summary>
        [Display(Name = "账户")]
        [Required(ErrorMessage = "{0} 不能未空", AllowEmptyStrings = false)]
        [StringLength(20, MinimumLength = 4, ErrorMessage = "{0}不应大于{1}或小于{2}字符")]
        [RegularExpression("^[a-zA-Z0-9]+$", ErrorMessage = "{0}应包括拉丁字母和数字")]
        [JsonProperty("l")]
        public string Login { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        [Display(Name = "密码")]
        [Required(ErrorMessage = "{0} 不能未空", AllowEmptyStrings = false)]
        [StringLength(20, MinimumLength = 4, ErrorMessage = "{0}不应大于{1}或小于{2}字符")]
        [RegularExpression("^[a-zA-Z0-9]+$", ErrorMessage = "{0}应包括拉丁字母和数字")]
        [JsonProperty("p")]
        public string Password { get; set; }
    }
}
