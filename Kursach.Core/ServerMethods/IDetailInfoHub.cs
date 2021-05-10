using ISTraining_Part.Core.Models;
using ISTraining_Part.Core.Models.Enums;
using System.Threading.Tasks;

namespace ISTraining_Part.Core.ServerMethods
{

    public interface IDetailInfoHub
    {
        Task<ISTrainingPartResponse<DetailInfo>> GetDetailInfoAsync(int id, DetailInfoType type);
        Task<ISTrainingPartResponse<bool>> AddOrUpdateAsync(DetailInfo detailInfo, DetailInfoType type);
    }
}
