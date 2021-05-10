using DevExpress.Mvvm;
using ISTraining_Part.Client.Interfaces;
using ISTraining_Part.Models;
using ISTraining_Part.Providers;
using System;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace ISTraining_Part.ViewModels
{

    class ChatViewModel : ViewModelBase
    {

        public string MessageText { get; set; }

        public ObservableCollection<ChatMessage> Messages { get; }

        readonly IClient client;


        public ChatViewModel()
        {
        }


        public ChatViewModel(IDataProvider dataProvider, IClient client)
        {
            Messages = dataProvider.ChatMessages;
            this.client = client;

            SendMessageCommand = new DelegateCommand(SendMessage);
        }


        public ICommand SendMessageCommand { get; }


        private void SendMessage()
        {
            if (MessageText.IsEmpty())
                return;

            client.Chat.SendMessage(MessageText);

            MessageText = "";
        }
    }
}
