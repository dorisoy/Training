using ISTraining_Part.Dialogs.Manager;
using System;

namespace ISTraining_Part.Excel.Classes
{
    class BaseExporter
    {
      
        readonly IDialogManager dialogManager;


        public string FileName { get; private set; }

        public BaseExporter(IDialogManager dialogManager)
        {
            this.dialogManager = dialogManager;
        }


        public bool SelectFile(string defName)
        {
            string fileName = dialogManager.SelectExportFileName(defName);

            FileName = fileName;

            return !fileName.IsEmpty();
        }
    }
}
