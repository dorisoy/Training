using DevExpress.Mvvm;
using DryIoc;
using ISTraining_Part.Core.ViewModels;
using ISTraining_Part.Dialogs.Interfaces;
using MaterialDesignXaml.DialogsHelper;
using System;
using System.Windows.Input;

namespace ISTraining_Part.Dialogs.Classes
{
    /// <summary>
    /// Base edit mode view model.
    /// </summary>
    class BaseEditModeViewModel<T> : ViewModelBase, IClosableDialog, IDialogIdentifier, IEditMode<T> where T : ValidateViewModel, ICloneable
    {
        /// <summary>
        /// Identifier.
        /// </summary>
        public string Identifier => nameof(BaseEditModeViewModel<T>);

        /// <summary>
        /// Owner.
        /// </summary>
        public IDialogIdentifier OwnerIdentifier { get; }


        public T EditableObject { get; }


        public bool IsEditMode { get; }

        public BaseEditModeViewModel()
        {
            EditableObject = Activator.CreateInstance<T>();
        }


        public BaseEditModeViewModel(T obj, bool isEditMode, IContainer container)
        {
            IsEditMode = isEditMode;
            OwnerIdentifier = container.ResolveRootDialogIdentifier();

            if (isEditMode)
                EditableObject = (T)obj.Clone();

            else EditableObject = container.Resolve<T>();

            CloseDialogCommand = new DelegateCommand(CloseDialog, IsObjectValid);
        }


        private bool IsObjectValid() => EditableObject.IsValid;

        public ICommand CloseDialogCommand { get; }


        private void CloseDialog()
        {
            OwnerIdentifier.Close(EditableObject);
        }
    }
}
