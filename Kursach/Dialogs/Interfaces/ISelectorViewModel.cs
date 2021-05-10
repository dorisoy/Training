using System.Collections.ObjectModel;

namespace ISTraining_Part.Dialogs.Interfaces
{

    interface ISelectorViewModel<T>
    {

        ObservableCollection<T> Items { get; }

        T SelectedItem { get; set; }
    }
}
