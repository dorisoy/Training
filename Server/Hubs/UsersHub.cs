using ISTraining_Part.Core;
using ISTraining_Part.Core.Models;
using ISTraining_Part.Core.ServerEvents;
using ISTraining_Part.Core.ServerMethods;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using Server.DataBase.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Server.Hubs
{

    [AuthorizeUser]
    [AdminHub]
    [HubName(HubNames.UsersHub)]
    public class UsersHub : Hub<IUsersHubEvents>, IUsersHub
    {

        readonly IUsersRepository repository;


        public UsersHub(IUsersRepository repository)
        {
            this.repository = repository;
        }

        #region Get region

        public Task<ISTrainingPartResponse<IEnumerable<User>>> GetUsersAsync()
        {
            return repository.GetUsersAsync();
        }


        public Task<ISTrainingPartResponse<User>> GetUserAsync(string login, string password, bool usePassword)
        {
            return repository.GetUserAsync(login, password, usePassword);
        }
        #endregion

        #region Log region

        public Task<ISTrainingPartResponse<IEnumerable<SignInLog>>> GetSignInLogsAsync(int userId)
        {
            return repository.GetSignInLogsAsync(userId);
        }
        #endregion

        #region CUD region

        public async Task<ISTrainingPartResponse<bool>> AddUserAsync(User user)
        {
            Logger.Log.Info($"Add user: {user.Login}");

            var res = await repository.AddUserAsync(user);

            if (res)
                Clients.Group(Consts.AdminGroup).OnChanged(DbChangeStatus.Add, user);

            return res;
        }


        public async Task<ISTrainingPartResponse<bool>> SaveUserAsync(User user)
        {
            Logger.Log.Info($"Save user: {user.Login}");

            var res = await repository.SaveUserAsync(user);

            if (res)
                Clients.Group(Consts.AdminGroup).OnChanged(DbChangeStatus.Update, user);

            return res;
        }


        public async Task<ISTrainingPartResponse<bool>> RemoveUserAsync(User user)
        {
            Logger.Log.Info($"Remove user: {user.Login}");

            var res = await repository.RemoveUserAsync(user);

            if (res)
                Clients.Group(Consts.AdminGroup).OnChanged(DbChangeStatus.Remove, user);

            return res;
        }
        #endregion
    }
}
