using ISTraining_Part.Core;
using ISTraining_Part.Core.Models;
using ISTraining_Part.Core.Models.Enums;
using ISTraining_Part.Core.ServerMethods;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using Server.DataBase.Interfaces;
using System.Threading.Tasks;

namespace Server.Hubs
{

    [AuthorizeUser]
    [HubName(HubNames.DetailInfoHub)]
    public class DetailInfoHub : Hub, IDetailInfoHub
    {

        readonly IDetailInfoRepository repository;

        public DetailInfoHub(IDetailInfoRepository repository)
        {
            this.repository = repository;
        }


        public Task<ISTrainingPartResponse<DetailInfo>> GetDetailInfoAsync(int id, DetailInfoType type)
        {
            return repository.GetDetailInfoAsync(id, type);
        }


        public Task<ISTrainingPartResponse<bool>> AddOrUpdateAsync(DetailInfo detailInfo, DetailInfoType type)
        {
            Logger.Log.Info($"Save detail info: {{student: {detailInfo.Student}, staff: {detailInfo.Staff}}}");

            return repository.AddOrUpdateAsync(detailInfo, type);
        }
    }
}
