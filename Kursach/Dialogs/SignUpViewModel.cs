using DryIoc;
using ISTraining_Part.Core.Models;
using ISTraining_Part.Dialogs.Attributes;
using ISTraining_Part.Dialogs.Classes;

namespace ISTraining_Part.Dialogs
{
    /// <summary>
    /// Signup view model.
    /// </summary>
    [DialogName(nameof(SignUpView))]
    class SignUpViewModel : BaseEditModeViewModel<User>
    {
        public SignUpViewModel()
        {

        }


        public SignUpViewModel(User user, bool isEditMode, IContainer container)
            : base(user, isEditMode, container)
        {
        }
    }
}
