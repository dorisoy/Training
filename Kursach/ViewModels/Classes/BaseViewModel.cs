using DevExpress.Mvvm;
using DryIoc;
using ISTraining_Part.Client.Interfaces;
using ISTraining_Part.Dialogs.Manager;
using ISTraining_Part.Providers;
using ISTraining_Part.ViewModels.Interfaces;
using MaterialDesignThemes.Wpf;
using MaterialDesignXaml.DialogsHelper;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ISTraining_Part.ViewModels.Classes
{
    abstract class BaseViewModel<T> : NavigationViewModel, IBaseViewModel<T>
    {

        public ObservableCollection<T> Items { get; set; }


        public T SelectedItem { get; set; }


        protected readonly IDialogIdentifier dialogIdentifier;


        protected readonly IDialogManager dialogManager;

        protected readonly ISnackbarMessageQueue snackbarMessageQueue;

        protected readonly IClient client;

        protected readonly IDataProvider dataProvider;


        public BaseViewModel()
        {
        }


        public BaseViewModel(IDialogManager dialogManager,
                             ISnackbarMessageQueue snackbarMessageQueue,
                             IClient client,
                             IDataProvider dataProvider,
                             IContainer container)
        {
            dialogIdentifier = container.ResolveRootDialogIdentifier();
            this.dialogManager = dialogManager;
            this.snackbarMessageQueue = snackbarMessageQueue;
            this.client = client;
            this.dataProvider = dataProvider;

            AddCommand = new DelegateCommand(Add);
            EditCommand = new AsyncCommand<T>(Edit, item => item != null);
            DeleteCommand = new AsyncCommand<T>(Delete, item => item != null);
        }


        public ICommand AddCommand { get; }
        public ICommand<T> EditCommand { get; }
        public ICommand<T> DeleteCommand { get; }
        public abstract void Add();
        public abstract Task Edit(T obj);
        public abstract Task Delete(T obj);
    }
}
