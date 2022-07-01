using System;
using System.Collections.Generic;
using System.IO;
using OfficeOpenXml;
using OfficeOpenXml.Drawing.Chart;
using OfficeOpenXml.Style;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using Ookii.Dialogs.Wpf;
using System.Windows.Forms;

namespace SpendControl
{
    static class ExcelReportGenerator
    {
        public static void MakeReport(List<Operation> data)
        {
            var package = new ExcelPackage();

            var sheet = package.Workbook.Worksheets.Add("История операций");

            // шапка таблицы
            sheet.Cells[2, 2, 2, 5].LoadFromArrays(new object[][] { new[] { "Категория", "Сумма", "Дата", "Описание" } });
            sheet.Cells[2, 2, 2, 5].Style.Fill.PatternType = ExcelFillStyle.Solid;
            sheet.Cells[2, 2, 2, 5].Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#66a3e8"));  // blue color

            var row = 3;

            foreach (var op in data)
            {
                sheet.Cells[row, 2].Value = op.Category;
                sheet.Cells[row, 3].Value = op.Value;
                sheet.Cells[row, 4].Value = op.OperationDate;
                sheet.Cells[row, 5].Value = op.Description;

                sheet.Cells[row, 2, row, 5].Style.Fill.PatternType = ExcelFillStyle.Solid;

                if (op.Type == "Доход") // green color
                    sheet.Cells[row, 2, row, 5].Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#00ff7f"));
                else                    // red color
                    sheet.Cells[row, 2, row, 5].Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#ff5757"));

                row++;
            }
            row--;

            //--------------------------//
            //      форматирование      //
            //--------------------------//
            // автоширина ячеек

            sheet.Cells[2, 2, row, 5].AutoFitColumns();

            // формат даты
            sheet.Cells[3, 4, row, 4].Style.Numberformat.Format = "dd.mm.yy";
            //--------------------------//

            MakeFile(package.GetAsByteArray());
        }

        private static void MakeFile(byte[] data)
        {
            SaveFileDialog dialog = new SaveFileDialog();

            // настройки диалога
            dialog.Title = "Сохранение отчёта об операциях";
            dialog.DefaultExt = ".xlsx";
            dialog.CheckPathExists = true;
            //--------------------------//

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                File.WriteAllBytes(dialog.FileName, data);

                MessageBox.Show("Отчёт сохранён");
            }
        }
    }
}
