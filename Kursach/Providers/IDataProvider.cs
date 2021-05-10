using ISTraining_Part.Core.Models;
using ISTraining_Part.Models;
using System.Collections.ObjectModel;

namespace ISTraining_Part.Providers
{

    interface IDataProvider
    {

        ObservableCollection<User> Users { get; }


        ObservableCollection<Staff> Staff { get; }


        ObservableCollection<Group> Groups { get; }


        ObservableCollection<ChatMessage> ChatMessages { get; }

        void Load(UserMode mode);


        void Clear();
    }
}
