using ISTraining_Part.Client.Interfaces;
using ISTraining_Part.Core;
using ISTraining_Part.Core.Models;
using System.Threading.Tasks;

namespace ISTraining_Part.Client.Classes
{

    class Login : Invoker, ILogin
    {

        public Login(IHubConfigurator hubConfigurator) : base(hubConfigurator, HubNames.LoginHub)
        {
        }


        public Task<ISTrainingPartResponse<User, LoginResponse>> LoginAsync(string login, string password)
        {
            Logger.Log.Info($"Попытка авторизации: {{login: {login}}}");

            return TryInvokeAsync<User, LoginResponse>(argDefault: LoginResponse.ServerError, args: new object[] { login, password });
        }

        public void Logout()
        {
            TryInvokeAsync();
        }
    }
}
