using DryIoc;
using ISTraining_Part.Core.Models;
using ISTraining_Part.Core.Models.Enums;
using ISTraining_Part.Views;
using MaterialDesignXaml.DialogsHelper;
using Microsoft.Win32;
using System.Threading.Tasks;

namespace ISTraining_Part.Dialogs.Manager
{

    class DialogManager : IDialogManager
    {

        readonly IDialogIdentifier dialogIdentifier;

        readonly IContainer container;


        readonly IDialogsFactoryView viewFactory;

        public DialogManager(IContainer container, IDialogsFactoryView viewFactory)
        {
            this.dialogIdentifier = container.ResolveRootDialogIdentifier();
            this.container = container;
            this.viewFactory = viewFactory;
        }

        bool chatWindowOpened = false;
        ChatWindow currentChatWindow;

    
        public void CloseChatWindow()
        {
            if (!chatWindowOpened)
                return;

            currentChatWindow.Close();
        }


        public void ShowChatWindow()
        {
            if (chatWindowOpened)
                return;

            currentChatWindow = container.Resolve<ChatWindow>();
            currentChatWindow.Closed += WindowClosed;
            currentChatWindow.Show();
            chatWindowOpened = true;
        }

        void WindowClosed(object sender, object e)
        {
            chatWindowOpened = false;
            currentChatWindow.Closed -= WindowClosed;
        }


        public string SelectImportFile()
        {
            OpenFileDialog ofd = new OpenFileDialog
            {
                Title = "选择要导入的组文件",
                Filter = "Excel files|*.xlsx;*xls"
            };

            if (ofd.ShowDialog() == true)
                return ofd.FileName;
            else return null;
        }

        public string SelectExportFileName(string defName)
        {
            SaveFileDialog sfd = new SaveFileDialog
            {
                Title = "输入要保存的文件名",
                Filter = "Excel files|*.xlsx;*xls",
                FileName = defName
            };

            if (sfd.ShowDialog() == true)
                return sfd.FileName;
            else return null;
        }

        async Task<T> show<T, VM>(object[] args = null, IDialogIdentifier dialogIdentifier = null)
        {
            var vm = container.Resolve<VM>(args: args);
            var view = viewFactory.GetView(vm);

            dialogIdentifier = dialogIdentifier ?? this.dialogIdentifier;

            Logger.Log.Info($"Попытка показать диалог: {{view: {view}, viewmodel: {typeof(VM)}}}");

            var res = await dialogIdentifier.ShowAsync<T>(view);

            return res;
        }


        public Task<DetailInfo> ShowDetailInfoEditor(int id, DetailInfoType type)
        {
            return show<DetailInfo, DetailInfoEditorViewModel>(new object[] { id, type });
        }


        public Task ShowDetailInfo(int id, DetailInfoType type)
        {
            return show<DetailInfo, DetailInfoViewModel>(new object[] { id, type });
        }


        public Task<Student> StudentEditor(Student student, bool isEditMode, int groupId)
        {
            return show<Student, StudentEditorViewModel>(new object[] { student, isEditMode, groupId });
        }


        public Task<Staff> StaffEditor(Staff staff, bool isEditMode)
        {
            return show<Staff, StaffEditorViewModel>(new object[] { staff, isEditMode });
        }


        public Task<Staff> SelectStaff(int currentId, IDialogIdentifier dialogIdentifier)
        {
            return show<Staff, SelectStaffViewModel>(new object[] { currentId, dialogIdentifier }, dialogIdentifier);
        }


        public Task<Group> GroupEditor(Group group, bool isEditMode, int division)
        {
            return show<Group, GroupEditorViewModel>(new object[] { group, isEditMode, division });
        }


        public async void ShowLogs(User user)
        {
            await show<string, SignInLogsViewModel>(new object[] { user });
        }


        public Task<User> SignUp(User user, bool isEditMode)
        {
            return show<User, SignUpViewModel>(new object[] { user, isEditMode });
        }
    }
}
