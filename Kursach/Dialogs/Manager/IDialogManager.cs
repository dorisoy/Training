using ISTraining_Part.Core.Models;
using ISTraining_Part.Core.Models.Enums;
using MaterialDesignXaml.DialogsHelper;
using System.Threading.Tasks;

namespace ISTraining_Part.Dialogs.Manager
{
 
    interface IDialogManager
    {

        void ShowChatWindow();


        void CloseChatWindow();


        string SelectExportFileName(string defName);


        string SelectImportFile();


        Task<DetailInfo> ShowDetailInfoEditor(int id, DetailInfoType type);


        Task ShowDetailInfo(int id, DetailInfoType type);


        void ShowLogs(User user);


        Task<User> SignUp(User user, bool isEditMode);


        Task<Group> GroupEditor(Group group, bool isEditMode, int division);


        Task<Staff> SelectStaff(int currentId, IDialogIdentifier dialogIdentifier);


        Task<Staff> StaffEditor(Staff staff, bool isEditMode);


        Task<Student> StudentEditor(Student student, bool isEditMode, int groupId);
    }
}
