using DevExpress.Mvvm;
using DryIoc;
using ISTraining_Part.Client.Design;
using ISTraining_Part.Client.Interfaces;
using ISTraining_Part.Core.Models;
using ISTraining_Part.Core.Models.Enums;
using ISTraining_Part.Core.ServerEvents;
using ISTraining_Part.Dialogs.Manager;
using ISTraining_Part.Providers;
using ISTraining_Part.ViewModels.Classes;
using ISTraining_Part.ViewModels.Interfaces;
using MaterialDesignThemes.Wpf;
using MaterialDesignXaml.DialogsHelper;
using MaterialDesignXaml.DialogsHelper.Enums;
using Prism.Regions;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ISTraining_Part.ViewModels
{

    class StudentsViewModel : BaseViewModel<Student>, IDetail
    {

        Group selectedGroup;


        IRegionNavigationJournal journal;


        readonly TaskFactory sync;


        public StudentsViewModel()
        {
            Items = new ObservableCollection<Student>();
            var res = new DesignStudents().GetStudentsAsync(0).Result;
            Items.AddRange(res.Response);
        }

        public StudentsViewModel(IDialogManager dialogManager,
                                 ISnackbarMessageQueue snackbarMessageQueue,
                                 IClient client,
                                 IDataProvider dataProvider,
                                 IContainer container,
                                 TaskFactory sync)
            : base(dialogManager, snackbarMessageQueue, client, dataProvider, container)
        {
            this.sync = sync;

            Items = new ObservableCollection<Student>();

            client.Students.OnChanged += Students_OnChanged;
            client.Students.Imported += Load;

            client.Groups.OnChanged += Groups_OnChanged;

            GoBackCommand = new DelegateCommand(GoBack);
            ShowDetailInfoCommand = new DelegateCommand<People>(ShowDetailInfo, s => s != null);
            ShowDetailInfoEditorCommand = new DelegateCommand<People>(ShowDetailInfoEditor, s => s != null);
        }


        public ICommand GoBackCommand { get; }


        public ICommand<People> ShowDetailInfoCommand { get; }


        public ICommand<People> ShowDetailInfoEditorCommand { get; }

        public override async void Add()
        {
            var editor = await dialogManager.StudentEditor(null, false, selectedGroup?.Id ?? 0);
            if (editor == null)
                return;

            var res = await client.Students.AddStudentAsync(editor);
            var msg = res ? "Студент добавлен" : res;

            Log(msg, editor);
        }

        public override async Task Edit(Student student)
        {
            var editor = await dialogManager.StudentEditor(student, true, student.GroupId);
            if (editor == null)
                return;

            var res = await client.Students.SaveStudentAsync(editor);
            var msg = res ? "Студент сохранен" : res;

            Log(msg, student);
        }


        public override async Task Delete(Student student)
        {
            var answ = await dialogIdentifier.ShowMessageBoxAsync($"Удалить студента '{student.FullName}'?", MaterialMessageBoxButtons.YesNo);
            if (answ != MaterialMessageBoxButtons.Yes)
                return;

            var res = await client.Students.RemoveStudentAsync(student);
            var msg = res ? "Студент удален" : res;

            Log(msg, student);
        }


        private async void ShowDetailInfoEditor(People student)
        {
            var editor = await dialogManager.ShowDetailInfoEditor(student.Id, DetailInfoType.Student);
            if (editor == null)
                return;

            var res = await client.DetailInfo.AddOrUpdateAsync(editor, DetailInfoType.Student);
            var msg = res ? "Детальная информация сохранена" : res;

            snackbarMessageQueue.Enqueue(msg);
        }


        private void ShowDetailInfo(People student)
        {
            dialogManager.ShowDetailInfo(student.Id, DetailInfoType.Student);
        }

        private void GoBack()
        {
            journal.GoBack();
        }


        public override void OnNavigatedTo(NavigationContext navigationContext)
        {
            base.OnNavigatedTo(navigationContext);

            journal = navigationContext.NavigationService.Journal;

            selectedGroup = navigationContext.Parameters["group"] as Group;

            Load();
        }


        private async void Load()
        {
            await sync.StartNew(() => Items.Clear());

            var res = await client.Students.GetStudentsAsync(selectedGroup.Id);

            if (res)
                await sync.StartNew(() => Items.AddRange(res.Response));
        }


        private void Students_OnChanged(DbChangeStatus status, Student student)
        {
            int index = Items.IndexOf(student);
            if (index > -1 && student.GroupId != Items[index].GroupId)
            {
                sync.StartNew(() => Items.Remove(student));
            }

            if (student.GroupId != selectedGroup.Id)
                return;

            ProcessChangesHelper.ProcessChanges(status, student, Items, sync);
        }


        private void Groups_OnChanged(DbChangeStatus status, Group group)
        {
            if (status != DbChangeStatus.Remove)
                return;

            if (selectedGroup.Id == group.Id)
            {
                sync.StartNew(() => GoBack());
            }
        }

        void Log(string msg, Student student)
        {
            Logger.Log.Info($"{msg}: {{id: {student.Id}}}");
            snackbarMessageQueue.Enqueue(msg);
        }
    }
}
