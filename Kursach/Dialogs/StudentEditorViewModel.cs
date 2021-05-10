using DryIoc;
using ISTraining_Part.Core.Models;
using ISTraining_Part.Dialogs.Attributes;
using ISTraining_Part.Dialogs.Classes;
using ISTraining_Part.Providers;
using System.Linq;
using System.Windows.Data;

namespace ISTraining_Part.Dialogs
{
    /// <summary>
    /// Student editor view model.
    /// </summary>
    [DialogName(nameof(StudentEditorView))]
    class StudentEditorViewModel : BaseEditModeViewModel<Student>
    {

        public ListCollectionView Groups { get; }

        Group selectedGroup;

        public Group SelectedGroup
        {
            get
            {
                return selectedGroup;
            }
            set
            {
                selectedGroup = value;
                EditableObject.GroupId = value?.Id ?? -1;

                RaisePropertyChanged(nameof(SelectedGroup));
            }
        }

        public StudentEditorViewModel()
        {
        }


        public StudentEditorViewModel(Student student,
                                      bool isEditMode,
                                      int groupId,
                                      IDataProvider dataProvider,
                                      IContainer container)
            : base(student, isEditMode, container)
        {
            Groups = new ListCollectionView(dataProvider.Groups);
            Groups.GroupDescriptions.Add(new PropertyGroupDescription(nameof(Group.Division)));

            SelectedGroup = dataProvider.Groups.FirstOrDefault(x => x.Id == groupId);
        }
    }
}
