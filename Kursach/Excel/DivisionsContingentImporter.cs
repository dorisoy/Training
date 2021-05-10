using ISTraining_Part.Client.Interfaces;
using ISTraining_Part.Core.Models;
using ISTraining_Part.Dialogs.Manager;
using ISTraining_Part.Excel.Classes;
using ISTraining_Part.Excel.Interfaces;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ISTraining_Part.Excel
{

    class DivisionsContingentImporter : BaseImporter, IAsyncImporter<IEnumerable<Group>>
    {

        readonly IClient client;


        public DivisionsContingentImporter(IClient client, IDialogManager dialogManager) : base(dialogManager)
        {
            this.client = client;
        }


        public async Task<IEnumerable<Group>> Import()
        {
            if (!SelectFile())
                return null;

            try
            {
                using (var excel = new ExcelPackage(new FileInfo(FileName)))
                {
                    var worksheet = excel.Workbook.Worksheets[1];

        
                    var groups = new List<Group>();

   
                    var res = await client.Staff.GetOrCreateFirstStaffAsync();
                    if (!res)
                    {
                        return Enumerable.Empty<Group>();
                    }
                    int curator = res.Response.Id;

                    for (int i = 0; i < 3; i++)
                    {
                        int i6 = i * 6;

                        int row = 0; 
                        while (true)
                        {
                            int currentRow = 3 + row; 
                            string name = worksheet.Cells[currentRow, 2 + i6].GetValue<string>() ?? "";


                            if (!name.IsMatch("^[а-z-Z]{1,3}-[0-9]{2}$"))
                                break;

                            var group = new Group
                            {
                                Name = name,
                                SpoNpo = SPOHelper.GetIntSpo(worksheet.Cells[currentRow, 3 + i6].GetValue<string>()), 
                                IsBudget = BudgetHelper.GetBoolBudget(worksheet.Cells[currentRow, 4 + i6].GetValue<string>()), 
                                Division = i,
                                End = DateTime.Now,
                                Start = DateTime.Now,
                                Specialty = "Специальность",
                                CuratorId = curator
                            };
                            groups.Add(group);
                            row++;
                        }
                    }

                    Logger.Log.Info($"输入定额数据: {{fileName: {FileName}}}");

                    return groups;
                }
            }
            catch (Exception ex)
            {
                Logger.Log.Error($"输入定额数据: {{fileName: {FileName}}}", ex);

                return null;
            }
        }
    }
}
