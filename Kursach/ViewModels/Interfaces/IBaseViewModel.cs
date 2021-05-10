using DevExpress.Mvvm;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ISTraining_Part.ViewModels.Interfaces
{

    interface IBaseViewModel<T>
    {

        ObservableCollection<T> Items { get; set; }

        T SelectedItem { get; set; }


        ICommand AddCommand { get; }


        ICommand<T> EditCommand { get; }

  
        ICommand<T> DeleteCommand { get; }


        void Add();


        Task Edit(T obj);


        Task Delete(T obj);
    }
}
