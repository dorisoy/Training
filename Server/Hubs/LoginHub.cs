using ISTraining_Part.Core;
using ISTraining_Part.Core.Models;
using ISTraining_Part.Core.ServerMethods;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using Server.DataBase.Interfaces;
using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace Server.Hubs
{

    [HubName(HubNames.LoginHub)]
    public class LoginHub : Hub, ILoginHub
    {
        public static ConcurrentDictionary<string, User> Users { get; } = new ConcurrentDictionary<string, User>();

        readonly IUsersRepository usersRepository;

        public LoginHub(IUsersRepository usersRepository)
        {
            this.usersRepository = usersRepository;
        }


        /// <summary>
        /// 客户端登录
        /// </summary>
        /// <param name="login"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public async Task<ISTrainingPartResponse<User, LoginResponse>> LoginAsync(string login, string password)
        {
            var id = Context.ConnectionId;
            var res = await usersRepository.GetUserAsync(login, password, true);

            LoginResponse loginResponse = LoginResponse.Invalid;
            if (res && res.Response != null)
            {
                loginResponse = LoginResponse.Ok;

                if (res.Response.Mode == UserMode.Admin)
                    await HubHelper.AddToAdminGroup<UsersHub>(id);

                await HubHelper.AddToAuthorizedGroup<UsersHub>(id);
                await HubHelper.AddToAuthorizedGroup<StaffHub>(id);
                await HubHelper.AddToAuthorizedGroup<StudentsHub>(id);
                await HubHelper.AddToAuthorizedGroup<GroupsHub>(id);
                await HubHelper.AddToAuthorizedGroup<ChatHub>(id);

                usersRepository.AddSignInLogAsync(res.Response);

                Users[id] = res.Response;

                Console.WriteLine($"{Context.ConnectionId} logged in");
            }

            return new ISTrainingPartResponse<User, LoginResponse>(res.Code, loginResponse, res.Response);
        }


        /// <summary>
        /// 客户端连接时
        /// </summary>
        /// <returns></returns>
        public override Task OnConnected()
        {
            Console.WriteLine($"{Context.ConnectionId} connected");

            return base.OnConnected();
        }


        /// <summary>
        /// 客户端断开连接
        /// </summary>
        /// <param name="stopCalled"></param>
        /// <returns></returns>
        public override Task OnDisconnected(bool stopCalled)
        {
            var id = Context.ConnectionId;

            Users.TryRemove(id, out _);

            Console.WriteLine($"{id} disconnected");

            return base.OnDisconnected(stopCalled);
        }


        /// <summary>
        /// 客户端注销
        /// </summary>
        public async void Logout()
        {
            var id = Context.ConnectionId;

            Users.TryRemove(id, out _);

            Console.WriteLine($"{id} logged out");

            await HubHelper.RemoveFromAdminGroup<UsersHub>(id);
            await HubHelper.RemoveFromAuthorizedGroup<UsersHub>(id);
            await HubHelper.RemoveFromAuthorizedGroup<StaffHub>(id);
            await HubHelper.RemoveFromAuthorizedGroup<StudentsHub>(id);
            await HubHelper.RemoveFromAuthorizedGroup<GroupsHub>(id);
            await HubHelper.RemoveFromAuthorizedGroup<ChatHub>(id);
        }
    }
}
