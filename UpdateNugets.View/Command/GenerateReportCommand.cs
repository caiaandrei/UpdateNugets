using Microsoft.Win32;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UpdateNugets.UI.ViewModel;

namespace UpdateNugets.UI.Command
{
    public class GenerateReportCommand : IGenerateReportCommand
    {
        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter) => true;

        public void Execute(object parameter)
        {
            var mainViewModel = parameter as MainViewModel;
            var namesAndCurrentVersions = mainViewModel.ManageNuGets.NuGets.Select(item => $"{item.Name} {item.CurrentVersion.NuGetVersion}").ToList();
            var namesAndInitialVersions = mainViewModel.ManageNuGets.NuGets.Select(item => $"{item.Name} {item.InitialNuGetVersion}").ToList();

            var dialog = new SaveFileDialog();
            string filePath = "";

            try
            {
                dialog.Filter = "Excel file (*.xlsx)|*.xlsx";
                dialog.AddExtension = true;
                dialog.ShowDialog();
                filePath = dialog.FileName;
                GenerateReport(filePath, namesAndInitialVersions, namesAndCurrentVersions);
            }
            catch (Exception ex)
            {
                return;
            }
        }

        private void GenerateReport(string filePath, IList<string> nameAndInitialVersions, List<string> nameAndCurrentVersions)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using (var excel = new ExcelPackage())
            {
                excel.Workbook.Worksheets.Add("My sheet");

                var headerRow = new List<string[]>()
                {
                    new string[]
                    {
                        "Previous Component Dependencies",
                        "Current Component Dependencies"
                    }
                };

                string headerRange = "A1:" + System.Char.ConvertFromUtf32(headerRow[0].Length + 64) + "1";

                var worksheet = excel.Workbook.Worksheets["My sheet"];

                worksheet.Cells[headerRange].LoadFromArrays(headerRow);
                worksheet.Cells[headerRange].Style.Font.Bold = true;

                worksheet.Cells[headerRange].Style.WrapText = true;

                worksheet.Cells[2, 1].LoadFromCollection(nameAndInitialVersions);
                worksheet.Cells[2, 2].LoadFromCollection(nameAndCurrentVersions);

                for (int i = 2; i < worksheet.Cells.Rows; i++)
                {
                    if (worksheet.Cells[i, 1].Text.Length > worksheet.Cells[i - 1, 1].Text.Length)
                    {
                        //worksheet.Cells[i, 1].Style.
                    }
                    if (worksheet.Cells[i, 1].Text != worksheet.Cells[i, 2].Text)
                    {
                        worksheet.Row(i).Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                        worksheet.Row(i).Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Yellow);
                    }
                }

                worksheet.Cells.AutoFitColumns();

                FileInfo excelFile = new FileInfo(filePath);

                excel.SaveAs(excelFile);
            }
        }
    }
}
