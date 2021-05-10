using ISTraining_Part.Client.Delegates;
using ISTraining_Part.Client.Interfaces;
using ISTraining_Part.Core;
using ISTraining_Part.Core.Models;
using ISTraining_Part.Core.ServerEvents;
using Microsoft.AspNet.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ISTraining_Part.Client.Classes
{

    class Groups : Invoker, IGroups
    {

        public Groups(IHubConfigurator hubConfigurator) : base(hubConfigurator, HubNames.GroupsHub)
        {
            Proxy.On<DbChangeStatus, Group>(nameof(IGroupsHubEvents.OnChanged),
                (status, group) => OnChanged?.Invoke(status, group));

            Proxy.On(nameof(IGroupsHubEvents.GroupsImport), () => Imported?.Invoke());
        }


        public event OnChanged<Group> OnChanged;


        public event Action Imported;

        #region Get region

        public Task<ISTrainingPartResponse<IEnumerable<Group>>> GetGroupsAsync(int divisionId = -1)
        {
            Logger.Log.Info($"Получение списка групп: {{division: {divisionId}}}");

            return TryInvokeAsync(args: divisionId, defaultValue: Enumerable.Empty<Group>());
        }
        #endregion

        #region CUD region

        public Task<ISTrainingPartResponse<bool>> AddGroupsAsync(IEnumerable<Group> groups)
        {
            Logger.Log.Info($"Добавление групп: {{name: {string.Join(",", groups)}}}");

            return TryInvokeAsync<bool>(args: groups);
        }


        public Task<ISTrainingPartResponse<bool>> AddGroupAsync(Group group)
        {
            Logger.Log.Info($"Добавление группы: {{id: {group.Id}}}");

            return TryInvokeAsync<bool>(args: group);
        }


        public Task<ISTrainingPartResponse<bool>> SaveGroupAsync(Group group)
        {
            Logger.Log.Info($"Сохранение группы: {{id: {group.Id}}}");

            return TryInvokeAsync<bool>(args: group);
        }


        public Task<ISTrainingPartResponse<bool>> RemoveGroupAsync(Group group)
        {
            Logger.Log.Info($"Удаление группы: {{id: {group.Id}}}");

            return TryInvokeAsync<bool>(args: group);
        }
        #endregion
    }
}
