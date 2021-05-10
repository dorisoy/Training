using DevExpress.Mvvm;
using DryIoc;
using ISTraining_Part.Client.Interfaces;
using ISTraining_Part.Core;
using ISTraining_Part.Core.Models;
using ISTraining_Part.Properties;
using ISTraining_Part.Providers;
using ISTraining_Part.ViewModels.Classes;
using MaterialDesignXaml.DialogsHelper;
using MaterialDesignXaml.DialogsHelper.Enums;
using Prism.Regions;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ISTraining_Part.ViewModels
{

    class LoginViewModel : NavigationViewModel
    {

        public LoginUser LoginUser { get; }


        readonly IRegionManager regionManager;

        readonly IDialogIdentifier dialogIdentifier;


        readonly IClient client;


        readonly IDataProvider dataProvider;


        public LoginViewModel(IRegionManager regionManager,
                              IClient client,
                              IDataProvider dataProvider,
                              IContainer container)
        {
            this.regionManager = regionManager;
            this.dialogIdentifier = container.ResolveRootDialogIdentifier();
            this.client = client;
            this.dataProvider = dataProvider;

            TryLoginCommand = new AsyncCommand(TryLogin, IsUserValid);

            LoginUser = new LoginUser
            {
                Login = Settings.Default.lastLogin,
                Password = Settings.Default.lastPassword
            };
        }


        private bool IsUserValid() => LoginUser.IsValid;

        public ICommand TryLoginCommand { get; }


        private async Task TryLogin()
        {
            if (!LoginUser.IsValid)
            {
                await dialogIdentifier.ShowMessageBoxAsync(LoginUser.Error, MaterialMessageBoxButtons.Ok);
                return;
            }

            Settings.Default.lastLogin = LoginUser.Login;
            Settings.Default.lastPassword = LoginUser.Password;

            var res = await client.Login.LoginAsync(LoginUser.Login, LoginUser.Password);
            User user;
            if (res && res.Arg == LoginResponse.Ok)
                user = res.Response;
            else
            {
                string msg;
                switch (res.Arg)
                {
                    case LoginResponse.Ok:
                        msg = "登录成功！";
                        break;

                    case LoginResponse.Invalid:
                        msg = "错误的登录或密码";
                        break;

                    case LoginResponse.ServerError:
                        msg = "服务器错误";
                        break;

                    default:
                        msg = "未知错误...";
                        break;
                }

                if (res.Code != ISTrainingPartResponseCode.Ok)
                    msg = res;

                await dialogIdentifier.ShowMessageBoxAsync(msg, MaterialMessageBoxButtons.Ok);
                return;
            }

            if (res && res.Arg == LoginResponse.Ok)
            {
                Consts.LoginStatus = true;

                Logger.Log.Info($"成功登录: {{login: {user.Login}, mode: {user.Mode}}}");

                var parameters = NavigationParametersFluent.GetNavigationParameters().SetUser(user).SetValue("fromLogin", true);
                regionManager.RequestNavigateInRootRegion(RegionViews.MainView, parameters);
                regionManager.ReqeustNavigateInMainRegion(RegionViews.GroupsView, parameters);
            }
        }

        public override void OnNavigatedTo(NavigationContext navigationContext)
        {
            dataProvider.Clear();

            if (navigationContext.Parameters.ContainsKey("fromConnecting"))
            {
                if (Consts.LoginStatus)
                    TryLoginCommand.Execute(null);
            }
        }
    }
}
