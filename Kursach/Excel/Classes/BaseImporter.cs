using ISTraining_Part.Dialogs.Manager;
using System;

namespace ISTraining_Part.Excel.Classes
{
    class BaseImporter
    {

        readonly IDialogManager dialogManager;

        public string FileName { get; private set; }

        public BaseImporter(IDialogManager dialogManager)
        {
            this.dialogManager = dialogManager;
        }


        public bool SelectFile()
        {
            string fileName = dialogManager.SelectImportFile();

            FileName = fileName;

            return !fileName.IsEmpty();
        }
    }
}
