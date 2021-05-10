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
 
    class DivisionsContingentExporter : BaseExporter, IAsyncExporter<IEnumerable<Group>>
    {

        readonly IClient client;

        public DivisionsContingentExporter(IDialogManager dialogManager, IClient client) : base(dialogManager)
        {
            this.client = client;
        }

        public async Task<bool> Export(IEnumerable<Group> groups)
        {
            if (!SelectFile("各单位定额"))
                return false;

            using (var excel = new ExcelPackage())
            {
                var worksheet = excel.Workbook.Worksheets.Add(DateTime.Now.ToString("MMMM"));

                worksheet.Cells.SetFontName("Arial").SetFontSize(10);

                var now = DateTime.Now;
                int year = now.Month >= 9 ? now.Year + 1 : now.Year - 1;
                int nowYear = now.Month >= 9 ? now.Year : now.Year;

                bool isFirst = now.Month >= 9;
                string years = $"{(isFirst ? nowYear : year)}-{(isFirst ? year : nowYear)}";

                worksheet.Cells["B1:P1"]
                    .SetMerge()
                    .SetValueWithBold($"BBC{years}学生人数", 14)
                    .SetVerticalHorizontalAligment();

                var division0 = groups.Where(x => x.Division == 0); //1-е 分队
                var division1 = groups.Where(x => x.Division == 1); //2-е 分队
                var division2 = groups.Where(x => x.Division == 2); //3-е 分队

                var divisions = new[] { division0, division1, division2 }; //所有单位

                int max = divisions.Max(x => x.Count());
                max += 2; //

                int intramuralCount = 0; //全科学生全科
                int correspondenceCount = 0; //普通函授学校

                //标题
                worksheet.Cells[2, 1, 2, 18].SetBold();

                for (int i = 0; i < 3; i++)
                {
                    var i6 = i * 6;

                    //表格标题
                    SetTableHeader(worksheet, i6, max);

                    var count = await client.Students.GetStudentsCountAsync(divisions[i].Select(x => x.Id)); //在特定部门的学生组
                    var studentsCount = divisions[i].ToDictionary(x => x, x => count.Response[x.Id]);
                    int row = 0; //当前行
                    int intraSabbatical = 0;
                    int corresSabbatical = 0;
                    int intra = 0; 
                    int corres = 0; 
                    foreach (var item in studentsCount)
                    {
          
                        SetGroupInfo(worksheet, row, i6, item);
                        if (item.Key.IsIntramural)
                        {
                            intra += item.Value.Total;
                            intraSabbatical += item.Value.OnSabbatical;
                        }
                        else
                        {
                            corres += item.Value.Total;
                            corresSabbatical += item.Value.OnSabbatical;
                        }
                        row++;
                    }

                    SetTableCount(worksheet, max, i6, intra, corres, intraSabbatical, corresSabbatical);
                    intramuralCount += intra;
                    correspondenceCount += corres;
                }


                SetCountAfterTable(worksheet, max, intramuralCount, correspondenceCount);

                worksheet.Cells.SetVerticalHorizontalAligment();
                worksheet.Cells[2, 1, 2 + max + 1, 18].SetTable();
                worksheet.Cells.AutoFitColumns(5);

                try
                {
                    excel.SaveAs(new FileInfo(FileName));
                    Logger.Log.Info($"出口配额信息: {{fileName: {FileName}}}");
                    return true;
                }
                catch (Exception ex)
                {
                    Logger.Log.Error($"出口配额信息: {{fileName: {FileName}}}", ex);
                    return false;
                }
            }
        }


        void SetTableHeader(ExcelWorksheet worksheet, int i6, int max)
        {
            worksheet.Cells[2, 1 + i6, 2, 2 + i6].SetMerge().SetValue("Группа");
            worksheet.Cells[2, 3 + i6].SetValue("СПО/НПО").SetWrapText();
            worksheet.Cells[2, 4 + i6].SetValue("Б/К");
            worksheet.Cells[2, 5 + i6].SetValue($"На {DateTime.Now.ToString("dd.MM.yyyy")}").SetWrapText();
            worksheet.Cells[2, 6 + i6].SetValue("ак. отп").SetWrapText();

            worksheet.Cells[2, 5 + i6, 2, 6 + i6].SetFontSize(8);

            worksheet.Cells[3, 1 + i6, 3 + max, 1 + i6]
                .SetMerge()
                .SetValue($"{i6 / 6 + 1} подразделение")
                .SetTextRotation(90)
                .SetVerticalHorizontalAligment();
        }

        void SetGroupInfo(ExcelWorksheet worksheet, int row, int i6, KeyValuePair<Group, StudentsCount> keyValuePair)
        {
            worksheet.Cells[3 + row, 2 + i6].SetValueWithBold(keyValuePair.Key.Name); 
            worksheet.Cells[3 + row, 3 + i6].SetValue(SPOHelper.GetStrSpo(keyValuePair.Key.SpoNpo)); 
            worksheet.Cells[3 + row, 4 + i6].SetValue(BudgetHelper.GetStrBudget(keyValuePair.Key.IsBudget)); 
            worksheet.Cells[3 + row, 5 + i6].SetValue(keyValuePair.Value.Total);
            worksheet.Cells[3 + row, 6 + i6].SetValue(keyValuePair.Value.OnSabbatical);
        }


        void SetCountAfterTable(ExcelWorksheet worksheet, int max, int intra, int corres)
        {

            worksheet.Cells[6 + max, 2, 6 + max, 4].SetMerge().SetValueWithBold($"Всего на {DateTime.Now.ToString("dd.MM.yyyy")}:");
            worksheet.Cells[6 + max, 5].SetValueWithBold(intra + corres);

            worksheet.Cells[7 + max, 2].SetValueWithBold("Дневное:");
            worksheet.Cells[7 + max, 5].SetValueWithBold(intra);

  
            worksheet.Cells[8 + max, 2].SetValueWithBold("Заочное:");
            worksheet.Cells[8 + max, 5].SetValueWithBold(corres);
        }

 
        void SetTableCount(ExcelWorksheet worksheet, int max, int i6, int intra, int corres, int intraSabbatical, int corresSabbatical)
        {

            worksheet.Cells[3 + max - 2, 2 + i6, 3 + max - 2, 4 + i6].SetMerge().SetValueWithBold("ВСЕГО:", 12);
            worksheet.Cells[3 + max - 2, 5 + i6].SetValueWithBold(intra + corres, 9.5f);
            worksheet.Cells[3 + max - 2, 6 + i6].SetValueWithBold(intraSabbatical + corresSabbatical, 9.5f);

  
            worksheet.Cells[4 + max - 2, 2 + i6, 4 + max - 2, 4 + i6].SetMerge().SetValueWithBold("очная:", 11);
            worksheet.Cells[4 + max - 2, 5 + i6].SetValueWithBold(intra, 9.5f);
            worksheet.Cells[4 + max - 2, 6 + i6].SetValueWithBold(intraSabbatical, 9.5f);

            worksheet.Cells[5 + max - 2, 2 + i6, 5 + max - 2, 4 + i6].SetMerge().SetValueWithBold("заочная:", 11);
            worksheet.Cells[5 + max - 2, 5 + i6].SetValueWithBold(corres, 9.5f);
            worksheet.Cells[5 + max - 2, 6 + i6].SetValueWithBold(corresSabbatical, 9.5f);
        }
    }
}
