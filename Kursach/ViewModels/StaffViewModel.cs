﻿using DevExpress.Mvvm;
using DryIoc;
using ISTraining_Part.Client.Design;
using ISTraining_Part.Client.Interfaces;
using ISTraining_Part.Core.Models;
using ISTraining_Part.Core.Models.Enums;
using ISTraining_Part.Dialogs.Manager;
using ISTraining_Part.Excel.Interfaces;
using ISTraining_Part.Providers;
using ISTraining_Part.ViewModels.Classes;
using ISTraining_Part.ViewModels.Interfaces;
using MaterialDesignThemes.Wpf;
using MaterialDesignXaml.DialogsHelper;
using MaterialDesignXaml.DialogsHelper.Enums;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ISTraining_Part.ViewModels
{

    class StaffViewModel : BaseViewModel<Staff>, IDetail
    {

        readonly IExporter<IEnumerable<Staff>> exporter;


        public StaffViewModel()
        {
            Items = new ObservableCollection<Staff>();
            var res = new DesignStaff().GetStaffsAsync().Result;
            Items.AddRange(res.Response);
        }

        public StaffViewModel(IExporter<IEnumerable<Staff>> exporter,
                              IDialogManager dialogManager,
                              ISnackbarMessageQueue snackbarMessageQueue,
                              IClient client,
                              IDataProvider dataProvider,
                              IContainer container)
            : base(dialogManager, snackbarMessageQueue, client, dataProvider, container)
        {
            this.exporter = exporter;

            Items = dataProvider.Staff;

            ExportToExcelCommand = new DelegateCommand(ExportToExcel);

            ShowDetailInfoCommand = new DelegateCommand<People>(ShowDetailInfo, s => s != null);
            ShowDetailInfoEditorCommand = new DelegateCommand<People>(ShowDetailInfoEditor, s => s != null);
        }


        public ICommand ExportToExcelCommand { get; }


        public ICommand<People> ShowDetailInfoCommand { get; }


        public ICommand<People> ShowDetailInfoEditorCommand { get; }

        public override async void Add()
        {
            var editor = await dialogManager.StaffEditor(null, false);
            if (editor == null)
                return;

            var res = await client.Staff.AddStaffAsync(editor);
            var msg = res ? "Сотрудник добавлен" : res;

            Log(msg, editor);
        }


        public override async Task Edit(Staff staff)
        {
            var editor = await dialogManager.StaffEditor(staff, true);
            if (editor == null)
                return;

            var res = await client.Staff.SaveStaffAsync(editor);
            var msg = res ? "Сотрудник сохранен" : res;

            Log(msg, staff);
        }


        public override async Task Delete(Staff staff)
        {
            var answ = await dialogIdentifier.ShowMessageBoxAsync($"Удалить '{staff.FullName}'?", MaterialMessageBoxButtons.YesNo);
            if (answ != MaterialMessageBoxButtons.Yes)
                return;

            var res = await client.Staff.RemoveStaffAsync(staff);
            var msg = res ? "Сотрудник удален" : res;

            Log(msg, staff);
        }


    
        private async void ShowDetailInfoEditor(People staff)
        {
            var editor = await dialogManager.ShowDetailInfoEditor(staff.Id, DetailInfoType.Staff);
            if (editor == null)
                return;

            var res = await client.DetailInfo.AddOrUpdateAsync(editor, DetailInfoType.Staff);
            var msg = res ? "Детальная информация сохранена" : res;

            snackbarMessageQueue.Enqueue(msg);
        }

 
        private void ShowDetailInfo(People staff)
        {
            dialogManager.ShowDetailInfo(staff.Id, DetailInfoType.Staff);
        }

        public void ExportToExcel()
        {
            var res = exporter.Export(Items);
            var msg = res ? "Сотрудники экспортированы" : "Сотрудники не экспортированы";

            snackbarMessageQueue.Enqueue(msg);
        }

        void Log(string msg, Staff staff)
        {
            Logger.Log.Info($"{msg}: {{id: {staff.Id}}}");
            snackbarMessageQueue.Enqueue(msg);
        }
    }
}
