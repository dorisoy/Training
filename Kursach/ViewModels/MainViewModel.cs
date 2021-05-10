using DevExpress.Mvvm;
using DryIoc;
using ISTraining_Part.Client.Interfaces;
using ISTraining_Part.Core.Models;
using ISTraining_Part.Dialogs.Manager;
using ISTraining_Part.Properties;
using ISTraining_Part.Providers;
using ISTraining_Part.UI;
using ISTraining_Part.ViewModels.Classes;
using MaterialDesignXaml.DialogsHelper;
using MaterialDesignXaml.DialogsHelper.Enums;
using Prism.Regions;
using System.Diagnostics;
using System.Windows.Input;

namespace ISTraining_Part.ViewModels
{
    class MainViewModel : NavigationViewModel
    {

        public int SlideNumber { get; set; }


        public bool LeftMenuOpened { get; set; }


        public bool IsDarkTheme
        {
            set => SetThemeColor.SetTheme(value);
            get => Settings.Default.isDarkTheme;
        }

        readonly IRegionManager regionManager;


        readonly IDialogIdentifier dialogIdentifier;


        readonly IClient client;

        readonly IDataProvider dataProvider;


        readonly IDialogManager dialogManager;


        public MainViewModel()
        {
            User = new User
            {
                Login = "DESIGN TIME USER",
                Name = "DESIGN TIME NAME",
                Mode = UserMode.Admin
            };
        }


        public MainViewModel(IRegionManager regionManager,
                             IClient client,
                             IDataProvider dataProvider,
                             IDialogManager dialogManager,
                             IContainer container)
        {
            this.regionManager = regionManager;
            dialogIdentifier = container.ResolveRootDialogIdentifier();
            this.client = client;
            this.dataProvider = dataProvider;
            this.dialogManager = dialogManager;

            NavigateCommand = new DelegateCommand<string>(Navigate);
            OpenChatWindowCommand = new DelegateCommand(OpenChatWindow);
            ExitCommand = new DelegateCommand(Exit);
            OpenVkCommand = new DelegateCommand(OpenVk);
        }


        public ICommand<string> NavigateCommand { get; }


        public ICommand OpenChatWindowCommand { get; }


        public ICommand ExitCommand { get; }


        public ICommand OpenVkCommand { get; }


        private void OpenVk()
        {
            Process.Start("https://vk.com/id99551920");
        }


        private void OpenChatWindow()
        {
            dialogManager.ShowChatWindow();
            LeftMenuOpened = false;
        }


        private async void Exit()
        {
            var res = await dialogIdentifier.ShowMessageBoxAsync("你确定要退出吗?", MaterialMessageBoxButtons.Yes | MaterialMessageBoxButtons.Cancel);
            if (res != MaterialMessageBoxButtons.Yes)
                return;

            LeftMenuOpened = false;
            SlideNumber = 0;
            Logger.Log.Info("Выход из приложения");
            client.Login.Logout();
            Consts.LoginStatus = false;
            dialogManager.CloseChatWindow();
            regionManager.RequestNavigateInRootRegion(RegionViews.LoginView);
        }


        private void Navigate(string view)
        {
            regionManager.RequstNavigateInMainRegionWithUser(view, User);
            LeftMenuOpened = false;
        }

        public override void OnNavigatedTo(NavigationContext navigationContext)
        {
            LeftMenuOpened = false;

            if (!navigationContext.Parameters.ContainsKey("fromLogin"))
                return;

            base.OnNavigatedTo(navigationContext);

            dataProvider.Load(User.Mode);
        }
    }
}
