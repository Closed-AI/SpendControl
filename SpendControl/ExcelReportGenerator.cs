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
        #region RowAndColumnConsts
        private const int START_ROW = 2;
        private const int START_COLUMN = 2;
        private const int END_COLUMN = 5;

        private const int CATEGORY_COLUMN = 2;
        private const int VALUE_COLUMN = 3;
        private const int DATE_COLUMN = 4;
        private const int DESCRIPTION_COLUMN = 5;
        #endregion

        #region ColorConsts
        private const string HEADER_COLOR = "#66a3e8";
        private const string GAIN_OPERATION_COLOR = "#00ff7f";
        private const string SPEND_OPERATION_COLOR = "#ff5757";
        #endregion


        public static void MakeReport(List<Operation> data)
        {
            var package = new ExcelPackage();

            var sheet = package.Workbook.Worksheets.Add("История операций");

            // шапка таблицы
            var header = sheet.Cells[START_ROW, START_COLUMN, START_ROW, END_COLUMN];

            header.LoadFromArrays(new object[][] { new[] { "Категория", "Сумма", "Дата", "Описание" } });
            header.Style.Fill.PatternType = ExcelFillStyle.Solid;
            header.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml(HEADER_COLOR));
            //--------------------------

            var row = 3;

            foreach (var op in data)
            {
                sheet.Cells[row, CATEGORY_COLUMN].Value = op.Category;
                sheet.Cells[row, VALUE_COLUMN].Value = op.Value;
                sheet.Cells[row, DATE_COLUMN].Value = op.OperationDate;
                sheet.Cells[row, DESCRIPTION_COLUMN].Value = op.Description;

                var dataRow = sheet.Cells[row, START_COLUMN, row, END_COLUMN];

                var color = op.Type == "Доход" ?
                    GAIN_OPERATION_COLOR :  // подсветка доходов
                    SPEND_OPERATION_COLOR;  // подсветка расходов

                dataRow.Style.Fill.PatternType = ExcelFillStyle.Solid;
                dataRow.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml(color));

                row++;
            }
            row--;

            // форматирование
            // автоширина ячеек
            sheet.Cells[START_ROW, START_COLUMN, row, END_COLUMN].AutoFitColumns();
            // формат даты
            var DATA_START_ROW = START_ROW - 1;

            sheet.Cells[DATA_START_ROW, DATE_COLUMN, row, DATE_COLUMN].Style.Numberformat.Format = "dd.mm.yy";
            //--------------------------

            MakeFile(package.GetAsByteArray());
        }

        private static void MakeFile(byte[] data)
        {
            SaveFileDialog dialog = new SaveFileDialog();

            // настройки диалога
            dialog.Title = "Сохранение отчёта об операциях";
            dialog.DefaultExt = ".xlsx";
            dialog.CheckPathExists = true;
            //--------------------------

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                File.WriteAllBytes(dialog.FileName, data);

                MessageBox.Show("Отчёт сохранён");
            }
        }
    }
}
