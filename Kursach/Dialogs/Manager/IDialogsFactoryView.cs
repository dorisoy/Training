namespace ISTraining_Part.Dialogs.Manager
{

    interface IDialogsFactoryView
    {

        object GetView<TViewModel>(TViewModel viewModel);
    }
}
