using ISTraining_Part.Client.Interfaces;
using ISTraining_Part.Core.Models;
using ISTraining_Part.Core.ServerEvents;
using ISTraining_Part.Models;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace ISTraining_Part.Providers
{

    class DataProvider : IDataProvider
    {

        public ObservableCollection<User> Users { get; }


        public ObservableCollection<Staff> Staff { get; }


        public ObservableCollection<Group> Groups { get; }


        public ObservableCollection<ChatMessage> ChatMessages { get; }

   
        readonly IClient client;

        readonly TaskFactory sync;


        public DataProvider(IClient client, TaskFactory sync)
        {
            this.client = client;
            this.sync = sync;

            client.Users.OnChanged += Users_OnChanged;

            client.Staff.OnChanged += Staff_OnChanged;

            client.Groups.OnChanged += Groups_OnChanged;
            client.Groups.Imported += Groups_Imported;

            client.Chat.NewMessage += Chat_NewMessage;

            Users = new ObservableCollection<User>();
            Staff = new ObservableCollection<Staff>();
            Groups = new ObservableCollection<Group>();
            ChatMessages = new ObservableCollection<ChatMessage>();
        }


        private void Chat_NewMessage(string senderName, string text)
        {
            sync.StartNew(() => ChatMessages.Add(new ChatMessage(senderName, text)));
        }


        private async void Groups_Imported()
        {
            var group = await client.Groups.GetGroupsAsync();

            if (group)
            {
                await sync.StartNew(() =>
                {
                    Groups.Clear();
                    Groups.AddRange(group.Response);
                });
            }
        }

        public async void Load(UserMode mode)
        {
            if (mode == UserMode.Admin)
            {
                var users = await client.Users.GetUsersAsync();
                if (users)
                    await sync.StartNew(() => Users.AddRange(users.Response));
            }

            var staff = await client.Staff.GetStaffsAsync();

            if (staff)
                await sync.StartNew(() => Staff.AddRange(staff.Response));

            Groups_Imported();
        }


        public void Clear()
        {
            Users.Clear();
            Staff.Clear();
            Groups.Clear();
            ChatMessages.Clear();
        }


        private void Groups_OnChanged(DbChangeStatus status, Group arg)
        {
            ProcessChanges(status, arg, Groups);
        }


        private void Staff_OnChanged(DbChangeStatus status, Staff arg)
        {
            ProcessChanges(status, arg, Staff);
        }


        private void Users_OnChanged(DbChangeStatus status, User arg)
        {
            ProcessChanges(status, arg, Users);
        }

        void ProcessChanges<T>(DbChangeStatus status, T arg, ObservableCollection<T> collection)
        {
            ProcessChangesHelper.ProcessChanges(status, arg, collection, sync);

            if (status == DbChangeStatus.Remove && arg is Staff staff)
            {
                Clear(Groups, x => x.CuratorId == staff.Id);
            }
        }


        void Clear<T>(ObservableCollection<T> collection, Func<T, bool> func)
        {
            var removeList = collection.Where(func).ToList();
            foreach (var item in removeList)
            {
                collection.Remove(item);
            }
        }
    }
}
