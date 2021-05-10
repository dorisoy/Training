﻿using DryIoc;
using ISTraining_Part.Core.Models;
using ISTraining_Part.Dialogs.Attributes;
using ISTraining_Part.Dialogs.Classes;

namespace ISTraining_Part.Dialogs
{
    /// <summary>
    /// Staff editor view model.
    /// </summary>
    [DialogName(nameof(StaffEditorView))]
    class StaffEditorViewModel : BaseEditModeViewModel<Staff>
    {

        public StaffEditorViewModel()
        {

        }

        public StaffEditorViewModel(Staff staff, bool isEditMode, IContainer container)
            : base(staff, isEditMode, container)
        {
        }
    }
}
