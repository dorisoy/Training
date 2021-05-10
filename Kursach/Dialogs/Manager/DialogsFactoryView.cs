using DryIoc;
using ISTraining_Part.Dialogs.Attributes;
using System;
using System.Windows;

namespace ISTraining_Part.Dialogs.Manager
{

    class DialogsFactoryView : IDialogsFactoryView
    {

        readonly IContainer container;

        public DialogsFactoryView(IContainer container)
        {
            this.container = container;
        }


        public object GetView<TViewModel>(TViewModel viewModel)
        {
            var atr = (DialogNameAttribute)Attribute.GetCustomAttribute(typeof(TViewModel), typeof(DialogNameAttribute));

            if (atr == null)
            {
                Logger.Log.Error($"DialogNameAttribute is null: {{viewModel: {viewModel}}}");
                return null;
            }

            var view = container.Resolve<FrameworkElement>(atr.ViewName);

            if (view.GetType().Name == nameof(FrameworkElement))
            {
                Logger.Log.Error($"GetView<T> view is null: {{viewModel: {viewModel}}}");
                return null;
            }

            view.DataContext = viewModel;

            return view;
        }
    }
}
