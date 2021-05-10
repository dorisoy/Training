using ISTraining_Part.Core.ViewModels;

namespace ISTraining_Part.Dialogs.Interfaces
{

    interface IEditMode<T> where T : ValidateViewModel
    {
        bool IsEditMode { get; }
        T EditableObject { get; }
    }
}
