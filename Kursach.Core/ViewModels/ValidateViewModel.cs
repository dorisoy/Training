using Dapper.Contrib.Extensions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace ISTraining_Part.Core.ViewModels
{


    /// <summary>
    /// 模型验证
    /// </summary>
    public abstract class ValidateViewModel : INotifyPropertyChanged, IDataErrorInfo
    {
        public event PropertyChangedEventHandler PropertyChanged;

       /// <summary>
       /// 获取属性值
       /// </summary>
       /// <param name="propName"></param>
       /// <returns></returns>
        private object GetPropertyValue(string propName) => GetType().GetProperty(propName).GetValue(this);


        [Write(false)]
        [JsonIgnore]
        [ChangeIgnore]
        public string Error => string.Join(Environment.NewLine, GetValidationErrors());


        [Write(false)]
        [JsonIgnore]
        [ChangeIgnore]
        public bool IsValid => !GetValidationErrors().Any();


        [Write(false)]
        [JsonIgnore]
        [ChangeIgnore]
        public string this[string propName] => GetValidationError(propName);


        protected string GetValidationError(string propertyName)
        {
            string error = string.Empty;
            var context = new ValidationContext(this) { MemberName = propertyName };
            var results = new List<ValidationResult>();

            if (!Validator.TryValidateProperty(GetPropertyValue(propertyName), context, results))
            {
                error = results.First().ErrorMessage;
            }

            return error;
        }

        protected IEnumerable<ValidationResult> GetValidationErrors()
        {
            var context = new ValidationContext(this);
            var results = new List<ValidationResult>();
            Validator.TryValidateObject(this, context, results, true);
            return results;
        }
    }
}
