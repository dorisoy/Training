using ISTraining_Part.Core;
using ISTraining_Part.Core.Models;
using ISTraining_Part.Core.ServerEvents;
using ISTraining_Part.Core.ServerMethods;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using Server.DataBase.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server.Hubs
{

    [AuthorizeUser]
    [HubName(HubNames.GroupsHub)]
    public class GroupsHub : Hub<IGroupsHubEvents>, IGroupsHub
    {

        readonly IGroupsRepository repository;


        public GroupsHub(IGroupsRepository repository)
        {
            this.repository = repository;
        }

        #region Get region

        public Task<ISTrainingPartResponse<IEnumerable<Group>>> GetGroupsAsync(int divisionId = -1)
        {
            return repository.GetGroupsAsync(divisionId);
        }
        #endregion

        #region CUD region

        public async Task<ISTrainingPartResponse<bool>> AddGroupsAsync(IEnumerable<Group> groups)
        {
            Logger.Log.Info($"Import groups: {groups.Count()}");

            var res = await repository.AddGroupsAsync(groups);

            if (res)
                Clients.Group(Consts.AuthorizedGroup).GroupsImport();

            return res;
        }


        public async Task<ISTrainingPartResponse<bool>> AddGroupAsync(Group group)
        {
            Logger.Log.Info($"Add group: {group.Name}");

            var res = await repository.AddGroupAsync(group);

            if (res)
                Clients.Group(Consts.AuthorizedGroup).OnChanged(DbChangeStatus.Add, group);

            return res;
        }


        public async Task<ISTrainingPartResponse<bool>> SaveGroupAsync(Group group)
        {
            Logger.Log.Info($"Save group: {group.Name}");

            var res = await repository.SaveGroupAsync(group);

            if (res)
                Clients.Group(Consts.AuthorizedGroup).OnChanged(DbChangeStatus.Update, group);

            return res;
        }

        public async Task<ISTrainingPartResponse<bool>> RemoveGroupAsync(Group group)
        {
            Logger.Log.Info($"Remove group: {group.Name}");

            var res = await repository.RemoveGroupAsync(group);

            if (res)
                Clients.Group(Consts.AuthorizedGroup).OnChanged(DbChangeStatus.Remove, group);

            return res;
        }
        #endregion
    }
}
