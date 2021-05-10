﻿using DevExpress.Mvvm;
using DryIoc;
using ISTraining_Part.Client.Design;
using ISTraining_Part.Client.Interfaces;
using ISTraining_Part.Core.Models;
using ISTraining_Part.Dialogs.Manager;
using ISTraining_Part.Providers;
using ISTraining_Part.ViewModels.Classes;
using MaterialDesignThemes.Wpf;
using MaterialDesignXaml.DialogsHelper;
using MaterialDesignXaml.DialogsHelper.Enums;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace ISTraining_Part.ViewModels
{

    class UsersViewModel : BaseViewModel<User>
    {

        public UsersViewModel()
        {
            Items = new ObservableCollection<User>();
            var res = new DesignUsers().GetUsersAsync().Result;
            Items.AddRange(res.Response);
        }

        public UsersViewModel(IDialogManager dialogManager,
                              ISnackbarMessageQueue snackbarMessageQueue,
                              IClient client,
                              IDataProvider dataProvider,
                              IContainer container)
            : base(dialogManager, snackbarMessageQueue, client, dataProvider, container)
        {
            Items = dataProvider.Users;

            ShowLogsCommand = new DelegateCommand<User>(ShowLogs, u => u != null);
        }


        public ICommand<User> ShowLogsCommand { get; }


        public override async void Add()
        {
            var editor = await dialogManager.SignUp(null, false);
            if (editor == null)
                return;

            var res = await client.Users.AddUserAsync(editor);
            var msg = res ? "Пользователь добавлен" : res;

            Log(msg, editor);
        }


        public override async Task Edit(User user)
        {
            var editor = await dialogManager.SignUp(user, true);
            if (editor == null)
                return;

            var res = await client.Users.SaveUserAsync(editor);
            var msg = res ? "Пользователь сохранен" : res;

            Log(msg, user);
        }


        public override async Task Delete(User user)
        {
            var answ = await dialogIdentifier.ShowMessageBoxAsync($"Удалить '{user.Login}'?", MaterialMessageBoxButtons.YesNo);
            if (answ != MaterialMessageBoxButtons.Yes)
                return;

            var res = await client.Users.RemoveUserAsync(user);
            var msg = res ? "Пользователь удален" : res;

            Log(msg, user);
        }


        private void ShowLogs(User user)
        {
            dialogManager.ShowLogs(user);
        }


        void Log(string msg, User user)
        {
            Logger.Log.Info($"{msg}: {{id: {user.Id}}}");
            snackbarMessageQueue.Enqueue(msg);
        }
    }
}
