using DevExpress.Mvvm;
using DryIoc;
using ISTraining_Part.Client.Design;
using ISTraining_Part.Client.Interfaces;
using ISTraining_Part.Core.Models;
using ISTraining_Part.Dialogs.Manager;
using ISTraining_Part.Excel.Interfaces;
using ISTraining_Part.Providers;
using ISTraining_Part.ViewModels.Classes;
using MaterialDesignThemes.Wpf;
using MaterialDesignXaml.DialogsHelper;
using MaterialDesignXaml.DialogsHelper.Enums;
using Prism.Regions;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Input;

namespace ISTraining_Part.ViewModels
{
    /// <summary>
    /// Groups view model.
    /// </summary>
    class GroupsViewModel : BaseViewModel<Group>
    {

        public ListCollectionView Groups { get; }

        int selectedDivision = -1;

        public int SelectedDivision
        {
            get => selectedDivision;
            set
            {
                selectedDivision = value;
                Groups.Refresh();
            }
        }


        readonly IAsyncExporter<IEnumerable<Group>> divisionsContingentExporter;


        readonly IAsyncImporter<IEnumerable<Group>> divisionsContingentImporter;


        readonly IImporter<IEnumerable<Student>> studentsImporter;


        readonly IAsyncExporter<IEnumerable<IGrouping<Group, Student>>> studentsExporter;


        readonly IAsyncExporter<IEnumerable<IGrouping<Group, Student>>> minorStudentsExporter;


        readonly IRegionManager regionManager;

        public GroupsViewModel()
        {
            Items = new ObservableCollection<Group>();
            var res = new DesignGroups().GetGroupsAsync(1).Result;
            Items.AddRange(res.Response);

            Groups = new ListCollectionView(Items);
        }

        public GroupsViewModel(IDialogManager dialogManager,
                               IAsyncExporter<IEnumerable<Group>> divisionsContingentExporter,
                               IAsyncImporter<IEnumerable<Group>> divisionsContingentImporter,
                               IImporter<IEnumerable<Student>> studentsImporter,
                               ISnackbarMessageQueue snackbarMessageQueue,
                               IRegionManager regionManager,
                               IClient client,
                               IDataProvider dataProvider,
                               IContainer container)
            : base(dialogManager, snackbarMessageQueue, client, dataProvider, container)
        {
            this.divisionsContingentExporter = divisionsContingentExporter;
            this.divisionsContingentImporter = divisionsContingentImporter;
            this.studentsImporter = studentsImporter;
            this.studentsExporter = container.Resolve<IAsyncExporter<IEnumerable<IGrouping<Group, Student>>>>();
            this.minorStudentsExporter = container.Resolve<IAsyncExporter<IEnumerable<IGrouping<Group, Student>>>>("minor");
            this.regionManager = regionManager;

            Items = dataProvider.Groups;
            Groups = new ListCollectionView(Items);
            Groups.Filter += FilterGroup;

            DivisionsContingentExportCommand = new DelegateCommand(DivisionsContingentExport);
            StudentsExportCommand = new AsyncCommand(StudentsExport);
            MinorStudentsExportCommand = new AsyncCommand(MinorStudentsExport);

            DivisionsContingentImportCommand = new AsyncCommand(DivisionsContingentImport);
            StudentsImportCommand = new DelegateCommand(StudentsImport);

            ShowStudentsCommand = new DelegateCommand<Group>(ShowStudents, group => group != null);
        }


        private bool FilterGroup(object group)
        {
            var gr = (Group)group;

            return gr.Division == selectedDivision;
        }


        public ICommand DivisionsContingentExportCommand { get; }


        public ICommand DivisionsContingentImportCommand { get; }


        public ICommand StudentsImportCommand { get; }


        public ICommand StudentsExportCommand { get; }


        public ICommand MinorStudentsExportCommand { get; }


        public ICommand<Group> ShowStudentsCommand { get; }

        public override async void Add()
        {
            var editor = await dialogManager.GroupEditor(null, false, SelectedDivision == -1 ? 0 : SelectedDivision);
            if (editor == null)
                return;

            var res = await client.Groups.AddGroupAsync(editor);
            var msg = res ? "添加组" : res;

            Log(msg, editor);
        }


        public override async Task Edit(Group group)
        {
            var editor = await dialogManager.GroupEditor(group, true, group.Division);
            if (editor == null)
                return;

            var res = await client.Groups.SaveGroupAsync(editor);
            var msg = res ? "组保存" : res;

            Log(msg, group);
        }

        public override async Task Delete(Group group)
        {
            var answ = await dialogIdentifier.ShowMessageBoxAsync($"删除组 '{group.Name}'?", MaterialMessageBoxButtons.YesNo);
            if (answ != MaterialMessageBoxButtons.Yes)
                return;

            var res = await client.Groups.RemoveGroupAsync(group);
            var msg = res ? "删除组" : res;

            Log(msg, group);
        }

        private async void StudentsImport()
        {
            var students = studentsImporter.Import();

            if (students == null || students.Count() == 0)
                return;

            foreach (var item in students.Chunk(10))
            {
                var res = await client.Students.ImportStudentsAsync(item);

                if (!res)
                    return;
            }

            await client.Students.RaiseStudentsImported();

            snackbarMessageQueue.Enqueue($"添加学生: {students.Count()}");
        }


        private void ShowStudents(Group group)
        {
            var @params = NavigationParametersFluent.GetNavigationParameters()
                                                    .SetValue("group", group)
                                                    .SetUser(User);

            regionManager.ReqeustNavigateInMainRegion(RegionViews.StudentsView, @params);
        }


        public async void DivisionsContingentExport()
        {
            var groups = await client.Groups.GetGroupsAsync();
            if (!groups)
            {
                snackbarMessageQueue.Enqueue(groups);
                return;
            }

            var res = await divisionsContingentExporter.Export(groups.Response);
            var msg = res ? "导出组" : "不导出组";

            snackbarMessageQueue.Enqueue(msg);
        }


        public async Task DivisionsContingentImport()
        {
            var groups = await divisionsContingentImporter.Import();

            if (groups == null)
                return;

            var res = await client.Groups.AddGroupsAsync(groups);

            if (res)
                snackbarMessageQueue.Enqueue($"添加组: {groups.Count()}");
        }


        private async Task MinorStudentsExport()
        {
            var students = await GetStudents();
            var res = await minorStudentsExporter.Export(students);
            var msg = res ? "Несовершеннолетние экспортированы" : "Несовершеннолетние не экспортированы";
            snackbarMessageQueue.Enqueue(msg);
        }


        private async Task StudentsExport()
        {
            var students = await GetStudents();
            var res = await studentsExporter.Export(students);
            var msg = res ? "Студенты экспортированы" : "Студенты не экспортированы";
            snackbarMessageQueue.Enqueue(msg);
        }

        async Task<IEnumerable<IGrouping<Group, Student>>> GetStudents()
        {
            List<Student> students = new List<Student>();
            foreach (var item in Items)
            {
                var res = await client.Students.GetStudentsAsync(item.Id);
                if (!res)
                    return Enumerable.Empty<IGrouping<Group, Student>>();

                students.AddRange(res.Response);
            }

            return students.GroupBy(x => Items.FirstOrDefault(g => g.Id == x.GroupId));
        }


        void Log(string msg, Group group)
        {
            Logger.Log.Info($"{msg}: {{id: {group.Id}}}");
            snackbarMessageQueue.Enqueue(msg);
        }
    }
}
