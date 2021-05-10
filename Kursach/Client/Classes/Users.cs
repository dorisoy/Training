using ISTraining_Part.Client.Delegates;
using ISTraining_Part.Client.Interfaces;
using ISTraining_Part.Core;
using ISTraining_Part.Core.Models;
using ISTraining_Part.Core.ServerEvents;
using Microsoft.AspNet.SignalR.Client;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ISTraining_Part.Client.Classes
{

    class Users : Invoker, IUsers
    {

        public Users(IHubConfigurator hubConfigurator) : base(hubConfigurator, HubNames.UsersHub)
        {
            Proxy.On<DbChangeStatus, User>(nameof(IUsersHubEvents.OnChanged),
                (status, user) => OnChanged?.Invoke(status, user));
        }


        public event OnChanged<User> OnChanged;

        #region Get region

        public Task<ISTrainingPartResponse<IEnumerable<User>>> GetUsersAsync()
        {
            Logger.Log.Info("Получение списка пользователей");

            return TryInvokeAsync(defaultValue: Enumerable.Empty<User>());
        }


        public Task<ISTrainingPartResponse<User>> GetUserAsync(string login, string password, bool usePassword)
        {
            Logger.Log.Info($"Получение пользователя: {{login: {login}, password: {usePassword}}}");

            return TryInvokeAsync<User>(args: new object[] { login, password, usePassword });
        }
        #endregion

        #region Log region

        public Task<ISTrainingPartResponse<IEnumerable<SignInLog>>> GetSignInLogsAsync(int userId)
        {
            Logger.Log.Info($"Получение логов входа пользователя: {{id: {userId}}}");

            return TryInvokeAsync(args: userId, defaultValue: Enumerable.Empty<SignInLog>());
        }
        #endregion

        #region CUD region

        public Task<ISTrainingPartResponse<bool>> AddUserAsync(User user)
        {
            Logger.Log.Info($"Добавление пользователя: {{id: {user.Id}}}");

            return TryInvokeAsync<bool>(args: user);
        }


        public Task<ISTrainingPartResponse<bool>> SaveUserAsync(User user)
        {
            Logger.Log.Info($"Сохранение пользователя: {{id: {user.Id}}}");

            return TryInvokeAsync<bool>(args: user);
        }

        public Task<ISTrainingPartResponse<bool>> RemoveUserAsync(User user)
        {
            Logger.Log.Info($"Удаление пользователя: {{id: {user.Id}}}");

            return TryInvokeAsync<bool>(args: user);
        }
        #endregion
    }
}
