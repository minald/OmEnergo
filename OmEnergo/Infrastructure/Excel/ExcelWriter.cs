using ClosedXML.Excel;
using OmEnergo.Infrastructure.Database;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace OmEnergo.Infrastructure.Excel
{
    public class ExcelWriter
    {
        public const string XlsxMimeType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

        private Repository _Repository { get; }

        public ExcelWriter(Repository repository) => _Repository = repository;

        public MemoryStream CreateExcelStream()
        {
            var workbook = new XLWorkbook();
            AddWorksheet(workbook, _Repository.GetAllSections());
            AddWorksheet(workbook, _Repository.GetAllProducts());
            AddWorksheet(workbook, _Repository.GetAllProductModels());
            AddWorksheet(workbook, _Repository.GetAllConfigKeys());
            SetCellsWidthAndHeight(workbook);
            return ToMemoryStream(workbook);
        }

        private void AddWorksheet<T>(XLWorkbook workbook, IEnumerable<T> objects)
        {
            var dataTableCreator = new DataTableCreatorFromIEnumerable<T>();
            DataTable dataTable = dataTableCreator.Create(objects);
            workbook.Worksheets.Add(dataTable);
        }

        private void SetCellsWidthAndHeight(XLWorkbook xlWorkbook)
        {
            xlWorkbook.Worksheets.ToList().ForEach(x => x.Rows(1, 10000).Height = 15);
            foreach (IXLWorksheet worksheet in xlWorkbook.Worksheets)
            {
                worksheet.Columns(1, 50).ToList().ForEach(c => SetColumnWidth(c));
            }
        }

        private void SetColumnWidth(IXLColumn column)
        {
            IEnumerable<IXLCell> cellsUsed = column.CellsUsed().Skip(1);
            if (column.FirstCell().Value as string == "Id")
            {
                column.Hide();
            }
            else
            {
                bool isAllValuesAreNumbersOrEmpty = cellsUsed.Count() == 0 ||
                cellsUsed.All(x => Regex.IsMatch(x.Value as string, "^[0-9\\.\\,]+$"));
                column.Width = isAllValuesAreNumbersOrEmpty ? 10 : 60;
            }
        }

        private MemoryStream ToMemoryStream(XLWorkbook xlWorkbook)
        {
            var memoryStream = new MemoryStream();
            xlWorkbook.SaveAs(memoryStream, false);
            memoryStream.Position = 0;
            return memoryStream;
        }
    }
}
