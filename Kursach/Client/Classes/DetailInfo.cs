using ISTraining_Part.Client.Interfaces;
using ISTraining_Part.Core;
using ISTraining_Part.Core.Models.Enums;
using System.Threading.Tasks;

namespace ISTraining_Part.Client.Classes
{

    class DetailInfo : Invoker, IDetailInfo
    {

        public DetailInfo(IHubConfigurator hubConfigurator) : base(hubConfigurator, HubNames.DetailInfoHub)
        {

        }


        public Task<ISTrainingPartResponse<Core.Models.DetailInfo>> GetDetailInfoAsync(int id, DetailInfoType type)
        {
            Logger.Log.Info($"Получение детальной информации: {{people id: {id}, type: {type}}}");

            return TryInvokeAsync(args: new object[] { id, type }, defaultValue: new Core.Models.DetailInfo());
        }


        public Task<ISTrainingPartResponse<bool>> AddOrUpdateAsync(Core.Models.DetailInfo detailInfo, DetailInfoType type)
        {
            Logger.Log.Info($"Сохранение детальной информации: {{id: {detailInfo.Id}, {detailInfo}, type: {type}}}");

            return TryInvokeAsync<bool>(args: new object[] { detailInfo, type });
        }
    }
}
