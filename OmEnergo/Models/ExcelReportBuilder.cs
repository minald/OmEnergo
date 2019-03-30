using ClosedXML.Excel;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace OmEnergo.Models
{
    public class ExcelReportBuilder
    {
        private Repository Repository { get; }

        public ExcelReportBuilder(Repository repository) => Repository = repository;

        public void CreateDatabaseBackup(string backupPath)
        {
            var workbook = new XLWorkbook();
            workbook.Worksheets.Add(GetSectionsTable());
            workbook.Worksheets.Add(GetProductsTable());
            workbook.Worksheets.Add(GetProductModelsTable());
            workbook.Worksheets.Add(GetConfigKeysTable());
            workbook.SaveAs(backupPath);
        }

        private DataTable GetSectionsTable()
        {
            DataTable table = new DataTable("Sections");

            var columnNames = new List<string>() { "Id", "Name", "EnglishName", "SequenceNumber", "Description",
                "ParentSectionId", "ProductProperties", "ProductModelProperties"};
            columnNames.ForEach(x => table.Columns.Add(x));

            var sections = Repository.GetAllSections().ToList();
            sections.ForEach(x => table.Rows.Add(x.Id, x.Name, x.EnglishName, x.SequenceNumber,
                x.Description, x.ParentSection?.Id, x.ProductProperties, x.ProductModelProperties));

            return table;
        }

        private DataTable GetProductsTable()
        {
            DataTable table = new DataTable("Products");

            var columnNames = new List<string>() { "Id", "Name", "EnglishName", "SequenceNumber", "Description",
                "SectionId", "Properties"};
            columnNames.ForEach(x => table.Columns.Add(x));

            var products = Repository.GetAllProducts().ToList();
            products.ForEach(x => table.Rows.Add(x.Id, x.Name, x.EnglishName, x.SequenceNumber,
                x.Description, x.Section?.Id, x.Properties));

            return table;
        }

        private DataTable GetProductModelsTable()
        {
            DataTable table = new DataTable("ProductModels");

            var columnNames = new List<string>() { "Id", "Name", "EnglishName", "SequenceNumber", "Price",
                "ParentId", "SectionId", "Properties"};
            columnNames.ForEach(x => table.Columns.Add(x));

            var productModels = Repository.GetAllProductModels().ToList();
            productModels.ForEach(x => table.Rows.Add(x.Id, x.Name, x.EnglishName, x.SequenceNumber, 
                x.Price, x.Product?.Id, x.Section?.Id, x.Properties));

            return table;
        }

        private DataTable GetConfigKeysTable()
        {
            DataTable table = new DataTable("ConfigKeys");

            var columnNames = new List<string>() { "Id", "Key", "Value"};
            columnNames.ForEach(x => table.Columns.Add(x));

            var configKeys = Repository.GetAllConfigKeys().ToList();
            configKeys.ForEach(x => table.Rows.Add(x.Id, x.Key, x.Value));

            return table;
        }
    }
}
