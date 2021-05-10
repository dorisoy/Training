using Newtonsoft.Json;

namespace ISTraining_Part.Core.Models
{

    public class StudentsCount
    {
        /// <summary>
        /// 学生数
        /// </summary>
        [JsonProperty("t")]
        public int Total { get; }


        /// <summary>
        /// 休学数
        /// </summary>
        [JsonProperty("sab")]
        public int OnSabbatical { get; }


        public StudentsCount(int total, int onSabbatical)
        {
            Total = total;
            OnSabbatical = onSabbatical;
        }
    }
}
