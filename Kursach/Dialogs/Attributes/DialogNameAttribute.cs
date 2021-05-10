using System;

namespace ISTraining_Part.Dialogs.Attributes
{

    [AttributeUsage(AttributeTargets.Class)]
    class DialogNameAttribute : Attribute
    {

        public string ViewName { get; }


        public DialogNameAttribute(string viewName)
        {
            ViewName = viewName;
        }
    }
}
