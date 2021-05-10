using DevExpress.Mvvm;
using ISTraining_Part.Core.Models;

namespace ISTraining_Part.ViewModels.Interfaces
{

    interface IDetail
    {
        ICommand<People> ShowDetailInfoCommand { get; }
        ICommand<People> ShowDetailInfoEditorCommand { get; }
    }
}
